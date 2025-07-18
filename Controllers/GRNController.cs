using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreManagement_Project.Data;
using StoreManagement_Project.Models;
using StoreManagement_Project.ViewModels;

namespace StoreManagement_Project.Controllers
{
    public class GRNController : Controller
    {
        private readonly ApplicationDbContext _context;
        private decimal RemainingQuantity;

        public GRNController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            var model = new GRNViewModel
            {
                Warehouses = _context.Warehouses.ToList(),
                ReceivedDate = DateTime.Now,
                Suppliers = _context.Suppliers.Select(s => new SelectListItem
                {
                    Value = s.SupplierId.ToString(),
                    Text = s.SupplierName
                }).ToList()
            };

            return View(model);
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(GRNViewModel model)
        //{
        //    if (model.GRNItems == null || !model.GRNItems.Any())
        //    {
        //        ModelState.AddModelError("", "At least one item must be included.");
        //    }

        //    // Pre-generate GRNNumber if PurchaseOrder is selected
        //    if (model.PurchaseOrderId > 0)
        //    {
        //        var po = await _context.PurchaseOrders
        //            .FirstOrDefaultAsync(p => p.PurchaseOrderId == model.PurchaseOrderId);

        //        if (po != null)
        //        {
        //            var count = await _context.GRNs
        //                .CountAsync(g => g.GRNNumber.StartsWith($"GRN-{po.PONo}/"));
        //            var serial = (count + 1).ToString("D2");
        //            model.GRNNumber = $"GRN-{po.PONo}/{serial}"; // 🔥 assign here
        //        }
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var grn = new GRN
        //        {
        //            GRNNumber = model.GRNNumber,
        //            SupplierId = model.SupplierId,
        //            PurchaseOrderId = model.PurchaseOrderId,
        //            WarehouseId = model.WarehouseId,
        //            ReceivedDate = model.ReceivedDate,
        //            InvoiceDate = (DateTime)model.InvoiceDate,
        //            InvoiceNo = model.InvoiceNo,
        //            ReceivedBy = model.ReceivedBy,
        //            GRNItems = new List<GRNItem>()
        //        };

        //        foreach (var item in model.GRNItems)
        //        {
        //            var totalReceived = await _context.GRNs
        //                .Where(g => g.PurchaseOrderId == model.PurchaseOrderId)
        //                .SelectMany(g => g.GRNItems)
        //                .Where(i => i.ItemId == item.ItemId)
        //                .SumAsync(i => (decimal?)i.QuantityReceived) ?? 0;

        //            var remainingQty = item.Quantity - totalReceived - item.QuantityReceived;

        //            grn.GRNItems.Add(new GRNItem
        //            {
        //                ItemId = item.ItemId,
        //                LocationComponentId = item.LocationComponentId,
        //                ItemName = item.ItemName,
        //                Quantity = item.Quantity,
        //                UnitName = item.UnitName,
        //                QuantityReceived = item.QuantityReceived,
        //                RemainingQuantity = remainingQty < 0 ? 0 : remainingQty,
        //                Inspection = item.Inspection
        //            });

        //            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.ItemId == item.ItemId);
        //            if (stock != null)
        //            {
        //                stock.Quantity += item.QuantityReceived;
        //                _context.Stocks.Update(stock);
        //            }
        //            else
        //            {
        //                _context.Stocks.Add(new Stock
        //                {
        //                    ItemId = item.ItemId,
        //                    Quantity = item.QuantityReceived,
        //                    UnitId = item.UnitId,
        //                    LocationComponentId = item.LocationComponentId
        //                });
        //            }
        //        }

        //        _context.GRNs.Add(grn);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    // Reload dropdowns
        //    model.Suppliers = _context.Suppliers.Select(s => new SelectListItem
        //    {
        //        Value = s.SupplierId.ToString(),
        //        Text = s.SupplierName
        //    }).ToList();

        //    if (model.SupplierId != 0)
        //    {
        //        model.PurchaseOrders = _context.PurchaseOrders
        //            .Where(po => po.SupplierId == model.SupplierId)
        //            .Select(po => new SelectListItem
        //            {
        //                Value = po.PurchaseOrderId.ToString(),
        //                Text = po.PONo
        //            }).ToList();
        //    }

        //    return View(model);
        //}
        //    [HttpPost]
        //    public async Task<IActionResult> Create(GRNViewModel model)
        //    {
        //        if (model.GRNItems == null || !model.GRNItems.Any())
        //        {
        //            ModelState.AddModelError("", "At least one item must be included.");
        //        }

        //        // Pre-generate GRNNumber if PurchaseOrder is selected
        //        if (model.PurchaseOrderId > 0)
        //        {
        //            var po = await _context.PurchaseOrders
        //                .FirstOrDefaultAsync(p => p.PurchaseOrderId == model.PurchaseOrderId);

        //            if (po != null)
        //            {
        //                var count = await _context.GRNs
        //                    .CountAsync(g => g.GRNNumber.StartsWith($"GRN-{po.PONo}/"));
        //                var serial = (count + 1).ToString("D2");
        //                model.GRNNumber = $"GRN-{po.PONo}/{serial}";
        //            }
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            var grn = new GRN
        //            {
        //                GRNNumber = model.GRNNumber,
        //                SupplierId = model.SupplierId,
        //                PurchaseOrderId = model.PurchaseOrderId,
        //                WarehouseId = model.WarehouseId,
        //                ReceivedDate = model.ReceivedDate,
        //                InvoiceDate = (DateTime)model.InvoiceDate,
        //                InvoiceNo = model.InvoiceNo,
        //                ReceivedBy = model.ReceivedBy,
        //                GRNItems = new List<GRNItem>()
        //            };

        //            foreach (var item in model.GRNItems)
        //            {
        //                var totalReceived = await _context.GRNs
        //                    .Where(g => g.PurchaseOrderId == model.PurchaseOrderId)
        //                    .SelectMany(g => g.GRNItems)
        //                    .Where(i => i.ItemId == item.ItemId)
        //                    .SumAsync(i => (decimal?)i.QuantityReceived) ?? 0;

        //                var remainingQty = item.Quantity - totalReceived - item.QuantityReceived;

        //                grn.GRNItems.Add(new GRNItem
        //                {
        //                    ItemId = item.ItemId,
        //                    LocationComponentId = item.LocationComponentId,
        //                    ItemName = item.ItemName,
        //                    Quantity = item.Quantity,
        //                    UnitName = item.UnitName,
        //                    QuantityReceived = item.QuantityReceived,
        //                    RemainingQuantity = remainingQty < 0 ? 0 : remainingQty,
        //                    Inspection = item.Inspection
        //                });

        //                // 🟢 Updated stock logic — track per Item + Warehouse + Location
        //                var stock = await _context.Stocks.FirstOrDefaultAsync(s =>
        //                    s.ItemId == item.ItemId &&
        //                    s.WarehouseId == model.WarehouseId &&
        //                    s.LocationComponentId == item.LocationComponentId);

        //                if (stock != null)
        //                {
        //                    stock.Quantity += item.QuantityReceived;
        //                    _context.Stocks.Update(stock);
        //                }
        //                else
        //                {
        //                    _context.Stocks.Add(new Stock
        //                    {
        //                        ItemId = item.ItemId,
        //                        Quantity = item.QuantityReceived,
        //                        UnitId = (int)item.UnitId,
        //                        WarehouseId = model.WarehouseId,
        //                        LocationComponentId = item.LocationComponentId
        //                    });
        //                }
        //            }

        //            _context.GRNs.Add(grn);
        //            var invalidUnitIds = grn.GRNItems
        //.Where(i => !_context.Units.Any(u => u.UnitId == i.UnitId))
        //.Select(i => i.UnitId)
        //.ToList();

        //            await _context.SaveChangesAsync();
        //            return RedirectToAction("Index");
        //        }

        //        // Reload dropdowns
        //        model.Suppliers = _context.Suppliers.Select(s => new SelectListItem
        //        {
        //            Value = s.SupplierId.ToString(),
        //            Text = s.SupplierName
        //        }).ToList();

        //        if (model.SupplierId != 0)
        //        {
        //            model.PurchaseOrders = _context.PurchaseOrders
        //                .Where(po => po.SupplierId == model.SupplierId)
        //                .Select(po => new SelectListItem
        //                {
        //                    Value = po.PurchaseOrderId.ToString(),
        //                    Text = po.PONo
        //                }).ToList();
        //        }

        //        return View(model);
        //    }

        //[HttpPost]
        //public async Task<IActionResult> Create(GRNViewModel model)
        //{
        //    if (model.GRNItems == null || !model.GRNItems.Any())
        //    {
        //        ModelState.AddModelError("", "At least one item must be included.");
        //    }

        //    // Auto-generate GRN Number if PurchaseOrder is selected
        //    if (model.PurchaseOrderId > 0)
        //    {
        //        var po = await _context.PurchaseOrders
        //            .FirstOrDefaultAsync(p => p.PurchaseOrderId == model.PurchaseOrderId);

        //        if (po != null)
        //        {
        //            var count = await _context.GRNs
        //                .CountAsync(g => g.GRNNumber.StartsWith($"GRN-{po.PONo}/"));
        //            var serial = (count + 1).ToString("D2");
        //            model.GRNNumber = $"GRN-{po.PONo}/{serial}";
        //        }
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var grn = new GRN
        //        {
        //            GRNNumber = model.GRNNumber,
        //            SupplierId = model.SupplierId,
        //            PurchaseOrderId = model.PurchaseOrderId,
        //            WarehouseId = model.WarehouseId,
        //            ReceivedDate = model.ReceivedDate,
        //            InvoiceDate = model.InvoiceDate ?? DateTime.Now,
        //            InvoiceNo = model.InvoiceNo,
        //            ReceivedBy = model.ReceivedBy,
        //            GRNItems = new List<GRNItem>()
        //        };

        //        foreach (var item in model.GRNItems)
        //        {
        //            if (!item.UnitId.HasValue)
        //            {
        //                ModelState.AddModelError("", $"Unit is required for item: {item.ItemName}");
        //                continue;
        //            }
        //            // Total already received for this item in this PO
        //            var totalReceived = await _context.GRNs
        //                .Where(g => g.PurchaseOrderId == model.PurchaseOrderId)
        //                .SelectMany(g => g.GRNItems)
        //                .Where(i => i.ItemId == item.ItemId)
        //                .SumAsync(i => (decimal?)i.QuantityReceived) ?? 0;

        //            var remainingQty = item.Quantity - totalReceived - item.QuantityReceived;
        //            if (remainingQty < 0) remainingQty = 0;

        //            grn.GRNItems.Add(new GRNItem
        //            {
        //                ItemId = item.ItemId,
        //                ItemName = item.ItemName,
        //                Quantity = item.Quantity,
        //                QuantityReceived = item.QuantityReceived,
        //                RemainingQuantity = remainingQty,
        //                //UnitId = (int)item.UnitId.Value,
        //                NameOfUnit = item.NameOfUnit,
        //                LocationComponentId = item.LocationComponentId,
        //                Inspection = item.Inspection
        //            });

        //            // Update stock per item, warehouse, and location
        //            var existingStock = await _context.Stocks.FirstOrDefaultAsync(s =>
        //                s.ItemId == item.ItemId &&
        //                s.WarehouseId == model.WarehouseId &&
        //                s.LocationComponentId == item.LocationComponentId);


        //            if (existingStock != null)
        //            {
        //                existingStock.Quantity += item.QuantityReceived;
        //                _context.Stocks.Update(existingStock);
        //            }
        //            else
        //            {
        //                _context.Stocks.Add(new Stock
        //                {
        //                    ItemId = item.ItemId,
        //                    Quantity = item.QuantityReceived,
        //                    //UnitId = (int)item.UnitId.Value,
        //                    WarehouseId = model.WarehouseId,
        //                    LocationComponentId = item.LocationComponentId
        //                });
        //            }
        //        }

        //        // Validate UnitIds (if necessary)
        //        var invalidUnitIds = grn.GRNItems
        //            .Where(i => !_context.Units.Any(u => u.UnitId == i.UnitId))
        //            .Select(i => i.UnitId)
        //            .ToList();


        //        _context.GRNs.Add(grn);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    // Reload dropdowns
        //    model.Suppliers = await _context.Suppliers
        //        .Select(s => new SelectListItem
        //        {
        //            Value = s.SupplierId.ToString(),
        //            Text = s.SupplierName
        //        }).ToListAsync();

        //    if (model.SupplierId != 0)
        //    {
        //        model.PurchaseOrders = await _context.PurchaseOrders
        //            .Where(po => po.SupplierId == model.SupplierId)
        //            .Select(po => new SelectListItem
        //            {
        //                Value = po.PurchaseOrderId.ToString(),
        //                Text = po.PONo
        //            }).ToListAsync();
        //    }

        //    return View(model);
        //}


        [HttpPost]
        public async Task<IActionResult> Create(GRNViewModel model)
        {
            if (model.GRNItems == null || !model.GRNItems.Any())
            {
                ModelState.AddModelError("", "At least one item must be included.");
            }

            // Auto-generate GRN Number if PurchaseOrder is selected
            if (model.PurchaseOrderId > 0)
            {
                var po = await _context.PurchaseOrders
                    .FirstOrDefaultAsync(p => p.PurchaseOrderId == model.PurchaseOrderId);

                if (po != null)
                {
                    var count = await _context.GRNs
                        .CountAsync(g => g.GRNNumber.StartsWith($"GRN-{po.PONo}/"));
                    var serial = (count + 1).ToString("D2");
                    model.GRNNumber = $"GRN-{po.PONo}/{serial}";
                }
            }

            if (ModelState.IsValid)
            {
                var grn = new GRN
                {
                    GRNNumber = model.GRNNumber,
                    SupplierId = model.SupplierId,
                    PurchaseOrderId = model.PurchaseOrderId,
                    WarehouseId = model.WarehouseId,
                    ReceivedDate = model.ReceivedDate,
                    InvoiceDate = model.InvoiceDate ?? DateTime.Now,
                    InvoiceNo = model.InvoiceNo,
                    ReceivedBy = model.ReceivedBy,
                    GRNItems = new List<GRNItem>()
                };

                foreach (var item in model.GRNItems)
                {
                    var itemEntity = await _context.Items
                        .FirstOrDefaultAsync(i => i.ItemId == item.ItemId);

                    if (itemEntity == null)
                    {
                        ModelState.AddModelError("", $"Invalid item: {item.ItemName}");
                        continue;
                    }

                    // Total already received for this item in this PO
                    var totalReceived = await _context.GRNs
                        .Where(g => g.PurchaseOrderId == model.PurchaseOrderId)
                        .SelectMany(g => g.GRNItems)
                        .Where(i => i.ItemId == item.ItemId)
                        .SumAsync(i => (decimal?)i.QuantityReceived) ?? 0;

                    var remainingQty = item.Quantity - totalReceived - item.QuantityReceived;
                    if (remainingQty < 0) remainingQty = 0;

                    grn.GRNItems.Add(new GRNItem
                    {
                        ItemId = item.ItemId,
                        ItemName = item.ItemName,
                        Quantity = item.Quantity,
                        QuantityReceived = item.QuantityReceived,
                        RemainingQuantity = remainingQty,
                        NameOfUnit = item.NameOfUnit,
                        LocationComponentId = item.LocationComponentId,
                        Inspection = item.Inspection
                        // No need to set UnitId here since it's not used in GRNItem
                    });

                    // Update stock per item, warehouse, and location
                    var existingStock = await _context.Stocks.FirstOrDefaultAsync(s =>
                        s.ItemId == item.ItemId &&
                        //s.WarehouseId == model.WarehouseId &&
                        s.LocationComponentId == item.LocationComponentId);

                    // Get WarehouseId from LocationComponent
                    var location = await _context.LocationComponents
                        .FirstOrDefaultAsync(l => l.LocationComponentId == item.LocationComponentId);

                    var warehouseIdFromLocation = location?.WarehouseId ?? model.WarehouseId;

                    if (existingStock != null)
                    {
                        existingStock.Quantity += item.QuantityReceived;
                        existingStock.UnitId = itemEntity.UnitId; // Keep unit in sync
                        existingStock.WarehouseId = warehouseIdFromLocation;
                        _context.Stocks.Update(existingStock);
                    }
                    else
                    {
                        _context.Stocks.Add(new Stock
                        {
                            ItemId = item.ItemId,
                            Quantity = item.QuantityReceived,
                            UnitId = itemEntity.UnitId, // ✅ Get UnitId from Item
                            //WarehouseId = model.WarehouseId,
                            WarehouseId = warehouseIdFromLocation,
                            LocationComponentId = item.LocationComponentId
                        });
                    }
                }

                _context.GRNs.Add(grn);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // Reload dropdowns
            model.Suppliers = await _context.Suppliers
                .Select(s => new SelectListItem
                {
                    Value = s.SupplierId.ToString(),
                    Text = s.SupplierName
                }).ToListAsync();

            if (model.SupplierId != 0)
            {
                model.PurchaseOrders = await _context.PurchaseOrders
                    .Where(po => po.SupplierId == model.SupplierId)
                    .Select(po => new SelectListItem
                    {
                        Value = po.PurchaseOrderId.ToString(),
                        Text = po.PONo
                    }).ToListAsync();
            }

            return View(model);
        }


        // AJAX: Get Purchase Orders by Supplier
        public IActionResult GetPurchaseOrdersBySupplier(int supplierId)
        {
            var pos = _context.PurchaseOrders
                .Where(po => po.SupplierId == supplierId)
                .Select(po => new { po.PurchaseOrderId, po.PONo })
                .ToList();

            return Json(pos);
        }

        // AJAX: Get PO Items + RemainingQuantity
        public IActionResult GetPOItems(int purchaseOrderId)
        {
            var items = _context.PurchaseOrderItems
                .Include(poi => poi.Item)
                .Include(poi => poi.Unit)
                .Where(poi => poi.PurchaseOrderId == purchaseOrderId)
                .ToList()
                .Select(poi =>
                {
                    var totalReceived = _context.GRNs
                        .Where(g => g.PurchaseOrderId == purchaseOrderId)
                        .SelectMany(g => g.GRNItems)
                        .Where(i => i.ItemId == poi.ItemId)
                        .Sum(i => (decimal?)i.QuantityReceived) ?? 0;

                    var remainingQuantity = poi.Quantity - totalReceived;

                    return new
                    {
                        poi.ItemId,
                        ItemName = poi.Item.ItemName,
                        poi.Quantity,
                        UnitName = poi.Unit.NameOfUnit,
                        QuantityReceived = totalReceived,
                        RemainingQuantity = remainingQuantity
                    };
                })
                .Where(item => item.RemainingQuantity > 0) // Filter here
                .ToList();

            return Json(items);
        }
        // When PO is selected, get associated Warehouse
        [HttpGet]
        public IActionResult GetWarehouseByPO(int purchaseOrderId)
        {
            var warehouse = _context.PurchaseOrders
                .Where(p => p.PurchaseOrderId == purchaseOrderId)
                .Select(p => new {
                    p.WarehouseId,
                    WarehouseName = p.Warehouse.Name
                })
                .FirstOrDefault();

            return Json(warehouse);
        }

        // When Warehouse is selected, get LocationComponents under it
        [HttpGet]
        public IActionResult GetLocationComponentsByWarehouse(int warehouseId)
        {
            var locations = _context.LocationComponents
                .Where(l => l.WarehouseId == warehouseId) // top-level
                .Select(l => new {
                    l.LocationComponentId,
                    l.Location
                }).ToList();

            return Json(locations);
        }


        public async Task<IActionResult> Index()
        {
            var grns = await _context.GRNs
                .Include(g => g.Supplier)
                .Include(g => g.PurchaseOrder)
                .Include(g => g.Warehouse)
                .OrderByDescending(g => g.ReceivedDate)
                .ToListAsync();

            return View(grns);
        }
        [HttpGet]
        public async Task<IActionResult> GenerateGRNNumber(int purchaseOrderId)
        {
            var po = await _context.PurchaseOrders.FirstOrDefaultAsync(p => p.PurchaseOrderId == purchaseOrderId);
            if (po == null)
                return Json("");

            var count = await _context.GRNs
                .CountAsync(g => g.GRNNumber.StartsWith($"GRN-{po.PONo}/"));
            var serial = (count + 1).ToString("D2");
            var grnNumber = $"GRN-{po.PONo}/{serial}";

            return Json(grnNumber);
        }

        public decimal GetRemainingQuantity()
        {
            return RemainingQuantity;
        }

        // GET: GRN/Details/5
        //public async Task<IActionResult> Details(int id, decimal remainingQuantity)
        //{
        //    // Load the GRN and related data
        //    var grn = await _context.GRNs
        //        .Include(g => g.Supplier)
        //        .Include(g => g.PurchaseOrder)
        //            .ThenInclude(po => po.PurchaseOrderItem) // Needed for ordered quantity
        //        .Include(g => g.Warehouse)
        //        .Include(g => g.GRNItems)
        //            .ThenInclude(gi => gi.Item)
        //        .Include(g => g.GRNItems)
        //            .ThenInclude(gi => gi.LocationComponent)
        //        .FirstOrDefaultAsync(g => g.GRNId == id);

        //    if (grn == null)
        //        return NotFound();

        //    // Load all GRNs under the same PO (excluding current GRN)
        //    var otherGRNs = await _context.GRNs
        //        .Where(g => g.PurchaseOrderId == grn.PurchaseOrderId && g.GRNId != grn.GRNId)
        //        .Include(g => g.GRNItems)
        //        .ToListAsync();

        //    // Calculate total received per item excluding current GRN
        //    var totalReceivedPerItem = otherGRNs
        //        .SelectMany(g => g.GRNItems)
        //        .GroupBy(i => i.ItemId)
        //        .ToDictionary(
        //            g => g.Key,
        //            g => g.Sum(i => i.QuantityReceived)
        //        );

        //    // Build the ViewModel
        //    var viewModel = new GRNViewModel
        //    {
        //        GRNId = grn.GRNId,
        //        GRNNumber = grn.GRNNumber,
        //        ReceivedDate = grn.ReceivedDate,
        //        InvoiceDate = grn.InvoiceDate,
        //        InvoiceNo = grn.InvoiceNo,
        //        ReceivedBy = grn.ReceivedBy,
        //        Supplier = grn.Supplier,
        //        PurchaseOrder = grn.PurchaseOrder,
        //        Warehouse = grn.Warehouse,
        //        GRNItems = grn.GRNItems.Select(item =>
        //        {
        //            var orderedQty = grn.PurchaseOrder.PurchaseOrderItem
        //                .FirstOrDefault(poi => poi.ItemId == item.ItemId)?.Quantity ?? 0;

        //            var totalReceived = totalReceivedPerItem.ContainsKey(item.ItemId)
        //                ? totalReceivedPerItem[item.ItemId]
        //                : 0;

        //            var remainingQty = orderedQty - totalReceived - item.QuantityReceived;

        //            return new GRNItemViewModel
        //            {
        //                ItemId = item.ItemId,
        //                ItemName = item.Item?.ItemName,
        //                Quantity = item.Quantity,
        //                QuantityReceived = item.QuantityReceived,
        //                remainingQuantity = item.Quantity - item.QuantityReceived,
        //                NameOfUnit = item.NameOfUnit,
        //                Inspection = item.Inspection,
        //                LocationComponentId = item.LocationComponentId,
        //                Location = item.LocationComponent?.Location
        //            };
        //        }).ToList()
        //    };

        //    return View(viewModel);
        //}
        //public IActionResult Details(int id)
        //{
        //    var grn = _context.GRNs
        //        .Include(g => g.Supplier)
        //        .Include(g => g.PurchaseOrder)
        //        .Include(g => g.Warehouse)
        //        .Include(g => g.GRNItems) // Correct name based on your model
        //            .ThenInclude(i => i.Item)
        //        .Include(g => g.GRNItems)
        //            .ThenInclude(i => i.Unit)
        //        .Include(g => g.GRNItems)
        //            .ThenInclude(i => i.LocationComponent)
        //        .FirstOrDefault(g => g.GRNId == id); // Corrected property name

        //    if (grn == null)
        //        return NotFound();

        //    var viewModel = new GRNViewModel
        //    {
        //        GRNId = grn.GRNId,
        //        GRNNumber = grn.GRNNumber,
        //        SupplierId = grn.SupplierId, // keep int ID
        //        Supplier = grn.Supplier,     // assign full navigation property
        //        PurchaseOrderId = grn.PurchaseOrderId, // keep int ID
        //        PurchaseOrder = grn.PurchaseOrder,     // assign full navigation property
        //        InvoiceNo = grn.InvoiceNo,
        //        InvoiceDate = grn.InvoiceDate,
        //        ReceivedDate = grn.ReceivedDate,
        //        ReceivedBy = grn.ReceivedBy,
        //        Warehouse = grn.Warehouse,   // for full info
        //        Name = grn.Warehouse?.Name,
        //        GRNItems = grn.GRNItems.Select(i => new GRNItemViewModel
        //        {
        //            ItemId = i.ItemId,
        //            ItemName = i.Item?.ItemName ?? string.Empty,
        //            LocationComponentId = i.LocationComponentId,
        //            Location = i.LocationComponent?.Location ?? "", // assuming FullPath is computed
        //            Quantity = i.Quantity,
        //            NameOfUnit = i.Unit?.NameOfUnit ?? string.Empty,
        //            UnitId = i.UnitId,
        //            QuantityReceived = i.QuantityReceived,
        //            Inspection = i.Inspection
        //        }).ToList()
        //    };

        //    return View(viewModel);
        //}
        public IActionResult Details(int id)
        {
            var grn = _context.GRNs
                .Include(g => g.Supplier)
                .Include(g => g.PurchaseOrder)
                    .ThenInclude(po => po.Warehouse)
                .Include(g => g.GRNItems)
                    .ThenInclude(i => i.Item)
                .FirstOrDefault(g => g.GRNId == id);

            if (grn == null)
                return NotFound();

            var poItems = _context.PurchaseOrderItems
                .Where(p => p.PurchaseOrderId == grn.PurchaseOrderId)
                .ToList();

            var stocks = _context.Stocks
                .Include(ws => ws.Unit)
                .Include(ws => ws.LocationComponent)
                .Include(ws => ws.Warehouse)
                .Where(ws => grn.GRNItems.Select(i => i.ItemId).Contains(ws.ItemId))
                .ToList();

            var viewModel = new GRNViewModel
            {
                GRNId = grn.GRNId,
                GRNNumber = grn.GRNNumber,
                SupplierId = grn.SupplierId,
                Supplier = grn.Supplier,
                PurchaseOrderId = grn.PurchaseOrderId,
                PurchaseOrder = grn.PurchaseOrder,
                InvoiceNo = grn.InvoiceNo,
                InvoiceDate = grn.InvoiceDate,
                ReceivedDate = grn.ReceivedDate,
                ReceivedBy = grn.ReceivedBy,
                Warehouse = grn.PurchaseOrder?.Warehouse,
                Name = grn.PurchaseOrder?.Warehouse?.Name,
                GRNItems = grn.GRNItems.Select(i =>
                {
                    var poItem = poItems.FirstOrDefault(p => p.ItemId == i.ItemId);
                    var stock = stocks.FirstOrDefault(ws => ws.ItemId == i.ItemId);

                    return new GRNItemViewModel
                    {
                        ItemId = i.ItemId,
                        ItemName = i.Item?.ItemName ?? string.Empty,
                        Quantity = poItem?.Quantity ?? 0, // ordered qty
                        QuantityReceived = i.QuantityReceived,
                        Inspection = i.Inspection,

                        // Unit, Location, Warehouse from Stock
                        UnitId = stock?.UnitId ?? 0,
                        NameOfUnit = stock?.Unit?.NameOfUnit ?? string.Empty,
                        LocationComponentId = stock?.LocationComponentId,
                        Location = stock?.LocationComponent?.Location ?? "",                      
                        remainingQuantity = stock?.Quantity ?? 0
                    };
                }).ToList()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var grn = await _context.GRNs
                .Include(g => g.Supplier)
                .Include(g => g.PurchaseOrder)
                .FirstOrDefaultAsync(g => g.GRNId == id);

            if (grn == null)
                return NotFound();

            return View(grn);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grn = await _context.GRNs
                .Include(g => g.GRNItems)
                .FirstOrDefaultAsync(g => g.GRNId == id);

            if (grn == null)
                return NotFound();

            foreach (var item in grn.GRNItems)
            {
                var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.ItemId == item.ItemId);
                if (stock != null)
                {
                    stock.Quantity -= item.QuantityReceived;

                    // ❗ Just in case, prevent negative stock
                    if (stock.Quantity < 0)
                        stock.Quantity = 0;

                    _context.Stocks.Update(stock);
                }
            }
            // Remove related GRNItems first
            if (grn.GRNItems != null && grn.GRNItems.Any())
            {
                _context.GRNItems.RemoveRange(grn.GRNItems);
            }

            _context.GRNs.Remove(grn);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var grn = await _context.GRNs
                .Include(g => g.GRNItems)
                .FirstOrDefaultAsync(g => g.GRNId == id);

            if (grn == null) return NotFound();

            var model = new GRNViewModel
            {
                GRNId = grn.GRNId,
                GRNNumber = grn.GRNNumber,
                SupplierId = grn.SupplierId,
                PurchaseOrderId = grn.PurchaseOrderId,
                InvoiceDate = grn.InvoiceDate,
                InvoiceNo = grn.InvoiceNo,
                ReceivedDate = grn.ReceivedDate,
                ReceivedBy = grn.ReceivedBy,
                GRNItems = grn.GRNItems.Select(i => new GRNItemViewModel
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    Quantity = i.Quantity,
                    NameOfUnit = i.NameOfUnit,
                    QuantityReceived = i.QuantityReceived,
                    //RemainingQuantity = i.RemainingQuantity
                }).ToList(),
                Suppliers = _context.Suppliers.Select(s => new SelectListItem
                {
                    Value = s.SupplierId.ToString(),
                    Text = s.SupplierName
                }).ToList(),
                PurchaseOrders = _context.PurchaseOrders
                    .Where(po => po.SupplierId == grn.SupplierId)
                    .Select(po => new SelectListItem
                    {
                        Value = po.PurchaseOrderId.ToString(),
                        Text = po.PONo
                    }).ToList()
            };

            return View(model);
        }
        //[HttpPost("Edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, GRNViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Suppliers = _context.Suppliers.Select(s => new SelectListItem
                {
                    Value = s.SupplierId.ToString(),
                    Text = s.SupplierName
                }).ToList();

                model.PurchaseOrders = _context.PurchaseOrders
                    .Where(po => po.SupplierId == model.SupplierId)
                    .Select(po => new SelectListItem
                    {
                        Value = po.PurchaseOrderId.ToString(),
                        Text = po.PONo
                    }).ToList();

                return View(model);
            }

            var grn = await _context.GRNs
                .Include(g => g.GRNItems)
                .FirstOrDefaultAsync(g => g.GRNId == id);

            if (grn == null) return NotFound();

            // Rollback previous stock
            foreach (var oldItem in grn.GRNItems)
            {
                var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.ItemId == oldItem.ItemId);
                if (stock != null)
                {
                    stock.Quantity -= oldItem.QuantityReceived;
                    if (stock.Quantity < 0)
                        stock.Quantity = 0;

                    _context.Stocks.Update(stock);
                }
            }

            // Remove old GRN items
            _context.GRNItems.RemoveRange(grn.GRNItems);

            // Update GRN header
            grn.SupplierId = model.SupplierId;
            grn.PurchaseOrderId = model.PurchaseOrderId;
            grn.ReceivedDate = model.ReceivedDate;
            grn.InvoiceDate = (DateTime)model.InvoiceDate;
            grn.InvoiceNo = model.InvoiceNo;
            grn.ReceivedBy = model.ReceivedBy;

            grn.GRNItems = new List<GRNItem>();

            foreach (var item in model.GRNItems)
            {
                var totalReceived = await _context.GRNs
                    .Where(g => g.PurchaseOrderId == model.PurchaseOrderId && g.GRNId != id)
                    .SelectMany(g => g.GRNItems)
                    .Where(i => i.ItemId == item.ItemId)
                    .SumAsync(i => (decimal?)i.QuantityReceived) ?? 0;

                var remainingQty = item.Quantity - totalReceived - item.QuantityReceived;

                grn.GRNItems.Add(new GRNItem
                {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    Quantity = item.Quantity,
                    NameOfUnit = item.NameOfUnit,
                    QuantityReceived = item.QuantityReceived,
                    RemainingQuantity = remainingQty < 0 ? 0 : remainingQty,
                    Inspection = item.Inspection,
                    LocationComponentId = item.LocationComponentId
                });

                var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.ItemId == item.ItemId);
                if (stock != null)
                {
                    stock.Quantity += item.QuantityReceived;
                    _context.Stocks.Update(stock);
                }
                else
                {
                    _context.Stocks.Add(new Stock
                    {
                        ItemId = item.ItemId,
                        Quantity = item.QuantityReceived,
                        UnitId = (int)item.UnitId,
                        LocationComponentId = item.LocationComponentId
                    });
                }
            }

            _context.GRNs.Update(grn);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
