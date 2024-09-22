using AutoMapper;
using EmployeeMgmt.Application.DTOs;
using EmployeeMgmt.Application.Interfaces;
using EmployeeMgmt.Domain.Entities;
using EmployeeMgmt.Domain.Interfaces;
using Microsoft.Extensions.Logging;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DepartmentService> _logger;

    public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper, ILogger<DepartmentService> logger)
    {
        _departmentRepository = departmentRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all departments in GetAllDepartmentsAsync: DepartmentService");

            var departments = await _departmentRepository.GetAllAsync();
            var departmentDTOs = _mapper.Map<IEnumerable<DepartmentDTO>>(departments);

            return departmentDTOs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching all departments in GetAllDepartmentsAsync: DepartmentService due to " + ex.Message);
            throw new Exception("Could not retrieve departments. Please try again later.", ex);
        }
    }

    public async Task<DepartmentDTO> GetDepartmentByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation($"Fetching department with ID {id} in GetDepartmentByIdAsync: DepartmentService");

            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                throw new KeyNotFoundException($"Department with ID {id} not found.");
            }

            var departmentDTO = _mapper.Map<DepartmentDTO>(department);
            return departmentDTO;
        }
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogWarning(knfEx, $"Department with ID {id} not found in GetDepartmentByIdAsync: DepartmentService due to " + knfEx.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while fetching department with ID {id} in GetDepartmentByIdAsync: DepartmentService due to " + ex.Message);
            throw new Exception("Could not retrieve department. Please try again later.", ex);
        }
    }

    public async Task AddDepartmentAsync(DepartmentDTO departmentDto)
    {
        try
        {
            _logger.LogInformation("Adding a new department in AddDepartmentAsync: DepartmentService");

            var department = _mapper.Map<Department>(departmentDto);
            await _departmentRepository.AddAsync(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding a new department in AddDepartmentAsync: DepartmentService due to " + ex.Message);
            throw new Exception("Could not add department. Please check the data and try again.", ex);
        }
    }

    public async Task UpdateDepartmentAsync(DepartmentDTO departmentDto)
    {
        try
        {
            _logger.LogInformation($"Updating department with ID {departmentDto.DepartmentId} in UpdateDepartmentAsync: DepartmentService");

            var department = _mapper.Map<Department>(departmentDto);
            await _departmentRepository.UpdateAsync(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating the department with ID {departmentDto.DepartmentId} in UpdateDepartmentAsync: DepartmentService due to " + ex.Message);
            throw new Exception("Could not update department. Please try again later.", ex);
        }
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        try
        {
            _logger.LogInformation($"Deleting department with ID {id} in DeleteDepartmentAsync: DepartmentService");

            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                throw new KeyNotFoundException($"Department with ID {id} not found.");
            }

            await _departmentRepository.DeleteAsync(department);
        }
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogWarning(knfEx, $"Department with ID {id} not found in DeleteDepartmentAsync: DepartmentService due to " + knfEx.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting department with ID {id} in DeleteDepartmentAsync: DepartmentService due to " + ex.Message);
            throw new Exception("Could not delete department. Please try again later.", ex);
        }
    }
}
