using CoreCrewApp.Controllers;
using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CoreCrewApp.Tests.Controllers
{
    public class EmployeeControllerTests
    {
        private (EmployeeController Controller, AppDbContext Context) CreateController()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);
            var controller = new EmployeeController(context);

            // Clear data in Employees and Departments tables at the start of each test
            context.Employees.RemoveRange(context.Employees);
            context.Departments.RemoveRange(context.Departments);
            context.SaveChanges();

            return (controller, context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithEmployees()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var department = new Department { DepartmentId = 1, DepartmentName = "IT" };
            var employee = new Employee
            {
                EmployeeID = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                HireDate = DateTime.Today,
                DepartmentID = 1,
                Department = department
            };
            context.Departments.Add(department);
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Employee>>(viewResult.Model);
            Assert.Single(model); // Verify only one employee is returned
        }

        [Fact]
        public async Task Details_ReturnsViewWithEmployee_WhenEmployeeExists()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var department = new Department { DepartmentId = 1, DepartmentName = "IT" };
            var employee = new Employee
            {
                EmployeeID = 1,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                HireDate = DateTime.Today,
                DepartmentID = 1,
                Department = department
            };
            context.Departments.Add(department);
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Employee>(viewResult.Model);
            Assert.Equal("Jane", model.FirstName);
            Assert.Equal("Smith", model.LastName);
        }

        [Fact]
        public async Task Create_AddsEmployeeAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed department data
            var department = new Department { DepartmentId = 1, DepartmentName = "HR" };
            context.Departments.Add(department);
            await context.SaveChangesAsync();

            // Create employee
            var employee = new Employee
            {
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.com",
                HireDate = DateTime.Today,
                DepartmentID = 1
            };

            // Act
            var result = await controller.Create(employee);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(context.Employees); // Verify employee was added
        }

        [Fact]
        public async Task Edit_UpdatesEmployeeAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var department = new Department { DepartmentId = 1, DepartmentName = "Finance" };
            var employee = new Employee
            {
                EmployeeID = 1,
                FirstName = "Bob",
                LastName = "Williams",
                Email = "bob.williams@example.com",
                HireDate = DateTime.Today,
                DepartmentID = 1,
                Department = department
            };
            context.Departments.Add(department);
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            // Update employee details
            employee.FirstName = "Robert";
            employee.LastName = "Williamson";

            // Act
            var result = await controller.Edit(1, employee);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            var updatedEmployee = await context.Employees.FindAsync(1);
            Assert.Equal("Robert", updatedEmployee.FirstName);
            Assert.Equal("Williamson", updatedEmployee.LastName);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesEmployeeAndRedirects()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var department = new Department { DepartmentId = 1, DepartmentName = "Marketing" };
            var employee = new Employee
            {
                EmployeeID = 1,
                FirstName = "Charlie",
                LastName = "Brown",
                Email = "charlie.brown@example.com",
                HireDate = DateTime.Today,
                DepartmentID = 1,
                Department = department
            };
            context.Departments.Add(department);
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(await context.Employees.FindAsync(1)); // Verify employee was removed
        }
    }
}
