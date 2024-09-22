using EmployeeMgmt.Domain.Entities;
using EmployeeMgmt.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeManagementDbContext _context;
    private readonly ILogger<EmployeeRepository> _logger;

    public EmployeeRepository(EmployeeManagementDbContext context, ILogger<EmployeeRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        try
        {
            return await _context.Employees.Include(e => e.Department).ToListAsync();
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GetAllAsync method: Employee repository due to " + ex.Message);
            throw new Exception("An unexpected error occurred. Please try again later.", ex);
        }
    }

    public async Task<Employee> GetByIdAsync(int id)
    {
        try
        {
            var employee = await _context.Employees.Include(e => e.Department)
                                                    .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found.");
            }

            return employee;
        }
        
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogWarning(knfEx, "Item not found in GetByIdAsync method: Employee repository due to " + knfEx.Message);
            throw new Exception("Requested employee not found.", knfEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GetByIdAsync method: Employee repository due to " + ex.Message);
            throw new Exception("An unexpected error occurred. Please try again later.", ex);
        }
    }

    public async Task AddAsync(Employee entity)
    {
        try
        {
            await _context.Employees.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while adding a new employee in the AddAsync method: Employee repository due to " + ex.Message);
            throw new Exception("Could not add employee. Please check the data and try again.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in AddAsync method: Employee repository due to " + ex.Message);
            throw new Exception("An unexpected error occurred. Please try again later.", ex);
        }
    }

    public async Task UpdateAsync(Employee entity)
    {
        try
        {
            _context.Employees.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "An error occurred while updating the employee in the UpdateAsync method: Employee repository due to " + ex.Message);
            throw new Exception("Could not update employee. The data might have changed, please refresh and try again.", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while updating employee in UpdateAsync method: Employee repository due to " + ex.Message);
            throw new Exception("Could not update employee. Please try again later.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in UpdateAsync method: Employee repository due to " + ex.Message);
            throw new Exception("An unexpected error occurred. Please try again later.", ex);
        }
    }

    public async Task DeleteAsync(Employee entity)
    {
        try
        {
            _context.Employees.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the employee in the DeleteAsync method: Employee repository due to " + ex.Message);
            throw new Exception("Could not delete employee. Please try again later.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in DeleteAsync method: Employee repository due to " + ex.Message);
            throw new Exception("An unexpected error occurred. Please try again later.", ex);
        }
    }
}
