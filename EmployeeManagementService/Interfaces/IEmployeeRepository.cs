using EmployeeMgmt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMgmt.Domain.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        //Employee Specific methods here
    }
}
