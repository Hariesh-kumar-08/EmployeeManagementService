using EmployeeMgmt.Domain.Entities;
using EmployeeMgmt.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly EmployeeManagementDbContext _context;
    private readonly ILogger<DepartmentRepository> _logger;

    public DepartmentRepository(EmployeeManagementDbContext context, ILogger<DepartmentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        try
        {
            //Getting the employees
            return await _context.Departments.ToListAsync();
        }
       
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GetAllAsync method: Department repository due to " + ex.Message);
            throw new Exception("An unexpected error occurred. Please try again later.", ex);
        }
    }

    public async Task<Department> GetByIdAsync(int id)
    {
        try
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (department == null)
            {
                throw new KeyNotFoundException($"Department with ID {id} not found.");
            }

            return department;
        }
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogWarning(knfEx, "Item not found in GetByIdAsync method: Department repository due to " + knfEx.Message);
            throw new Exception("Requested department not found.", knfEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GetByIdAsync method: Department repository due to " + ex.Message);
            throw new Exception("An unexpected error occurred. Please try again later.", ex);
        }
    }

    public async Task AddAsync(Department entity)
    {
        try
        {
            await _context.Departments.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while adding a new department in the AddAsync method: Department repository due to " + ex.Message);
            throw new Exception("Could not add department. Please check the data and try again.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in AddAsync method: Department repository due to " + ex.Message);
            throw new Exception("An unexpected error occurred. Please try again later.", ex);
        }
    }

    public async Task UpdateAsync(Department entity)
    {
        try
        {
            _context.Departments.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "An error occurred while updating the department in the UpdateAsync method: Department repository due to " + ex.Message);
            throw new Exception("Could not update department. The data might have changed, please refresh and try again.", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while updating department in UpdateAsync method: Department repository due to " + ex.Message);
            throw new Exception("Could not update department. Please try again later.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in UpdateAsync method: Department repository due to " + ex.Message);
            throw new Exception("An unexpected error occurred. Please try again later.", ex);
        }
    }

    public async Task DeleteAsync(Department entity)
    {
        try
        {
            _context.Departments.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the department in the DeleteAsync method: Department repository due to " + ex.Message);
            throw new Exception("Could not delete department. Please try again later.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in DeleteAsync method: Department repository due to " + ex.Message);
            throw new Exception("An unexpected error occurred. Please try again later.", ex);
        }
    }
}
