using EmployeeMgmt.Domain.Entities;
using EmployeeMgmt.Domain.Interfaces;
using EmployeeMgmt.Domain.ValueObjects;
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
    public class EmployeeRepositoryTests
    {
        private readonly Mock<ILogger<EmployeeRepository>> _mockLogger;

        public EmployeeRepositoryTests()
        {
            _mockLogger = new Mock<ILogger<EmployeeRepository>>();
        }

        private EmployeeRepository CreateRepository()
        {
            var options = new DbContextOptionsBuilder<EmployeeManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database for each test
                .Options;

            var context = new EmployeeManagementDbContext(options);
            return new EmployeeRepository(context, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEmployees()
        {
            // Arrange
            var employeeRepository = CreateRepository();
            var employee1 = new Employee(new EmployeeCode("E001"), "John Doe", new EmailAddress("john.doe@example.com"), DateTime.Now, 1);
            var employee2 = new Employee(new EmployeeCode("E002"), "Jane Smith", new EmailAddress("jane.smith@example.com"), DateTime.Now, 1);

            await employeeRepository.AddAsync(employee1);
            await employeeRepository.AddAsync(employee2);

            // Act
            var result = await employeeRepository.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(e => e.Name == "John Doe");
            result.Should().Contain(e => e.Name == "Jane Smith");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEmployee_WhenExists()
        {
            // Arrange
            var employeeRepository = CreateRepository();
            var employee = new Employee(new EmployeeCode("E002"), "Jane Smith", new EmailAddress("jane.smith@example.com"), DateTime.Now, 1);
            await employeeRepository.AddAsync(employee);

            // Act
            var result = await employeeRepository.GetByIdAsync(employee.EmployeeId);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Jane Smith");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            var employeeRepository = CreateRepository();

            // Act
            Func<Task> act = async () => await employeeRepository.GetByIdAsync(999);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                     .WithMessage("Requested employee not found.*")
                     .Where(ex => ex.InnerException is KeyNotFoundException);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEmployee()
        {
            // Arrange
            var employeeRepository = CreateRepository();
            var employee = new Employee(new EmployeeCode("E003"), "Mark Johnson", new EmailAddress("mark.johnson@example.com"), DateTime.Now, 1);

            // Act
            await employeeRepository.AddAsync(employee);

            // Assert
            var result = await employeeRepository.GetByIdAsync(employee.EmployeeId);
            result.Should().NotBeNull();
            result.Name.Should().Be("Mark Johnson");
        }

        //[Fact]
        //public async Task UpdateAsync_ShouldUpdateEmployee()
        //{
        //    // Arrange
        //    var employeeRepository = CreateRepository();
        //    var employee = new Employee(new EmployeeCode("E004"), "Emily Davis", new EmailAddress("emily.davis@example.com"), DateTime.Now, 1);
        //    await employeeRepository.AddAsync(employee);

        //    employee.Name = "Emily Davis Updated"; // Update the name

        //    // Act
        //    await employeeRepository.UpdateAsync(employee);

        //    // Assert
        //    var updatedEmployee = await employeeRepository.GetByIdAsync(employee.EmployeeId);
        //    updatedEmployee.Name.Should().Be("Emily Davis Updated");
        //}

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEmployee()
        {
            // Arrange
            var employeeRepository = CreateRepository();
            var employee = new Employee(new EmployeeCode("E005"), "Michael Brown", new EmailAddress("michael.brown@example.com"), DateTime.Now, 1);
            await employeeRepository.AddAsync(employee);

            // Act
            await employeeRepository.DeleteAsync(employee);

            // Assert
            var deletedEmployee = await employeeRepository.GetByIdAsync(employee.EmployeeId);
            deletedEmployee.Should().BeNull();
        }
    }
}
