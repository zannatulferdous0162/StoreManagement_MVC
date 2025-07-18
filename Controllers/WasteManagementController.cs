using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreManagement_Project.Data;
using StoreManagement_Project.Models;

namespace StoreManagement_Project.Controllers
{
    public class WasteManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WasteManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WasteManagement
        public async Task<IActionResult> Index()
        {
            var wastes = await _context.WasteManagements
                .Include(w => w.Stock)
                .ThenInclude(s => s.Item)
                .ToListAsync();
            return View(wastes);
        }

        // GET: WasteManagement/Create
        public IActionResult Create()
        {
            ViewBag.Stocks = new SelectList(_context.Stocks
                .Include(s => s.Item)
                .Where(s => s.Quantity > 0)
                .Select(s => new {
                    s.StockId,
                    Display = $"{s.Item.ItemName} (Available: {s.Quantity})"
                }), "StockId", "Display");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StockId,WastedQuantity,WasteReason")] WasteManagement waste)
        {
            ModelState.Remove("Stock");
            if (ModelState.IsValid)
            {                
                var stock = await _context.Stocks.FindAsync(waste.StockId);
                if (stock == null)
                {
                    ModelState.AddModelError("", "Invalid stock selected");
                    return View(waste);
                }

                if (stock.Quantity < waste.WastedQuantity)
                {
                    ModelState.AddModelError("WastedQuantity", "Wasted quantity cannot exceed available stock");
                    return View(waste);
                }

                stock.Quantity -= waste.WastedQuantity;
                waste.WasteDate = DateTime.Now;

                _context.Add(waste);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns if error
            ViewBag.Stocks = new SelectList(_context.Stocks
                .Include(s => s.Item)
                .Where(s => s.Quantity > 0)
                .Select(s => new {
                    s.StockId,
                    Display = $"{s.Item.ItemName} (Available: {s.Quantity})"
                }), "StockId", "Display");

            return View(waste);
        }

        // GET: WasteManagement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var waste = await _context.WasteManagements
                .Include(w => w.Stock)
                .ThenInclude(s => s.Item)
                .FirstOrDefaultAsync(m => m.WasteManagementId == id);

            return waste == null ? NotFound() : View(waste);
        }

        // GET: WasteManagement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var waste = await _context.WasteManagements.FindAsync(id);
            if (waste == null) return NotFound();

            await _context.Entry(waste)
                .Reference(w => w.Stock)
                .Query()
                .Include(s => s.Item)
                .LoadAsync();

            return View(waste);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WasteManagementId,StockId,WastedQuantity,WasteReason,WasteDate")] WasteManagement waste)
        {
            if (id != waste.WasteManagementId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(waste);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WasteExists(waste.WasteManagementId))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(waste);
        }

        // GET: WasteManagement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var waste = await _context.WasteManagements
                .Include(w => w.Stock)
                .ThenInclude(s => s.Item)
                .FirstOrDefaultAsync(m => m.WasteManagementId == id);

            return waste == null ? NotFound() : View(waste);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var waste = await _context.WasteManagements.FindAsync(id);
            if (waste != null)
            {
                _context.WasteManagements.Remove(waste);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool WasteExists(int id)
        {
            return _context.WasteManagements.Any(e => e.WasteManagementId == id);
        }
    }
    public class WasteReportViewModel
    {
        public string ItemName { get; set; }
        public decimal TotalWasted { get; set; }
        public DateTime LastWasteDate { get; set; }
    }
}
