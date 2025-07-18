using System.ComponentModel.DataAnnotations;

namespace StoreManagement_Project.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; } // e.g., HR, Admin, Store, Purchase

        [StringLength(250)]
        public string? Remarks { get; set; }
    }
}
