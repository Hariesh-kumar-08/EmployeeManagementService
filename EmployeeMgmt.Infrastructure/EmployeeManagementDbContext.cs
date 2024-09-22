using Microsoft.EntityFrameworkCore;
using EmployeeMgmt.Domain.Entities;
using EmployeeMgmt.Domain.ValueObjects;

public class EmployeeManagementDbContext : DbContext
{
    public EmployeeManagementDbContext(DbContextOptions<EmployeeManagementDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure entity relationships and constraints here if needed
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Employee>()
         .OwnsOne(e => e.Email, email =>
         {
             email.Property(e => e.Value).HasColumnName("Email"); // Change 'Value' to the actual property name
         });

        modelBuilder.Entity<Employee>()
            .OwnsOne(e => e.EmployeeCode, employeeCode =>
            {
                employeeCode.Property(e => e.Value).HasColumnName("EmployeeCode"); // Change 'Value' to the actual property name
            });
        


    }
}

