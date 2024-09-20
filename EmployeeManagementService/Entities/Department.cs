using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMgmt.Domain.Entities
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [MaxLength(50)]
        public string DepartmentName { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        // Navigation Property to Employees (One-to-Many relationship)
        public ICollection<Employee> Employees { get; set; }
    }
}
