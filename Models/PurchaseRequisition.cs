using System.ComponentModel.DataAnnotations;

namespace StoreManagement_Project.Models
{
    public class PurchaseRequisition
    {
        public int PurchaseRequisitionId { get; set; }

        [Required]
        [Display(Name = "Requisition Number")]
        [StringLength(50)]
        public string? PurchaseRequisitionNumber { get; set; } // Auto-generated PurchaseRequisitionNumber: PR-20250506-0001

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Requisition Date")]
        public DateTime Date { get; set; }

        public int RequisitionId { get; set; }
        public Requisition? Requisition { get; set; }

        public ICollection<PurchaseRequisitionItem>? PurchaseRequisitionItems { get; set; } = new List<PurchaseRequisitionItem>();

        [StringLength(250)]
        public string? Remarks { get; set; }
    }
}
