using CoreCrewApp.Controllers;
using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CoreCrewApp.Tests.Controllers
{
    public class RoleControllerTests
    {
        private (RoleController Controller, AppDbContext Context) CreateController()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);
            var controller = new RoleController(context);

            // Clear tables at the start of each test
            context.Roles.RemoveRange(context.Roles);
            context.Employees.RemoveRange(context.Employees);
            context.SaveChanges();

            return (controller, context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithRoles()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var role = new Role { RoleID = 1, RoleName = "Developer" };
            context.Roles.Add(role);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Role>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ReturnsViewWithRole_WhenRoleExists()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var role = new Role { RoleID = 1, RoleName = "Manager" };
            context.Roles.Add(role);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Role>(viewResult.Model);
            Assert.Equal(1, model.RoleID);
        }

        [Fact]
        public async Task Create_AddsRoleAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            var role = new Role { RoleName = "Administrator" };

            // Act
            var result = await controller.Create(role);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(context.Roles);
        }

        [Fact]
        public async Task Edit_UpdatesRoleAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var role = new Role { RoleID = 1, RoleName = "Analyst" };
            context.Roles.Add(role);
            await context.SaveChangesAsync();

            // Update role
            role.RoleName = "Senior Analyst";

            // Act
            var result = await controller.Edit(1, role);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Senior Analyst", context.Roles.First().RoleName);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesRoleAndRedirects()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var role = new Role { RoleID = 1, RoleName = "Designer" };
            context.Roles.Add(role);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(await context.Roles.FindAsync(1)); // Verify deletion
        }
    }
}
