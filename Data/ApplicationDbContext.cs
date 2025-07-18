using Microsoft.EntityFrameworkCore;
using StoreManagement_Project.Models;

namespace StoreManagement_Project.Data
{
    public class ApplicationDbContext : DbContext
    {
        internal object Shelves;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Brand> Brands { get; set; }     
        public DbSet<UnitSet> UnitSets { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<PackSize> PackSizes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<GRN> GRNs { get; set; }
        public DbSet<GRNItem> GRNItems { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<LocationComponent> LocationComponents { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Aisle> Aisles { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Rack> Racks { get; set; }
        public DbSet<Shelf> shelves { get; set; }
        public DbSet<Bin> Bins { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Requisition> Requisitions { get; set; }
        public DbSet<RequisitionItem> RequisitionItems { get; set; }
        public DbSet<PurchaseRequisition> PurchaseRequisitions { get; set; }
        public DbSet<PurchaseRequisitionItem> PurchaseRequisitionItems { get; set; }
        public DbSet<RequisitionIssue> RequisitionIssues { get; set; }
        public DbSet<RequisitionIssueItem> RequisitionIssueItems { get; set; }
        public DbSet<StockAdjustment> StockAdjustments { get; set; }
        public DbSet<WasteManagement> WasteManagements { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StockAdjustment>()
                .Property(e => e.ApprovalStatus)
                .HasConversion<string>();

            // Unique index on PONumber
            modelBuilder.Entity<PurchaseOrder>()
                .HasIndex(p => p.PONo)
                .IsUnique();
            modelBuilder.Entity<GRN>()
                .HasIndex(g => g.GRNNumber)
                .IsUnique();

            modelBuilder.Entity<LocationComponent>()
        .HasOne(lc => lc.Warehouse)
        .WithMany(w => w.LocationComponents)
        .HasForeignKey(lc => lc.WarehouseId)
        .OnDelete(DeleteBehavior.Cascade); // 👈 This enables cascade delete

            // Optionally, disable cascade delete for Aisle/Zone/Rack/Shelf/Bin to avoid conflicts
            modelBuilder.Entity<LocationComponent>()
                .HasOne(lc => lc.Aisle)
                .WithMany()
                .HasForeignKey(lc => lc.AisleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LocationComponent>()
                .HasOne(lc => lc.Zone)
                .WithMany()
                .HasForeignKey(lc => lc.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LocationComponent>()
                .HasOne(lc => lc.Rack)
                .WithMany()
                .HasForeignKey(lc => lc.RackId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LocationComponent>()
                .HasOne(lc => lc.Shelf)
                .WithMany()
                .HasForeignKey(lc => lc.ShelfId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LocationComponent>()
                .HasOne(lc => lc.Bin)
                .WithMany()
                .HasForeignKey(lc => lc.BinId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GRNItem>()
                .HasOne(g => g.LocationComponent)
                .WithMany()
                .HasForeignKey(g => g.LocationComponentId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
