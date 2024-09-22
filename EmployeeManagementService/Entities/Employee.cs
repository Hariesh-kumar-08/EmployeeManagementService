using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EmployeeMgmt.Domain.ValueObjects;

namespace EmployeeMgmt.Domain.Entities
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }  // Changed to int

        [Required]
        public EmployeeCode EmployeeCode { get; private set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; private set; }

        [Required]
        public EmailAddress Email { get; private set; }

        [Required]
        public DateTime HireDate { get; private set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }  // Foreign key for department

        public Department Department { get; set; }

        // Parameterless constructor for EF
        public Employee() { }


        // Constructor to initialize an Employee object
        public Employee(EmployeeCode employeeCode, string name, EmailAddress email, DateTime hireDate, int departmentId)
        {
            EmployeeCode = employeeCode;
            Name = name;
            Email = email;
            HireDate = hireDate;
            DepartmentId = departmentId;
        }

       
    }
}
