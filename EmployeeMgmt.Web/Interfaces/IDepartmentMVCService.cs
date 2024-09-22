using EmployeeMgmt.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IDepartmentMVCService
{
    Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync();
    Task<DepartmentDTO> GetDepartmentByIdAsync(int id);
    Task AddDepartmentAsync(DepartmentDTO departmentDto);
    Task UpdateDepartmentAsync(DepartmentDTO departmentDto);
    Task DeleteDepartmentAsync(int id);
}
