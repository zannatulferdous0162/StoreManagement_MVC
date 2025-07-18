namespace StoreManagement_Project.Models
{
    public class GRNItem
    {
        public int GRNItemId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public Item? Item { get; set; }
        public int? LocationComponentId { get; set; } // nullable, not mandatory
        public LocationComponent? LocationComponent { get; set; } = new();
        public decimal Quantity { get; set; }
        public string NameOfUnit { get; set; } = string.Empty;
        public int? UnitId { get; set; }
        public Unit? Unit { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal RemainingQuantity { get; set; }//=> Quantity - QuantityReceived;
        public bool Inspection { get; set; } 
        public int GRNId { get; set; }
        public GRN? GRN { get; set; }
    }
}
