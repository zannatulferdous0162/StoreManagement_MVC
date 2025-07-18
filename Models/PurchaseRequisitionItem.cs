using System.ComponentModel.DataAnnotations;

namespace StoreManagement_Project.Models
{
    public class PurchaseRequisitionItem
    {
        public int PurchaseRequisitionItemId { get; set; }

        [Display(Name = "Purchase Requisition Number")]
        public int PurchaseRequisitionId { get; set; }
        public PurchaseRequisition? PurchaseRequisition { get; set; }

        [Required]
        [Display(Name = "Item Name")]
        public int ItemId { get; set; }
        public Item? Item { get; set; }


        [Display(Name = "UOM")]
        public int UnitId { get; set; }
        public Unit? Unit { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        [Display(Name = "Quantity Requested")]
        public decimal Quantity { get; set; }

        [StringLength(250)]
        public string? Remarks { get; set; }

        public int RequisitionItemId { get; set; }
        public RequisitionItem? RequisitionItem { get; set; }
    }
}
