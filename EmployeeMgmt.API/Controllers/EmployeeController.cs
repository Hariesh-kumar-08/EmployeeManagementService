using EmployeeMgmt.Application.DTOs;
using EmployeeMgmt.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeMgmt.API_.Controllers
{
    using EmployeeMgmt.Application.DTOs;
    using EmployeeMgmt.Application.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                _logger.LogInformation("Successfully fetched all employees in the GetAllEmployees method: EmployeeController");
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all employees in the GetAllEmployees method: EmployeeController due to " + ex.Message);
                return StatusCode(500, "An error occurred while retrieving the employee list. Please try again later.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                _logger.LogInformation("Successfully fetched employee with ID {id} in the GetEmployeeById method: EmployeeController", id);
                return Ok(employee);
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, "Item not found in the GetEmployeeById method: EmployeeController due to " + knfEx.Message);
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the employee with ID {id} in the GetEmployeeById method: EmployeeController due to " + ex.Message);
                return StatusCode(500, "An error occurred while retrieving the employee details. Please try again later.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddEmployee(EmployeeDTO employeeDto)
        {
            try
            {
                await _employeeService.AddEmployeeAsync(employeeDto);
                _logger.LogInformation("Successfully added a new employee in the AddEmployee method: EmployeeController");
                return Ok("Employee added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new employee in the AddEmployee method: EmployeeController due to " + ex.Message);
                return StatusCode(500, "An error occurred while adding the employee. Please check the data and try again.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEmployee(int id, EmployeeDTO employeeDto)
        {
            if (id != employeeDto.EmployeeId)
            {
                return BadRequest("Employee ID mismatch.");
            }

            try
            {
                await _employeeService.UpdateEmployeeAsync(employeeDto);
                _logger.LogInformation("Successfully updated employee in the UpdateEmployee method: EmployeeController");
                return Ok("Employee updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the employee in the UpdateEmployee method: EmployeeController due to " + ex.Message);
                return StatusCode(500, "An error occurred while updating the employee. Please try again later.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {
                await _employeeService.DeleteEmployeeAsync(id);
                _logger.LogInformation("Successfully deleted employee in the DeleteEmployee method: EmployeeController");
                return Ok("Employee deleted successfully.");
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, "Item not found in the DeleteEmployee method: EmployeeController due to " + knfEx.Message);
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the employee in the DeleteEmployee method: EmployeeController due to " + ex.Message);
                return StatusCode(500, "An error occurred while deleting the employee. Please try again later.");
            }
        }
    }

}
