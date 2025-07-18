using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreManagement_Project.Data;
using StoreManagement_Project.Models;

namespace StoreManagement_Project.Controllers
{
    public class PurchaseRequisitionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseRequisitionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PurchaseRequisition
        public async Task<IActionResult> Index()
        {
            var requisitions = await _context.PurchaseRequisitions
                .Include(r => r.PurchaseRequisitionItems)
                .OrderByDescending(r => r.Date)
                .ToListAsync();

            return View(requisitions);
        }

        // GET: PurchaseRequisition/Create
        public IActionResult Create()
        {
            //ViewBag.Items = new SelectList(_context.Items.Include(i => i.Unit), "ItemId", "ItemName");
            var requisitionItems = _context.RequisitionItems
                   .Include(ri => ri.Item)
                   .Include(ri => ri.Unit)
                   .Include(ri => ri.Requisition)
                   .Where(ri => !ri.IsFullyIssued && ri.Requisition.Status == "ForwardedToPurchase") // or "Approved"
                   .ToList();

            ViewBag.RequisitionItems = requisitionItems;


            ViewBag.Units = new SelectList(_context.Units, "UnitId", "NameOfUnit");

            var model = new PurchaseRequisition
            {
                Date = DateTime.Today,
                PurchaseRequisitionNumber = GenerateRequisitionNumber(),
                PurchaseRequisitionItems = new List<PurchaseRequisitionItem> { new PurchaseRequisitionItem() }
            };

            return View(model);
        }

        // POST: PurchaseRequisition/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseRequisition model)
        {
            if (ModelState.IsValid)
            {
                model.PurchaseRequisitionNumber = GenerateRequisitionNumber();

                if (model.PurchaseRequisitionItems != null)
                {

                    // ✅ Fill ItemId, UnitId, and Quantity from RequisitionItem
                    foreach (var item in model.PurchaseRequisitionItems)
                    {
                        var requisitionItem = await _context.RequisitionItems
                            .Include(ri => ri.Item)
                                .ThenInclude(i => i.Unit)
                            .FirstOrDefaultAsync(ri => ri.RequisitionItemId == item.RequisitionItemId);

                        if (requisitionItem != null)
                        {
                            item.ItemId = requisitionItem.ItemId;
                            item.UnitId = requisitionItem.Item?.UnitId ?? 0;
                            item.Quantity = requisitionItem.RemainingQuantity;
                            requisitionItem.IsFullyIssued = requisitionItem.QuantityRequested <= requisitionItem.QuantityIssued + item.Quantity;
                        }
                    }
                }

                _context.PurchaseRequisitions.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { id = model.PurchaseRequisitionId });
            }

            // Reload ViewBag values for invalid model state
            var requisitionItems = _context.RequisitionItems?
                .Include(ri => ri.Item)
                .Include(ri => ri.Unit)
                .Include(ri => ri.Requisition)
                .Where(ri => !ri.IsFullyIssued && ri.Requisition.Status == "ForwardedToPurchase")
                .ToList() ?? new List<RequisitionItem>();
            ViewBag.RequisitionItems = requisitionItems;
            //ViewBag.Units = new SelectList(_context.Units, "UnitId", "NameOfUnit");
            ViewBag.Units = new SelectList(_context.Units?.ToList() ?? new List<Unit>(), "UnitId", "NameOfUnit");


            return View(model);
        }


        private string GenerateRequisitionNumber()
        {
            var datePart = DateTime.Now.ToString("yyyyMMdd");
            var count = _context.PurchaseRequisitions.Count(r => r.Date.Date == DateTime.Today) + 1;
            return $"PR-{datePart}-{count:D4}";
        }

        // GET: PurchaseRequisition/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var requisition = await _context.PurchaseRequisitions
                .Include(r => r.PurchaseRequisitionItems)
                    .ThenInclude(ri => ri.Item)
                .Include(r => r.PurchaseRequisitionItems)
                    .ThenInclude(ri => ri.Unit)
                .FirstOrDefaultAsync(r => r.PurchaseRequisitionId == id);

            if (requisition == null)
                return NotFound();

            return View(requisition);
        }
    }
}
