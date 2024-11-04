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
    public class ProjectControllerTests
    {
        private (ProjectController Controller, AppDbContext Context) CreateController()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);
            var controller = new ProjectController(context);

            // Clear tables at the start of each test
            context.Projects.RemoveRange(context.Projects);
            context.Employees.RemoveRange(context.Employees);
            context.SaveChanges();

            return (controller, context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithProjects()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var manager = new Employee { EmployeeID = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com" };
            var project = new Project
            {
                ProjectID = 1,
                ProjectName = "Project A",
                Description = "Sample project",
                StartDate = DateTime.UtcNow,
                ManagerID = 1,
                Manager = manager
            };

            context.Employees.Add(manager);
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Project>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ReturnsViewWithProject_WhenExists()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var manager = new Employee { EmployeeID = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com" };
            var project = new Project
            {
                ProjectID = 1,
                ProjectName = "Project B",
                Description = "Detailed project",
                StartDate = DateTime.UtcNow,
                ManagerID = 1,
                Manager = manager
            };

            context.Employees.Add(manager);
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Project>(viewResult.Model);
            Assert.Equal(1, model.ProjectID);
        }

        [Fact]
        public async Task Create_AddsProjectAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var manager = new Employee { EmployeeID = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com" };
            context.Employees.Add(manager);
            await context.SaveChangesAsync();

            var project = new Project
            {
                ProjectName = "Project C",
                Description = "New project",
                StartDate = DateTime.UtcNow,
                ManagerID = 1
            };

            // Act
            var result = await controller.Create(project);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(context.Projects);
        }

        [Fact]
        public async Task Edit_UpdatesProjectAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var manager = new Employee { EmployeeID = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com" };
            var project = new Project
            {
                ProjectID = 1,
                ProjectName = "Project D",
                Description = "Initial project description",
                StartDate = DateTime.UtcNow,
                ManagerID = 1,
                Manager = manager
            };

            context.Employees.Add(manager);
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            // Update project
            project.Description = "Updated project description";

            // Act
            var result = await controller.Edit(1, project);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Updated project description", context.Projects.First().Description);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesProjectAndRedirects()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var manager = new Employee { EmployeeID = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com" };
            var project = new Project
            {
                ProjectID = 1,
                ProjectName = "Project E",
                Description = "Project to delete",
                StartDate = DateTime.UtcNow,
                ManagerID = 1,
                Manager = manager
            };

            context.Employees.Add(manager);
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(await context.Projects.FindAsync(1)); // Verify deletion
        }
    }
}
