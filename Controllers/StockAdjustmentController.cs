using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreManagement_Project.Data;
using StoreManagement_Project.Models;

namespace StoreManagement_Project.Controllers
{
    public class StockAdjustmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockAdjustmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StockAdjustment
        public async Task<IActionResult> Index()
        {
            var adjustments = await _context.StockAdjustments
                .Include(sa => sa.Stock)
                .ThenInclude(s => s.Item)
                .ToListAsync();
            return View(adjustments);
        }

        // GET: StockAdjustment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adjustment = await _context.StockAdjustments
                .Include(sa => sa.Stock)
                .ThenInclude(s => s.Item)
                .FirstOrDefaultAsync(m => m.StockAdjustmentId == id);

            if (adjustment == null)
            {
                return NotFound();
            }

            return View(adjustment);
        }

        // GET: StockAdjustment/Create
        public IActionResult Create()
        {
            // Verify the SelectList items have correct Value and Text
            ViewBag.Stocks = new SelectList(
                _context.Stocks
                    .Include(s => s.Item)
                    .Select(s => new {
                        Value = s.StockId.ToString(), // Ensure this is string
                        Text = $"{s.Item.ItemName} (Available: {s.Quantity})"
                    }),
                "Value", // This must match the property name above
                "Text"    // This must match the property name above
            );

            ViewBag.AdjustmentTypes = new SelectList(new[] {
                  new { Value = "Increase", Text = "Increase" },
                  new { Value = "Decrease", Text = "Decrease" }
            }, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StockId,AdjustedQuantity,AdjustmentReason,AdjustmentType,Requester")] StockAdjustment adjustment)
        {
            ModelState.Remove("Stock");
            if (ModelState.IsValid)
            {
                try
                {
                    var stock = await _context.Stocks.FindAsync(adjustment.StockId);
                    if (stock == null)
                    {
                        ModelState.AddModelError("StockId", "Invalid stock selected");
                        return View(adjustment);
                    }

                    // Apply adjustment logic
                    if (adjustment.AdjustmentType == "Increase")
                    {
                        stock.Quantity += adjustment.AdjustedQuantity;
                    }
                    else
                    {
                        if (stock.Quantity < adjustment.AdjustedQuantity)
                        {
                            ModelState.AddModelError("AdjustedQuantity", "Cannot decrease more than available quantity");
                            return View(adjustment);
                        }
                        stock.Quantity -= adjustment.AdjustedQuantity;
                    }

                    adjustment.AdjustmentDate = DateTime.Now;
                    adjustment.ApprovalStatus = ApprovalStatus.Pending;

                    _context.Add(adjustment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the error
                    ModelState.AddModelError("", "An error occurred while saving: " + ex.Message);
                }
            }

            // Repopulate dropdowns if there's an error
            ViewBag.Stocks = new SelectList(_context.Stocks
                .Include(s => s.Item)
                .Select(s => new
                {
                    s.StockId,
                    Display = $"{s.Item.ItemName} (Available: {s.Quantity})"
                }), "StockId", "Display");

            ViewBag.AdjustmentTypes = new SelectList(new[] { "Increase", "Decrease" });

            return View(adjustment);
        }

        // GET: StockAdjustment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adjustment = await _context.StockAdjustments.FindAsync(id);
            if (adjustment == null)
            {
                return NotFound();
            }

            ViewBag.Stocks = new SelectList(_context.Stocks
                .Include(s => s.Item)
                .Select(s => new
                {
                    s.StockId,
                    Display = $"{s.Item.ItemName} (Stock ID: {s.StockId})"
                }), "StockId", "Display", adjustment.StockId);

            ViewBag.AdjustmentTypes = new SelectList(new[] { "Increase", "Decrease" }, adjustment.AdjustmentType);

            return View(adjustment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StockAdjustmentId,StockId,AdjustedQuantity,AdjustmentReason,AdjustmentType,Requester,AdjustmentDate,ApprovalStatus")] StockAdjustment adjustment)
        {
            if (id != adjustment.StockAdjustmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adjustment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockAdjustmentExists(adjustment.StockAdjustmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(adjustment);
        }

        // GET: StockAdjustment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adjustment = await _context.StockAdjustments
                .Include(sa => sa.Stock)
                .ThenInclude(s => s.Item)
                .FirstOrDefaultAsync(m => m.StockAdjustmentId == id);

            if (adjustment == null)
            {
                return NotFound();
            }

            return View(adjustment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adjustment = await _context.StockAdjustments.FindAsync(id);
            if (adjustment != null)
            {
                _context.StockAdjustments.Remove(adjustment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Approval Action
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var adjustment = await _context.StockAdjustments.FindAsync(id);
            if (adjustment == null)
            {
                return NotFound();
            }

            adjustment.ApprovalStatus = ApprovalStatus.Approved;
            adjustment.ApprovedBy = User.Identity?.Name;
            adjustment.ApprovalDate = DateTime.Now;

            _context.Update(adjustment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool StockAdjustmentExists(int id)
        {
            return _context.StockAdjustments.Any(e => e.StockAdjustmentId == id);
        }
    }
}