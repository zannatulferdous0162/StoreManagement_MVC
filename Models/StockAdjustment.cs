using System.ComponentModel.DataAnnotations;

namespace StoreManagement_Project.Models
{
    public class StockAdjustment
    {
        public int StockAdjustmentId { get; set; }
        [Display(Name="Item")]
        public int StockId { get; set; }
        public Stock Stock { get; set; }
        public decimal AdjustedQuantity { get; set; }
        public string AdjustmentReason { get; set; } = string.Empty;
        public DateTime AdjustmentDate { get; set; } = DateTime.Now;
        public string AdjustmentType { get; set; }
        public string Requester { get; set; } = string.Empty;      
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending;
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
    }
    public enum ApprovalStatus
    {
        Pending,
        Approved,
        Rejected
    }
}