using System.ComponentModel.DataAnnotations;

namespace StoreManagement_Project.Models
{
    public class RequisitionIssue
    {
        public int RequisitionIssueId { get; set; }

        [Display(Name = "Issue Number")]
        public string? IssueNumber { get; set; } // ISSUE-REQ-20250506-0001

        [Required]
        [Display(Name = "Requisition")]
        public int RequisitionId { get; set; }
        public Requisition? Requisition { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Issue Date")]
        public DateTime IssueDate { get; set; }
        public ICollection<RequisitionIssueItem>? RequisitionIssueItems { get; set; } = new List<RequisitionIssueItem>();
    }
}
