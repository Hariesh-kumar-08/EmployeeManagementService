using EmployeeMgmt.Application.DTOs;
using EmployeeMgmt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeMgmt.API_.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger)
        {
            _departmentService = departmentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllDepartments()
        {
            try
            {
                _logger.LogInformation("Fetching all departments in GetAllDepartments: DepartmentController");

                var departments = await _departmentService.GetAllDepartmentsAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all departments in GetAllDepartments: DepartmentController due to " + ex.Message);
                return StatusCode(500, "An error occurred while fetching departments.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetDepartmentById(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching department with ID {id} in GetDepartmentById: DepartmentController");

                var department = await _departmentService.GetDepartmentByIdAsync(id);
                return Ok(department);
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, $"Department with ID {id} not found in GetDepartmentById: DepartmentController due to " + knfEx.Message);
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching department with ID {id} in GetDepartmentById: DepartmentController due to " + ex.Message);
                return StatusCode(500, "An error occurred while fetching the department.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddDepartment([FromBody] DepartmentDTO departmentDto)
        {
            try
            {
                _logger.LogInformation("Adding a new department in AddDepartment: DepartmentController");

                await _departmentService.AddDepartmentAsync(departmentDto);
                return Ok("Department added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new department in AddDepartment: DepartmentController due to " + ex.Message);
                return StatusCode(500, "An error occurred while adding the department.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDepartment(int id, [FromBody] DepartmentDTO departmentDto)
        {
            try
            {
                _logger.LogInformation($"Updating department with ID {id} in UpdateDepartment: DepartmentController");

                departmentDto.DepartmentId = id;
                await _departmentService.UpdateDepartmentAsync(departmentDto);
                return Ok("Department updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating department with ID {id} in UpdateDepartment: DepartmentController due to " + ex.Message);
                return StatusCode(500, "An error occurred while updating the department.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting department with ID {id} in DeleteDepartment: DepartmentController");

                await _departmentService.DeleteDepartmentAsync(id);
                return Ok("Department deleted successfully.");
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, $"Department with ID {id} not found in DeleteDepartment: DepartmentController due to " + knfEx.Message);
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting department with ID {id} in DeleteDepartment: DepartmentController due to " + ex.Message);
                return StatusCode(500, "An error occurred while deleting the department.");
            }
        }
    }

}
