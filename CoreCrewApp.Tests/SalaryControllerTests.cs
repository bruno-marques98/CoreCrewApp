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
    public class SalaryControllerTests
    {
        private (SalaryController Controller, AppDbContext Context) CreateController()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);
            var controller = new SalaryController(context);

            // Clear tables at the start of each test
            context.Salaries.RemoveRange(context.Salaries);
            context.Employees.RemoveRange(context.Employees);
            context.SaveChanges();

            return (controller, context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithSalaries()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com" };
            var salary = new Salary { SalaryID = 1, EmployeeID = 1, Amount = 5000m, EffectiveDate = DateTime.Today };
            context.Employees.Add(employee);
            context.Salaries.Add(salary);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Salary>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ReturnsViewWithSalary_WhenSalaryExists()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com" };
            var salary = new Salary { SalaryID = 1, EmployeeID = 1, Amount = 5000m, EffectiveDate = DateTime.Today };
            context.Employees.Add(employee);
            context.Salaries.Add(salary);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Salary>(viewResult.Model);
            Assert.Equal(1, model.SalaryID);
        }

        [Fact]
        public async Task Create_AddsSalaryAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            var employee = new Employee { EmployeeID = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com" };
            var salary = new Salary { SalaryID = 1, EmployeeID = 1, Amount = 4500m, EffectiveDate = DateTime.Today };

            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Create(salary);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(context.Salaries);
        }

        [Fact]
        public async Task Edit_UpdatesSalaryAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com" };
            var salary = new Salary { SalaryID = 1, EmployeeID = 1, Amount = 5500m, EffectiveDate = DateTime.Today };
            context.Employees.Add(employee);
            context.Salaries.Add(salary);
            await context.SaveChangesAsync();

            // Update salary amount
            salary.Amount = 6000m;

            // Act
            var result = await controller.Edit(1, salary);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal(6000m, context.Salaries.First().Amount);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesSalaryAndRedirects()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com" };
            var salary = new Salary { SalaryID = 1, EmployeeID = 1, Amount = 5000m, EffectiveDate = DateTime.Today };
            context.Employees.Add(employee);
            context.Salaries.Add(salary);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(await context.Salaries.FindAsync(1)); // Verify deletion
        }
    }
}
