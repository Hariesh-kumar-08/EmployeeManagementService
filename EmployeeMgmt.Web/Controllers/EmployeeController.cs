using EmployeeMgmt.Web.Services;
using EmployeeMgmt.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmployeeMgmt.Application.Interfaces;

public class EmployeeController : Controller
{
    private readonly IEmployeeMVCService _employeeService;
    private readonly IDepartmentMVCService _departmentMVCService;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IEmployeeMVCService employeeService, ILogger<EmployeeController> logger, IDepartmentMVCService departmentMVCService)
    {
        _employeeService = employeeService;
        _logger = logger;
        _departmentMVCService = departmentMVCService;
    }

    // GET: Employee
    public async Task<IActionResult> Index()
     {
        try
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            _logger.LogInformation("Index: EmployeeController - Retrieved all employees successfully.");
            return View(employees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Index: EmployeeController - Error while retrieving employees.");
            return View("Error");
        }
    }

    // GET: Employee/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                _logger.LogWarning("Details: EmployeeController - Employee with ID {id} not found.", id);
                return NotFound();
            }
            _logger.LogInformation("Details: EmployeeController - Retrieved details for employee with ID {id}.", id);
            return View(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Details: EmployeeController - Error while retrieving employee details.");
            return View("Error");
        }
    }

    // GET: Employee/Create
    public async Task<IActionResult> Create()
    {
        var departments = await _departmentMVCService.GetAllDepartmentsAsync();
        ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");
        return View();
    }

    // POST: Employee/Create
    [HttpPost]
    public async Task<IActionResult> Create(EmployeeDTO employeeDto)
    {
        
        if (ModelState.IsValid)
        {
            try
            {
                await _employeeService.AddEmployeeAsync(employeeDto);
                _logger.LogInformation("Create: EmployeeController - Successfully added new employee.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create: EmployeeController - Error while adding employee.");
                return View("Error");
            }
        }
        // If model state is invalid, repopulate the department list
        var departments = await _departmentMVCService.GetAllDepartmentsAsync();
        ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");
        return View(employeeDto);
    }



    // GET: Employee/Edit/{id}
    public async Task<IActionResult> Edit(int id)
    {
        var employeeDto = await _employeeService.GetEmployeeByIdAsync(id);
        if (employeeDto == null)
        {
            return NotFound();
        }

        // Populate departments for the dropdown
        var departments = await _departmentMVCService.GetAllDepartmentsAsync();
        ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");

        return View(employeeDto);
    }

    // POST: Employee/Edit/{id}
    [HttpPost]
    public async Task<IActionResult> Edit(EmployeeDTO employeeDto)
    {
        // Remove the Department navigation property from the model state
        ModelState.Remove("Department");

        if (ModelState.IsValid)
        {
            try
            {
                await _employeeService.UpdateEmployeeAsync(employeeDto);
                _logger.LogInformation("Edit: EmployeeController - Successfully updated employee.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Edit: EmployeeController - Error while updating employee.");
                return View("Error");
            }
        }

        // If model state is invalid, repopulate the department list
        var departments = await _departmentMVCService.GetAllDepartmentsAsync();
        ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");
        return View(employeeDto);
    }


    // GET: Employee/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                _logger.LogWarning("Delete: EmployeeController - Employee with ID {id} not found.", id);
                return NotFound();
            }
            return View(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Delete: EmployeeController - Error while retrieving employee for delete.");
            return View("Error");
        }
    }

    // POST: Employee/Delete/5
    [HttpPost, ActionName("Delete")]
    
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _employeeService.DeleteEmployeeAsync(id);
            _logger.LogInformation("DeleteConfirmed: EmployeeController - Successfully deleted employee with ID {id}.", id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeleteConfirmed: EmployeeController - Error while deleting employee with ID {id}.", id);
            return View("Error");
        }
    }
}
