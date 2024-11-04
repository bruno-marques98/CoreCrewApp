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
    public class EmployeeProjectControllerTests
    {
        private (EmployeeProjectController Controller, AppDbContext Context) CreateController()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);
            var controller = new EmployeeProjectController(context);

            // Clear data in EmployeeProjects, Employees, and Projects tables at the start of each test
            context.EmployeeProjects.RemoveRange(context.EmployeeProjects);
            context.Employees.RemoveRange(context.Employees);
            context.Projects.RemoveRange(context.Projects);
            context.SaveChanges();

            return (controller, context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithEmployeeProjects()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var project = new Project { ProjectID = 1, ProjectName = "Project Alpha", Description = "Test", StartDate = DateTime.Now, ManagerID = 1 };
            var employeeProject = new EmployeeProject { EmployeeID = 1, ProjectID = 1, AssignmentDate = DateTime.Today };

            context.Employees.Add(employee);
            context.Projects.Add(project);
            context.EmployeeProjects.Add(employeeProject);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<EmployeeProject>>(viewResult.Model);
            Assert.Single(model); // Verify only one employee-project assignment is returned
        }

        [Fact]
        public async Task Details_ReturnsViewWithEmployeeProject_WhenExists()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" };
            var project = new Project { ProjectID = 1, ProjectName = "Project Beta", Description = "Test", StartDate = DateTime.Now, ManagerID = 1 };
            var employeeProject = new EmployeeProject { EmployeeID = 1, ProjectID = 1, AssignmentDate = DateTime.Today };

            context.Employees.Add(employee);
            context.Projects.Add(project);
            context.EmployeeProjects.Add(employeeProject);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Details(1, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<EmployeeProject>(viewResult.Model);
            Assert.Equal(1, model.EmployeeID);
            Assert.Equal(1, model.ProjectID);
        }

        [Fact]
        public async Task Create_AddsEmployeeProjectAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com" };
            var project = new Project { ProjectID = 1, ProjectName = "Project Gamma", Description = "Test", StartDate = DateTime.Now, ManagerID = 1 };
            context.Employees.Add(employee);
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            // Create employee-project assignment
            var employeeProject = new EmployeeProject
            {
                EmployeeID = 1,
                ProjectID = 1,
                AssignmentDate = DateTime.Today
            };

            // Act
            var result = await controller.Create(employeeProject);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(context.EmployeeProjects); // Verify employee-project assignment was added
        }

        [Fact]
        public async Task Edit_UpdatesEmployeeProjectAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "Bob", LastName = "Williams", Email = "bob.williams@example.com" };
            var project = new Project { ProjectID = 1, ProjectName = "Project Delta", Description = "Test", StartDate = DateTime.Now, ManagerID = 1 };
            var employeeProject = new EmployeeProject { EmployeeID = 1, ProjectID = 1, AssignmentDate = DateTime.Today };

            context.Employees.Add(employee);
            context.Projects.Add(project);
            context.EmployeeProjects.Add(employeeProject);
            await context.SaveChangesAsync();

            // Update assignment date
            employeeProject.AssignmentDate = DateTime.Today.AddDays(-1);

            // Act
            var result = await controller.Edit(1, 1, employeeProject);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            var updatedEmployeeProject = await context.EmployeeProjects.FindAsync(1, 1);
            Assert.Equal(DateTime.Today.AddDays(-1), updatedEmployeeProject.AssignmentDate);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesEmployeeProjectAndRedirects()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "Charlie", LastName = "Brown", Email = "charlie.brown@example.com" };
            var project = new Project { ProjectID = 1, ProjectName = "Project Epsilon",Description="Test",StartDate = DateTime.Now,ManagerID = 1 };
            var employeeProject = new EmployeeProject { EmployeeID = 1, ProjectID = 1, AssignmentDate = DateTime.Today };

            context.Employees.Add(employee);
            context.Projects.Add(project);
            context.EmployeeProjects.Add(employeeProject);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteConfirmed(1, 1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(await context.EmployeeProjects.FindAsync(1, 1)); // Verify employee-project assignment was removed
        }
    }
}
