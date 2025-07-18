using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement_Project.Models
{
    public class PurchaseOrderItem
    {
        public int PurchaseOrderItemId { get; set; }
        public int ItemId { get; set; }
        public Item? Item { get; set; }
        public decimal Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 1.")]
        public decimal Price { get; set; }
        //public int UOMId { get; set; }
        [Column("UOM")]
        public int UnitId { get; set; }
        public virtual Unit? Unit { get; set; }
        //public UOM? UOM { get; set; }
        public int PurchaseOrderId { get; set; }  // Foreign Key
        public PurchaseOrder? PurchaseOrder { get; set; }
        public int? PurchaseRequisitionItemId { get; set; }  
        public PurchaseRequisitionItem? PurchaseRequisitionItem { get; set; }
    }
}
