using EmployeeMgmt.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementService
{
    public class Employee
    {
        [Key] // Primary Key annotation
        public int EmployeeId { get; set; }

        [Required] // Employee code is required
        [MaxLength(10)] // Optional: Set max length for employee code
        public string EmployeeCode { get; set; }

        [Required] // Name is required
        [MaxLength(100)] // Optional: Set max length for name
        public string Name { get; set; }

        [Required] // Email is required
        [EmailAddress] // Ensures the email format is valid
        public string Email { get; set; }

        [Required] // HireDate is required
        public DateTime HireDate { get; set; }

        // Foreign Key for Department
        [ForeignKey("Department")] // Specifies the foreign key column
        public int DepartmentId { get; set; }

        // Navigation Property to the Department
        public Department Department { get; set; }
    }
}
