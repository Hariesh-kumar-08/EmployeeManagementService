using EmployeeMgmt.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IEmployeeMVCService
{
    Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync();
    Task<EmployeeDTO> GetEmployeeByIdAsync(int id);
    Task AddEmployeeAsync(EmployeeDTO employeeDto);
    Task UpdateEmployeeAsync(EmployeeDTO employeeDto);
    Task DeleteEmployeeAsync(int id);
}
