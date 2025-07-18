using StoreManagement_Project.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement_Project.Models
{
    public class PurchaseOrder
    {
        public int PurchaseOrderId { get; set; }
        [Display(Name ="Supplier PO Number ID")]
        public string? PONo { get; set; }
      
        [Display(Name ="Pourchase Number")]
        public string? POManualNumber { get; set; }
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public DateTime OrderDate { get; set; }
        [Display(Name ="Warehouse Address")]
        public string? DeliveryPoint { get; set; }
        [RegularExpression(@"^[^<>]*$", ErrorMessage = "HTML tags are not allowed.")]
        [NoHtmlOrLinks]
        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; } = DateTime.Today.AddDays(4);
        public virtual List<PurchaseOrderItem>? PurchaseOrderItem { get; set; }
        public int? WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }
        public int? CurrencyId { get; set; }
        public Currency? Currency { get; set; }
    }
}
