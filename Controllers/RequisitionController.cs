using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreManagement_Project.Data;
using StoreManagement_Project.Models;

namespace StoreManagement_Project.Controllers
{
    public class RequisitionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequisitionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            ViewBag.Employees = new SelectList(_context.Employees, "EmployeeId", "EmployeeName");
            ViewBag.Items = new SelectList(_context.Items.Include(i => i.Unit), "ItemId", "ItemName");
            ViewBag.Units = new SelectList(_context.Units, "UnitId", "NameOfUnit");

            var requisition = new Requisition
            {
                RequisitionDate = DateTime.Today,
                RequisitionNumber = GenerateRequisitionNumber()
            };

            return View(requisition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Requisition requisition)
        {
            if (ModelState.IsValid)
            {
                requisition.RequisitionNumber = GenerateRequisitionNumber();
                requisition.Status = "Pending";

                foreach (var item in requisition.RequisitionItems)
                {
                    item.IsFullyIssued = item.QuantityRequested <= item.QuantityIssued;
                }

                _context.Requisitions.Add(requisition);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = requisition.RequisitionId });
            }

            ViewBag.Employees = new SelectList(_context.Employees, "EmployeeId", "EmployeeName");
            ViewBag.Items = new SelectList(_context.Items.Include(i => i.Unit), "ItemId", "ItemName");
            ViewBag.Units = new SelectList(_context.Units, "UnitId", "NameOfUnit");
            return View(requisition);
        }

        private string GenerateRequisitionNumber()
        {
            var datePart = DateTime.Now.ToString("yyyyMMdd");
            var count = _context.Requisitions.Count(r => r.RequisitionDate.Date == DateTime.Today) + 1;
            return $"REQ-{datePart}-{count.ToString("D4")}";
        }

        public async Task<IActionResult> Details(int id)
        {
            var requisition = await _context.Requisitions
                .Include(r => r.Employee)
                .Include(r => r.RequisitionItems).ThenInclude(ri => ri.Item)
                .Include(r => r.RequisitionItems).ThenInclude(ri => ri.Unit)
                .FirstOrDefaultAsync(r => r.RequisitionId == id);

            if (requisition == null)
                return NotFound();

            return View(requisition);
        }

        [HttpPost]
        public async Task<IActionResult> IssueItems(int requisitionId)
        {
            var requisition = await _context.Requisitions
                .Include(r => r.RequisitionItems)
                .FirstOrDefaultAsync(r => r.RequisitionId == requisitionId);

            if (requisition == null)
                return NotFound();

            // ✅ Check if PR already exists
            var existingPR = await _context.PurchaseRequisitions
                .FirstOrDefaultAsync(pr => pr.RequisitionId == requisitionId);

            if (existingPR != null)
            {
                TempData["ErrorMessage"] = "This requisition already has a purchase requisition.";
                return RedirectToAction("Details", new { id = requisitionId });
            }

            var issueItems = new List<RequisitionIssueItem>();
            bool needPurchase = false;
            string prNumber = "";
            PurchaseRequisition? purchaseRequisition = null;

            foreach (var item in requisition.RequisitionItems)
            {
                if (item.IsFullyIssued)
                    continue;

                var remainingQty = item.QuantityRequested - item.QuantityIssued;
                if (remainingQty <= 0)
                    continue;

                var stock = await _context.Stocks
                    .FirstOrDefaultAsync(s => s.ItemId == item.ItemId);
                var availableStock = stock?.Quantity ?? 0;

                var issueQty = Math.Min(availableStock, remainingQty);

                // ✅ ইস্যু করার মতো কিছু থাকলে
                if (issueQty > 0)
                {
                    item.QuantityIssued += issueQty;
                    item.IsFullyIssued = item.QuantityIssued >= item.QuantityRequested;

                    if (stock != null)
                        stock.Quantity -= issueQty;

                    issueItems.Add(new RequisitionIssueItem
                    {
                        RequisitionItemId = item.RequisitionItemId,
                        ItemId = item.ItemId,
                        UnitId = item.UnitId,
                        QuantityIssued = issueQty
                    });
                }

                // ✅ বাকি থাকলে PR তৈরি করুন
                var stillRemaining = item.QuantityRequested - item.QuantityIssued;
                if (stillRemaining > 0)
                {
                    if (!needPurchase)
                    {
                        prNumber = await GeneratePurchaseRequisitionNumberAsync();
                        purchaseRequisition = new PurchaseRequisition
                        {
                            RequisitionId = requisition.RequisitionId,
                            Date = DateTime.Now,
                            PurchaseRequisitionNumber = prNumber
                        };
                        _context.PurchaseRequisitions.Add(purchaseRequisition);
                        await _context.SaveChangesAsync();
                        needPurchase = true;
                    }

                    _context.PurchaseRequisitionItems.Add(new PurchaseRequisitionItem
                    {
                        ItemId = item.ItemId,
                        Quantity = stillRemaining,
                        UnitId = item.UnitId,
                        RequisitionItemId = item.RequisitionItemId,
                        PurchaseRequisitionId = purchaseRequisition!.PurchaseRequisitionId
                    });
                }
            }

            // ✅ অন্তত একটি issue item থাকলে, তখনই Issue তৈরি করুন
            if (issueItems.Any())
            {
                string isuNumber = await GenerateIssueNumberAsync();
                var requisitionIssue = new RequisitionIssue
                {
                    RequisitionId = requisitionId,
                    IssueNumber = isuNumber,
                    IssueDate = DateTime.Now
                };
                _context.RequisitionIssues.Add(requisitionIssue);
                await _context.SaveChangesAsync();

                foreach (var issueItem in issueItems)
                {
                    issueItem.RequisitionIssueId = requisitionIssue.RequisitionIssueId;
                    _context.RequisitionIssueItems.Add(issueItem);
                }
            }

            // ✅ Requisition status আপডেট করুন
            requisition.Status = GetUpdatedStatus(requisition.RequisitionItems);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = requisitionId });
        }

        private async Task<string> GeneratePurchaseRequisitionNumberAsync()
        {
            var today = DateTime.Today;
            var datePart = today.ToString("yyyyMMdd"); // e.g. 20250514

            var lastPR = await _context.PurchaseRequisitions
                .Where(pr => pr.Date.Date == today)
                .OrderByDescending(pr => pr.PurchaseRequisitionId)
                .FirstOrDefaultAsync();

            int nextSerial = 1;
            if (lastPR != null && lastPR.PurchaseRequisitionNumber != null)
            {
                var parts = lastPR.PurchaseRequisitionNumber.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastSerial))
                {
                    nextSerial = lastSerial + 1;
                }
            }

            return $"PR-{datePart}-{nextSerial.ToString("D4")}";
        }
        private async Task<string> GenerateIssueNumberAsync()
        {
            string datePart = DateTime.Today.ToString("yyyyMMdd");
            int count = _context.RequisitionIssues.Count(r => r.IssueDate.Date == DateTime.Today) + 1;
            return $"ISSUE-REQ-{datePart}-{count.ToString("D4")}";
        }

        private string GetUpdatedStatus(IEnumerable<RequisitionItem> items)
        {
            if (items.All(i => i.IsFullyIssued)) return "Fully Issued";
            if (items.Any(i => i.QuantityIssued > 0)) return "Partially Issued";
            return "Pending";
        }

        private decimal GetAvailableStock(int itemId)
        {
            return 100; // Replace with real stock logic
        }

        [HttpGet]
        public IActionResult GetUnitByItem(int itemId)
        {
            var unit = _context.Items
                .Include(i => i.Unit)
                .Where(i => i.ItemId == itemId)
                .Select(i => new { i.Unit.UnitId, i.Unit.NameOfUnit })
                .FirstOrDefault();

            if (unit == null)
                return NotFound();

            return Json(unit);
        }


        // GET: Requisition
        public async Task<IActionResult> Index()
        {
            var requisitions = await _context.Requisitions
                .Include(r => r.Employee)
                .OrderByDescending(r => r.RequisitionDate)
                .ToListAsync();

            return View(requisitions);
        }
    }
}

