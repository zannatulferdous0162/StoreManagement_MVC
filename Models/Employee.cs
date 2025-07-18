using System.ComponentModel.DataAnnotations;

namespace StoreManagement_Project.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee Name is required")]
        [StringLength(100)]
        [Display(Name = "Employee Name")]
        public string? EmployeeName { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        [StringLength(50)]
        public string? Designation { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email")]
        public string? EmployeeEmail { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        [Display(Name = "Phone Number")]
        public string? EmployeePhone { get; set; }

        [StringLength(250)]
        public string? Remarks { get; set; }
    }
}
