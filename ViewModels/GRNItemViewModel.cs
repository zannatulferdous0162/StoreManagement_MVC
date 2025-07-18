using StoreManagement_Project.Models;

namespace StoreManagement_Project.ViewModels
{
    public class GRNItemViewModel
    {
        internal decimal remainingQuantity;

        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int? LocationComponentId { get; set; }
        public string? Location { get; set; }
        public List<LocationComponent>? LocationComponents { get; set; }
        public decimal Quantity { get; set; }
        public int? UnitId { get; set; }
        public Unit? Unit { get; set; }
        public string NameOfUnit { get; set; } = string.Empty;
        public decimal QuantityReceived { get; set; }
        public decimal RemainingQuantity => Quantity - QuantityReceived;
        public bool Inspection { get; set; }
        public List<Item> Items { get; set; } = new();

    }
}
