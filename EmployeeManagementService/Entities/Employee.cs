using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMgmt.Domain.Entities
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(10)]
        public string EmployeeCode { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        // Foreign Key for Department
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

        // Navigation Property to the Department
        public Department Department { get; set; }
    }
}
