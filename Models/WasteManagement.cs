using System.ComponentModel.DataAnnotations;

namespace StoreManagement_Project.Models
{
    public class WasteManagement
    {
        public int WasteManagementId { get; set; }
        public int StockId { get; set; }
        public Stock Stock { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "Quantity will be minimum 1")]
        public decimal WastedQuantity { get; set; }
        public string WasteReason { get; set; } = string.Empty;
        public DateTime WasteDate { get; set; } = DateTime.Now;
    }
}
