using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreManagement_Project.Data;
using StoreManagement_Project.Models;

namespace StoreManagement_Project.Controllers
{
    public class RequisitionIssueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequisitionIssueController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RequisitionIssue/Create
        public IActionResult Create(int requisitionId)
        {
            var requisition = _context.Requisitions
                .Include(r => r.RequisitionItems)
                .ThenInclude(ri => ri.Item)
                .Include(r => r.RequisitionItems)
                .ThenInclude(ri => ri.Unit)
                .FirstOrDefault(r => r.RequisitionId == requisitionId);

            if (requisition == null) return NotFound();

            ViewBag.Requisition = requisition;
            ViewBag.Warehouses = _context.Warehouses.ToList();
            ViewBag.LocationComponents = _context.LocationComponents.ToList();
            return View();
        }

        // POST: RequisitionIssue/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RequisitionIssue issue)
        {
            if (!ModelState.IsValid)
                return View(issue);

            issue.IssueNumber = GenerateIssueNumber(issue.RequisitionId);
            _context.RequisitionIssues.Add(issue);

            foreach (var issueItem in issue.RequisitionIssueItems)
            {
                // Update RequisitionItem
                var reqItem = await _context.RequisitionItems.FindAsync(issueItem.RequisitionItemId);
                if (reqItem == null) continue;

                reqItem.QuantityIssued += issueItem.QuantityIssued;
                reqItem.IsFullyIssued = reqItem.QuantityIssued >= reqItem.QuantityRequested;

                // Update Stock
                var stock = await _context.Stocks.FirstOrDefaultAsync(s =>
                    s.ItemId == issueItem.ItemId &&
                    s.UnitId == issueItem.UnitId &&
                    s.WarehouseId == issueItem.WarehouseId &&
                    s.LocationComponentId == issueItem.LocationComponentId);

                if (stock != null)
                {
                    stock.Quantity -= issueItem.QuantityIssued;
                }
            }

            // Update Requisition status
            var requisition = await _context.Requisitions
                .Include(r => r.RequisitionItems)
                .FirstOrDefaultAsync(r => r.RequisitionId == issue.RequisitionId);

            if (requisition != null)
                requisition.Status = GetUpdatedStatus(requisition.RequisitionItems);

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Requisition", new { id = issue.RequisitionId });
        }

        private string GenerateIssueNumber(int requisitionId)
        {
            var datePart = DateTime.Now.ToString("yyyyMMdd");
            var count = _context.RequisitionIssues
                .Count(r => r.RequisitionId == requisitionId && r.IssueDate.Date == DateTime.Today) + 1;

            return $"ISSUE-REQ-{datePart}-{count.ToString("D4")}";
        }

        private string GetUpdatedStatus(IEnumerable<RequisitionItem> items)
        {
            if (items.All(i => i.IsFullyIssued))
                return "FullyIssued";
            if (items.Any(i => i.QuantityIssued > 0))
                return "PartiallyIssued";
            return "Pending";
        }
        // GET: RequisitionIssue
        public async Task<IActionResult> Index()
        {
            var issues = await _context.RequisitionIssues
                .Include(r => r.Requisition)
                .Include(r => r.RequisitionIssueItems)
                    .ThenInclude(rii => rii.Item)
                .Include(r => r.RequisitionIssueItems)
                    .ThenInclude(rii => rii.Warehouse) // ✅ ঠিক
                .Include(r => r.RequisitionIssueItems)
                    .ThenInclude(rii => rii.Unit)
                .Include(r => r.RequisitionIssueItems)
                    .ThenInclude(rii => rii.LocationComponent) // ✅ ঠিক
                .ToListAsync();

            return View(issues);
        }
        // GET: RequisitionIssue/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var issue = await _context.RequisitionIssues
                .Include(r => r.Requisition)
                .Include(r => r.RequisitionIssueItems)
                    .ThenInclude(rii => rii.Item)
                .Include(r => r.RequisitionIssueItems)
                    .ThenInclude(rii => rii.Unit)
                .Include(r => r.RequisitionIssueItems)
                    .ThenInclude(rii => rii.Warehouse) // ✅ Include Warehouse
                .Include(r => r.RequisitionIssueItems)
                    .ThenInclude(rii => rii.LocationComponent) // ✅ Include LocationComponent
                .FirstOrDefaultAsync(r => r.RequisitionIssueId == id);


            if (issue == null)
                return NotFound();

            return View(issue);
        }

    }
}