using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement_Project.Models
{
    public class RequisitionItem
    {
        public int RequisitionItemId { get; set; }

        [Required]
        [ForeignKey("Requisition")]
        [Display(Name = "Requisition Number")]
        public int RequisitionId { get; set; }
        public Requisition? Requisition { get; set; }

        [ForeignKey("Item")]
        [Display(Name = "Item Name")]
        public int ItemId { get; set; }
        public Item? Item { get; set; }


        [Display(Name = "UOM")]
        public int UnitId { get; set; }
        public Unit? Unit { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity Requested must be greater than 0")]
        [Display(Name = "Requested Quantity")]
        public decimal QuantityRequested { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Quantity Issued cannot be negative")]
        [Display(Name = "Issued Quantity")]
        public decimal QuantityIssued { get; set; }  // Total issued so far

        [NotMapped]
        [Display(Name = "Remaining Quantity")]
        public decimal RemainingQuantity => QuantityRequested - QuantityIssued;
        //public decimal RemainingQuantity { get; set; } // QuantityRequested - QuantityIssued

        public bool IsFullyIssued { get; set; } // ✅ new: to track if this item is fulfilled requisitionItem.IsFullyIssued = requisitionItem.QuantityRequested - requisitionItem.QuantityIssued <= 0;


        [StringLength(250)]
        public string? Remarks { get; set; }
    }
}
