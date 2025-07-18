using System.ComponentModel.DataAnnotations;

namespace StoreManagement_Project.Models
{
    public class RequisitionIssueItem
    {
        public int RequisitionIssueItemId { get; set; }

        [Display(Name = "Issue Number")]
        public int RequisitionIssueId { get; set; }
        public RequisitionIssue? RequisitionIssue { get; set; }

        [Required]
        [Display(Name = "Requisition Item")]
        public int RequisitionItemId { get; set; } // 👈 link to track which RequisitionItem this fulfills
        public RequisitionItem? RequisitionItem { get; set; }

        [Display(Name = "Item Name")]
        public int ItemId { get; set; }
        public Item? Item { get; set; }


        [Display(Name = "UOM")]
        public int UnitId { get; set; }
        public Unit? Unit { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity Issued must be greater than zero.")]
        [Display(Name = "Quantity Issued")]
        public decimal QuantityIssued { get; set; }

        [Display(Name = "Warehouse")]
        public int? WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }

        [Display(Name = "Location")]
        public int? LocationComponentId { get; set; }
        public LocationComponent? LocationComponent { get; set; }
    }
}
