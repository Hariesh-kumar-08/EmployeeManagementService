using EmployeeMgmt.Domain.Entities;
using EmployeeMgmt.Domain.Interfaces;
using EmployeeMgmt.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeMgmt.Tests.Repositories
{
    public class DepartmentRepositoryTests
    {
        private readonly Mock<ILogger<DepartmentRepository>> _mockLogger;
        private readonly DepartmentRepository _departmentRepository;
        private readonly EmployeeManagementDbContext _context;

        public DepartmentRepositoryTests()
        {
            _mockLogger = new Mock<ILogger<DepartmentRepository>>();

            var options = new DbContextOptionsBuilder<EmployeeManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new EmployeeManagementDbContext(options);
            _departmentRepository = new DepartmentRepository(_context, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllDepartments()
        {
            // Arrange
            var department = new Department { DepartmentId = 1, DepartmentName = "HR", Description = "HR" };
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();

            // Act
            var result = await _departmentRepository.GetAllAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainSingle(d => d.DepartmentName == "HR");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDepartment_WhenExists()
        {
            // Arrange
            var department = new Department { DepartmentId = 2, DepartmentName = "IT", Description = "IT" };
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();

            // Act
            var result = await _departmentRepository.GetByIdAsync(2);

            // Assert
            result.Should().NotBeNull();
            result.DepartmentName.Should().Be("IT");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Act
            Func<Task> act = async () => await _departmentRepository.GetByIdAsync(999);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                     .WithMessage("Requested department not found.*")
                    .Where(ex => ex.InnerException is KeyNotFoundException);

        }

        [Fact]
        public async Task AddAsync_ShouldAddDepartment()
        {
            // Arrange
            var department = new Department { DepartmentName = "IT", Description = "HR" };

            // Act
            await _departmentRepository.AddAsync(department);

            // Assert
            var result = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentName == "IT");
            result.Should().NotBeNull();
            result.DepartmentName.Should().Be("IT");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateDepartment()
        {
            // Arrange
            var department = new Department { DepartmentId = 6, DepartmentName = "Sales", Description = "Sale" };
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();

            department.DepartmentName = "Sales and Marketing";

            // Act
            await _departmentRepository.UpdateAsync(department);

            // Assert
            var updatedDepartment = await _departmentRepository.GetByIdAsync(6);
            updatedDepartment.DepartmentName.Should().Be("Sales and Marketing");
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveDepartment()
        {
            // Arrange
            var department = new Department { DepartmentId = 9, DepartmentName = "Marketing", Description = "Marketing" };
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();

            // Act
            await _departmentRepository.DeleteAsync(department);

            // Assert: Confirm that the specific department has been deleted
            var deletedDepartment = await _context.Departments.FindAsync(department.DepartmentId);
            deletedDepartment.Should().BeNull();
        }


    }
}
