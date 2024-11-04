using CoreCrewApp.Controllers;
using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CoreCrewApp.Tests.Controllers
{
    public class EmployeeBenefitControllerTests
    {
        private (EmployeeBenefitController Controller, AppDbContext Context) CreateController()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}") // Unique database for each test
                .Options;

            var context = new AppDbContext(options);
            var controller = new EmployeeBenefitController(context);

            // Clear data in EmployeeBenefits, Employees, and Benefits tables at the start of each test
            context.EmployeeBenefits.RemoveRange(context.EmployeeBenefits);
            context.Employees.RemoveRange(context.Employees);
            context.Benefits.RemoveRange(context.Benefits);
            context.SaveChanges();

            return (controller, context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithEmployeeBenefits()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, DepartmentID = 1, FirstName = "John", LastName = "Doe", Email = "test@test.com", HireDate = new DateTime(1985, 1, 1) };

            var benefit = new Benefit { BenefitID = 1, Name = "Health Insurance", Description = "Test benefit" };
            await context.Employees.AddAsync(employee);
            await context.Benefits.AddAsync(benefit);
            await context.EmployeeBenefits.AddAsync(new EmployeeBenefit { EmployeeID = 1, BenefitID = 1, EnrollmentDate = DateTime.Today });
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<EmployeeBenefit>>(viewResult.Model);
            Assert.Single(model); // Check only one EmployeeBenefit is returned
        }
        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var (controller, _) = CreateController();

            // Act
            var result = await controller.Details(null, null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenEmployeeBenefitDoesNotExist()
        {
            // Arrange
            var (controller, _) = CreateController();

            // Act
            var result = await controller.Details(1, 1); // IDs that do not exist

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsRedirect_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, DepartmentID = 1, FirstName = "John", LastName = "Doe", Email = "test@test.com", HireDate = new DateTime(1985, 1, 1) };
            var benefit = new Benefit { BenefitID = 1, Name = "Health Insurance",Description="Test benefit" };
            await context.Employees.AddAsync(employee);
            await context.Benefits.AddAsync(benefit);
            await context.SaveChangesAsync();

            // Act
            var employeeBenefit = new EmployeeBenefit { EmployeeID = 1, BenefitID = 1, EnrollmentDate = DateTime.Today };
            var result = await controller.Create(employeeBenefit);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(context.EmployeeBenefits); // Verify one EmployeeBenefit was added
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenEmployeeBenefitDoesNotExist()
        {
            // Arrange
            var (controller, _) = CreateController();

            // Act
            var result = await controller.Edit(1, 1); // IDs that do not exist

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_UpdatesEmployeeBenefitAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, DepartmentID = 1, FirstName = "John", LastName = "Doe", Email = "test@test.com", HireDate = new DateTime(1985, 1, 1) };

            var benefit = new Benefit { BenefitID = 1, Name = "Health Insurance", Description = "Test benefit" };
            var employeeBenefit = new EmployeeBenefit { EmployeeID = 1, BenefitID = 1, EnrollmentDate = DateTime.Today };
            await context.Employees.AddAsync(employee);
            await context.Benefits.AddAsync(benefit);
            await context.EmployeeBenefits.AddAsync(employeeBenefit);
            await context.SaveChangesAsync();

            // Act
            employeeBenefit.EnrollmentDate = DateTime.Today.AddDays(1); // Change the enrollment date
            var result = await controller.Edit(1, 1, employeeBenefit);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            var updatedEmployeeBenefit = await context.EmployeeBenefits.FindAsync(1, 1);
            Assert.Equal(DateTime.Today.AddDays(1), updatedEmployeeBenefit.EnrollmentDate);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesEmployeeBenefitAndRedirects()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, DepartmentID = 1, FirstName = "John", LastName = "Doe", Email = "test@test.com", HireDate = new DateTime(1985, 1, 1) };

            var benefit = new Benefit { BenefitID = 1, Name = "Health Insurance", Description = "Test benefit" };
            var employeeBenefit = new EmployeeBenefit { EmployeeID = 1, BenefitID = 1, EnrollmentDate = DateTime.Today };
            await context.Employees.AddAsync(employee);
            await context.Benefits.AddAsync(benefit);
            await context.EmployeeBenefits.AddAsync(employeeBenefit);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteConfirmed(1, 1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            var deletedEmployeeBenefit = await context.EmployeeBenefits.FindAsync(1, 1);
            Assert.Null(deletedEmployeeBenefit); // Verify deletion
        }
    }
}
