using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreManagement_Project.Data;
using StoreManagement_Project.Models;

namespace StoreManagement_Project.Controllers
{
    public class StockController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stock
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Stocks.Include(s => s.Item).Include(s => s.LocationComponent).Include(s => s.Unit).Include(s => s.Warehouse);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Stock/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks
                .Include(s => s.Item)
                .Include(s => s.LocationComponent)
                .Include(s => s.Unit)
                .Include(s => s.Warehouse)
                .FirstOrDefaultAsync(m => m.StockId == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // GET: Stock/Create
        public IActionResult Create()
        {
            // Fetch Items with UnitId
            var items = _context.Items
                .Select(i => new
                {
                    i.ItemId,
                    i.ItemName,
                    i.UnitId
                })
                .ToList();

            // Pass item list to ViewData
            ViewData["Items"] = items;

            // Pass dropdowns via ViewBag
            ViewBag.LocationComponentId = new SelectList(_context.LocationComponents, "LocationComponentId", "Location");
            ViewBag.Units = new SelectList(_context.Units, "UnitId", "NameOfUnit"); // FIXED LINE
            ViewBag.WarehouseId = new SelectList(_context.Warehouses, "WarehouseId", "Name");

            return View();
        }

        // POST: Stock/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StockId,ItemId,UnitId,Quantity,WarehouseId,LocationComponentId")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns on error
            ViewData["Items"] = _context.Items
                .Select(i => new
                {
                    i.ItemId,
                    i.ItemName,
                    i.UnitId
                })
                .ToList();

            ViewBag.LocationComponentId = new SelectList(_context.LocationComponents, "LocationComponentId", "Location", stock.LocationComponentId);
            ViewBag.Units = new SelectList(_context.Units, "UnitId", "NameOfUnit", stock.UnitId); // FIXED LINE
            ViewBag.WarehouseId = new SelectList(_context.Warehouses, "WarehouseId", "Name", stock.WarehouseId);

            return View(stock);
        }


        // GET: Stock/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemName", stock.ItemId);
            ViewData["LocationComponentId"] = new SelectList(_context.LocationComponents, "LocationComponentId", "Location", stock.LocationComponentId);
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "NameOfUnit", stock.UnitId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Name", stock.WarehouseId);
            return View(stock);
        }

        // POST: Stock/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StockId,ItemId,UnitId,Quantity,WarehouseId,LocationComponentId")] Stock stock)
        {
            if (id != stock.StockId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.StockId))
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
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemName", stock.ItemId);
            ViewData["LocationComponentId"] = new SelectList(_context.LocationComponents, "LocationComponentId", "Location", stock.LocationComponentId);
            ViewData["UnitId"] = new SelectList(_context.Units, "UnitId", "NameOfUnit", stock.UnitId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Name", stock.WarehouseId);
            return View(stock);
        }

        // GET: Stock/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks
                .Include(s => s.Item)
                .Include(s => s.LocationComponent)
                .Include(s => s.Unit)
                .Include(s => s.Warehouse)
                .FirstOrDefaultAsync(m => m.StockId == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // POST: Stock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock != null)
            {
                _context.Stocks.Remove(stock);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockExists(int id)
        {
            return _context.Stocks.Any(e => e.StockId == id);
        }
        // GET: Stock/Transfer
        public IActionResult Transfer()
        {
            ViewBag.Warehouses = new SelectList(_context.Warehouses, "WarehouseId", "Name");
            ViewBag.Items = new SelectList(_context.Items, "ItemId", "ItemName");
            ViewBag.Units = new SelectList(_context.Units, "UnitId", "NameOfUnit");

            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Transfer(int itemId, int unitId, int sourceWarehouseId, int destinationWarehouseId, decimal quantity)
        //{
        //    if (sourceWarehouseId == destinationWarehouseId)
        //    {
        //        ModelState.AddModelError("", "Source and destination warehouses must be different.");
        //    }

        //    var sourceStock = await _context.Stocks
        //        .FirstOrDefaultAsync(s => s.ItemId == itemId && s.UnitId == unitId && s.WarehouseId == sourceWarehouseId);

        //    if (sourceStock == null || sourceStock.Quantity < quantity)
        //    {
        //        ModelState.AddModelError("", "Insufficient stock in the source warehouse.");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Deduct from source
        //        sourceStock.Quantity -= quantity;

        //        // Add to destination
        //        var destStock = await _context.Stocks
        //            .FirstOrDefaultAsync(s => s.ItemId == itemId && s.UnitId == unitId && s.WarehouseId == destinationWarehouseId);

        //        if (destStock != null)
        //        {
        //            destStock.Quantity += quantity;
        //        }
        //        else
        //        {
        //            var newStock = new Stock
        //            {
        //                ItemId = itemId,
        //                UnitId = unitId,
        //                Quantity = quantity,
        //                WarehouseId = destinationWarehouseId
        //            };
        //            _context.Stocks.Add(newStock);
        //        }

        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    ViewBag.Warehouses = new SelectList(_context.Warehouses, "WarehouseId", "Name");
        //    ViewBag.Items = new SelectList(_context.Items, "ItemId", "ItemName");
        //    ViewBag.Units = new SelectList(_context.Units, "UnitId", "NameOfUnit");

        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(int itemId, int unitId, int sourceWarehouseId, int destinationWarehouseId, int? destinationLocationComponentId, decimal quantity)
        {
            if (sourceWarehouseId == destinationWarehouseId)
            {
                ModelState.AddModelError("", "Source and destination warehouses must be different.");
            }

            var sourceStock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.ItemId == itemId && s.UnitId == unitId && s.WarehouseId == sourceWarehouseId);

            if (sourceStock == null || sourceStock.Quantity < quantity)
            {
                ModelState.AddModelError("", "Insufficient stock in the source warehouse.");
            }

            if (ModelState.IsValid)
            {
                // Deduct from source
                sourceStock.Quantity -= quantity;

                // Add to destination
                var destStock = await _context.Stocks
                    .FirstOrDefaultAsync(s =>
                        s.ItemId == itemId &&
                        s.UnitId == unitId &&
                        s.WarehouseId == destinationWarehouseId &&
                        s.LocationComponentId == destinationLocationComponentId);

                if (destStock != null)
                {
                    destStock.Quantity += quantity;
                }
                else
                {
                    var newStock = new Stock
                    {
                        ItemId = itemId,
                        UnitId = unitId,
                        Quantity = quantity,
                        WarehouseId = destinationWarehouseId,
                        LocationComponentId = destinationLocationComponentId
                    };
                    _context.Stocks.Add(newStock);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Warehouses = new SelectList(_context.Warehouses, "WarehouseId", "Name");
            ViewBag.Items = new SelectList(_context.Items, "ItemId", "ItemName");
            ViewBag.Units = new SelectList(_context.Units, "UnitId", "NameOfUnit");

            return View();
        }

        [HttpGet]
        public JsonResult GetLocationComponentsByWarehouse(int warehouseId)
        {
            var locations = _context.LocationComponents
                .Where(l => l.WarehouseId == warehouseId)
                .Select(l => new
                {
                    l.LocationComponentId,
                    Location = l.Location 
                })
                .ToList();

            return Json(locations);
        }
        [HttpGet]
        public async Task<IActionResult> GetUnitsByItem(int itemId)
        {
            var unit = await _context.Items
                .Where(i => i.ItemId == itemId)
                .Select(i => new
                {
                    unitId = i.Unit.UnitId,
                    name = i.Unit.NameOfUnit
                })
                .FirstOrDefaultAsync();

            if (unit == null)
                return Json(new List<object>());

            return Json(new[] { unit }); // Wrap in array so JS can process it like a list
        }
    }
}
