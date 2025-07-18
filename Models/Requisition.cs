using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement_Project.Models
{
    public class Requisition
    {
        public int RequisitionId { get; set; }

        [Display(Name = "Requisition Number")]
        [StringLength(20)]
        public string? RequisitionNumber { get; set; } // Auto-generated RequisitionNumber: REQ-20250506-0001

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Requisition Date")]
        public DateTime RequisitionDate { get; set; }

        [Required]
        [ForeignKey("Employee")]
        [Display(Name = "Requested By")]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public ICollection<RequisitionItem> RequisitionItems { get; set; } = new List<RequisitionItem>();
        public string? Status { get; set; } // Pending, Approved, ForwardedToPurchase, Fulfilled ✅ Suggestion: Make sure this status is updated automatically when all RequisitionItems are IsFullyIssued == true.

        [StringLength(250)]
        public string? Remarks { get; set; }
    }
}
