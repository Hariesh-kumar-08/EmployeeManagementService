using EmployeeMgmt.Application.DTOs;
using EmployeeMgmt.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public class DepartmentController : Controller
{
    private readonly IDepartmentMVCService _departmentService;
    private readonly ILogger<DepartmentController> _logger;

    public DepartmentController(IDepartmentMVCService departmentService, ILogger<DepartmentController> logger)
    {
        _departmentService = departmentService;
        _logger = logger;
    }

    // GET: Department/Index
    public async Task<IActionResult> Index()
    {
        try
        {
            _logger.LogInformation("Index: DepartmentController - Fetching all departments");
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return View(departments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Index: DepartmentController - Error fetching departments");
            return View("Error");
        }
    }

    // GET: Department/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            _logger.LogInformation("Details: DepartmentController - Fetching department with ID {Id}", id);

            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                _logger.LogWarning("Details: DepartmentController - Department with ID {Id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Details: DepartmentController - Successfully fetched department with ID {Id}", id);
            return View(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Details: DepartmentController - Error fetching department with ID {Id} due to {Message}", id, ex.Message);
            return View("Error");
        }
    }

    // GET: Department/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Department/Create
    [HttpPost]
    public async Task<IActionResult> Create(DepartmentDTO departmentDto)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _logger.LogInformation("Create: DepartmentController - Adding new department");
                await _departmentService.AddDepartmentAsync(departmentDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create: DepartmentController - Error adding department");
                return View("Error");
            }
        }
        return View(departmentDto);
    }

    // GET: Department/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            _logger.LogInformation("Edit: DepartmentController - Fetching department with ID {Id}", id);
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                _logger.LogWarning("Edit: DepartmentController - Department not found with ID {Id}", id);
                return NotFound();
            }
            return View(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Edit: DepartmentController - Error fetching department");
            return View("Error");
        }
    }

    // POST: Department/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(DepartmentDTO departmentDto)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _logger.LogInformation("Edit: DepartmentController - Updating department with ID {Id}", departmentDto.DepartmentId);
                await _departmentService.UpdateDepartmentAsync(departmentDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Edit: DepartmentController - Error updating department");
                return View("Error");
            }
        }
        return View(departmentDto);
    }

    // GET: Department/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation("Delete: DepartmentController - Fetching department with ID {Id}", id);
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                _logger.LogWarning("Delete: DepartmentController - Department not found with ID {Id}", id);
                return NotFound();
            }
            return View(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Delete: DepartmentController - Error fetching department");
            return View("Error");
        }
    }

    // POST: Department/Delete/5
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            _logger.LogInformation("DeleteConfirmed: DepartmentController - Deleting department with ID {Id}", id);
            await _departmentService.DeleteDepartmentAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeleteConfirmed: DepartmentController - Error deleting department");
            return View("Error");
        }
    }
}
