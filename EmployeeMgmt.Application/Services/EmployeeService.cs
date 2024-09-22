using AutoMapper;
using EmployeeMgmt.Application.DTOs;
using EmployeeMgmt.Application.Interfaces;
using EmployeeMgmt.Domain.Entities;
using EmployeeMgmt.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, ILogger<EmployeeService> logger)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync()
    {
        try
        {
            var employees = await _employeeRepository.GetAllAsync();

            // Converting entities to DTOs using AutoMapper
            var employeeDTOs = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);

            _logger.LogInformation("Successfully fetched all employees in the GetAllEmployeesAsync method: EmployeeService");

            return employeeDTOs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching all employees in the GetAllEmployeesAsync method: EmployeeService due to " + ex.Message);
            throw new Exception("Could not retrieve employees. Please try again later.", ex);
        }
    }

    public async Task<EmployeeDTO> GetEmployeeByIdAsync(int id)
    {
        try
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found.");
            }

            // Converting entity to DTO using AutoMapper
            var employeeDTO = _mapper.Map<EmployeeDTO>(employee);

            _logger.LogInformation("Successfully fetched employee with ID {id} in the GetEmployeeByIdAsync method: EmployeeService", id);

            return employeeDTO;
        }
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogWarning(knfEx, "Item not found in the GetEmployeeByIdAsync method: EmployeeService due to " + knfEx.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching the employee with ID {id} in the GetEmployeeByIdAsync method: EmployeeService due to " + ex.Message);
            throw new Exception("Could not retrieve employee. Please try again later.", ex);
        }
    }

    public async Task AddEmployeeAsync(EmployeeDTO employeeDto)
    {
        try
        {
            // Converting DTO to entity using AutoMapper
            var employee = _mapper.Map<Employee>(employeeDto);

            await _employeeRepository.AddAsync(employee);

            _logger.LogInformation("Successfully added a new employee in the AddEmployeeAsync method: EmployeeService");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding a new employee in the AddEmployeeAsync method: EmployeeService due to " + ex.Message);
            throw new Exception("Could not add employee. Please check the data and try again.", ex);
        }
    }

    public async Task UpdateEmployeeAsync(EmployeeDTO employeeDto)
    {
        try
        {
            //Converting DTO to entity using Auto mapper
            var employee = _mapper.Map<Employee>(employeeDto);

            await _employeeRepository.UpdateAsync(employee);

            _logger.LogInformation("Successfully updated employee in the UpdateEmployeeAsync method: EmployeeService");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the employee in the UpdateEmployeeAsync method: EmployeeService due to " + ex.Message);
            throw new Exception("Could not update employee. The data might have changed, please refresh and try again.", ex);
        }
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        try
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found.");
            }

            await _employeeRepository.DeleteAsync(employee);

            _logger.LogInformation("Successfully deleted employee in the DeleteEmployeeAsync method: EmployeeService");
        }
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogWarning(knfEx, "Item not found in the DeleteEmployeeAsync method: EmployeeService due to " + knfEx.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the employee in the DeleteEmployeeAsync method: EmployeeService due to " + ex.Message);
            throw new Exception("Could not delete employee. Please try again later.", ex);
        }
    }
}
