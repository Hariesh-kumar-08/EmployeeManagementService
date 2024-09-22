using EmployeeMgmt.Domain.Entities;
using EmployeeMgmt.Domain.ValueObjects;
using System;
using System.Linq;

public static class AppDbInitializer
{
    public static void SeedData(EmployeeManagementDbContext context)
    {
        // Check if the database is already seeded
        if (context.Departments.Any() || context.Employees.Any())
            return; // DB has been seeded

        // Seed Departments
        var departments = new Department[]
        {
              new Department { DepartmentName = "HR", Description = "Human Resources" },
            new Department {  DepartmentName = "IT", Description = "Information Technology" },
            new Department { DepartmentName = "Finance", Description = "Financial Department" },
            new Department {  DepartmentName = "Marketing", Description = "Marketing Department" }
        };

        context.Departments.AddRange(departments);
        context.SaveChanges();

        // Seed Employees
        var employees = new Employee[]
        {
          new Employee(new EmployeeCode("E001"), "Alice Johnson", new EmailAddress("alice@example.com"), DateTime.Now.AddYears(-2), 1),
                new Employee(new EmployeeCode("E002"), "Bob Smith", new EmailAddress("bob@example.com"), DateTime.Now.AddYears(-1), 2),
                new Employee(new EmployeeCode("E003"), "Charlie Brown", new EmailAddress("charlie@example.com"), DateTime.Now.AddMonths(-6), 3),
                new Employee(new EmployeeCode("E004"), "David Wilson", new EmailAddress("david@example.com"), DateTime.Now.AddMonths(-8), 1),
                new Employee(new EmployeeCode("E005"), "Emma Davis", new EmailAddress("emma@example.com"), DateTime.Now.AddYears(-3), 4),
                new Employee(new EmployeeCode("E006"), "Fiona Garcia", new EmailAddress("fiona@example.com"), DateTime.Now.AddYears(-1), 2),
                new Employee(new EmployeeCode("E007"), "George Martinez", new EmailAddress("george@example.com"), DateTime.Now.AddMonths(-2), 3),
                new Employee(new EmployeeCode("E008"), "Hannah Rodriguez", new EmailAddress("hannah@example.com"), DateTime.Now.AddMonths(-5), 4),
                new Employee(new EmployeeCode("E009"), "Ian Lee", new EmailAddress("ian@example.com"), DateTime.Now.AddYears(-4), 1),
                new Employee(new EmployeeCode("E010"), "Julia Walker", new EmailAddress("julia@example.com"), DateTime.Now.AddYears(-2), 2)
        };

        context.Employees.AddRange(employees);
        context.SaveChanges();
    }
}
