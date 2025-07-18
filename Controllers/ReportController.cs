using FastReport;
using FastReport.Data;
using FastReport.Export.PdfSimple;
using FastReport.Utils;
using FastReport.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreManagement_Project.Data;

namespace StoreManagement_Project.Controllers
{
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public ReportController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }
        //============================================================================================
        // GET: Report
        public async Task<IActionResult> Index()
        {
            var purchaseOrders = await _context.PurchaseOrders
                .Include(po => po.Supplier)
                .ToListAsync();

            return View(purchaseOrders);
        }

        // GET: Report/ViewReport/5
        public IActionResult ViewReport(int id)
        {
            string reportPath = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "PurchaseOrder.frx");

            // FastReport's MsSqlDataConnection object register করুন
            RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));

            // WebReport অবজেক্ট তৈরি করুন (সরাসরি PDF না করে)
            WebReport webReport = new WebReport();
            webReport.Report.Load(reportPath);

            var po = _context.PurchaseOrders
                .Where(p => p.PurchaseOrderId == id)
                .Select(p => new
                {
                    p.PONo,
                    SupplierName = p.Supplier.SupplierName,
                    Items = p.PurchaseOrderItem.Select(i => new
                    {
                        ItemName = i.Item.ItemName,
                        i.Quantity,
                        i.Price
                    }).ToList()
                }).FirstOrDefault();

            if (po == null)
                return NotFound();

            webReport.Report.RegisterData(new[] { po }, "PurchaseOrder");
            webReport.Report.Prepare();

            // WebReport প্রোপার্টি কনফিগার করুন
            webReport.Width = "100%";
            webReport.Height = "800px";
            webReport.ShowExports = true;  // এক্সপোর্ট অপশন দেখান
            webReport.ShowPrint = true;    // প্রিন্ট বাটন দেখান
            //webReport.ToolbarStyle = WebToolbarStyle.Full; // সম্পূর্ণ টুলবার দেখান


            ViewBag.WebReport = webReport;
            return View();
        }
        //============================================================================================

        // URL: /Report/PurchaseOrderReport/5
        public IActionResult PurchaseOrderReport(int id)
        {
            RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));
            string reportPath = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "PurchaseOrder.frx");

            using (Report report = new Report())
            {
                report.Load(reportPath);

                var po = _context.PurchaseOrders
                    .Where(p => p.PurchaseOrderId == id)
                    .Select(p => new
                    {
                        p.PONo,
                        SupplierName = p.Supplier.SupplierName,
                        Items = p.PurchaseOrderItem.Select(i => new
                        {
                            ItemName = i.Item.ItemName,
                            i.Quantity,
                            i.Price
                        }).ToList()
                    }).FirstOrDefault();

                if (po == null)
                    return NotFound();

                report.RegisterData(new[] { po }, "PurchaseOrder");
                report.Prepare();

                using (MemoryStream ms = new MemoryStream())
                {
                    PDFSimpleExport pdfExport = new PDFSimpleExport();
                    report.Export(pdfExport, ms);
                    ms.Position = 0;

                    //return File(ms.ToArray(), "application/pdf", $"PurchaseOrder_{po.PONo}.pdf");
                    return RedirectToAction("Index");
                }
            }
        }

    }
}
