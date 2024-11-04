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
    public class EmployeeRoleControllerTests
    {
        private (EmployeeRoleController Controller, AppDbContext Context) CreateController()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);
            var controller = new EmployeeRoleController(context);

            // Clear data in EmployeeProjects, Employees, and Projects tables at the start of each test
            context.EmployeeRoles.RemoveRange(context.EmployeeRoles);
            context.Employees.RemoveRange(context.Employees);
            context.Roles.RemoveRange(context.Roles);
            context.SaveChanges();

            return (controller, context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithEmployeeRoles()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var role = new Role { RoleID = 1, RoleName = "Project Alpha" };
            var employeeRole = new EmployeeRole { EmployeeID = 1, RoleID= 1};

            context.Employees.Add(employee);
            context.Roles.Add(role);
            context.EmployeeRoles.Add(employeeRole);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<EmployeeRole>>(viewResult.Model);
            Assert.Single(model); // Verify only one employee-project assignment is returned
        }

        [Fact]
        public async Task Details_ReturnsViewWithEmployeeRole_WhenExists()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var role = new Role { RoleID = 1, RoleName = "Project Alpha" };
            var employeeRole = new EmployeeRole { EmployeeID = 1, RoleID = 1 };

            context.Employees.Add(employee);
            context.Roles.Add(role);
            context.EmployeeRoles.Add(employeeRole);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Details(1, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<EmployeeRole>(viewResult.Model);
            Assert.Equal(1, model.EmployeeID);
            Assert.Equal(1, model.RoleID);
        }

        [Fact]
        public async Task Create_AddsEmployeeRoleAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var role = new Role { RoleID = 1, RoleName = "Project Alpha" };
            context.Employees.Add(employee);
            context.Roles.Add(role);
            await context.SaveChangesAsync();

            // Create employee-role assignment
            var employeeRole = new EmployeeRole
            {
                EmployeeID = 1,
                RoleID = 1
            };

            // Act
            var result = await controller.Create(employeeRole);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(context.EmployeeRoles); // Verify employee-role assignment was added
        }

        [Fact]
        public async Task Edit_UpdatesEmployeeRoleAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data

            var employee = new Employee
            {
                EmployeeID = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            var role1 = new Role { RoleID = 1, RoleName = "Project Alpha" };
            var role2 = new Role { RoleID = 2, RoleName = "Project Beta" };
            context.Employees.Add(employee);
            context.Roles.Add(role1);
            context.Roles.Add(role2);
            var employeeRole = new EmployeeRole { EmployeeID = 1, RoleID = 1 };
            context.EmployeeRoles.Add(employeeRole);
            await context.SaveChangesAsync();

            employeeRole.AssignDate = DateTime.Now;

            // Act
            var result = await controller.Edit(1, 1, employeeRole);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }



        [Fact]
        public async Task DeleteConfirmed_RemovesEmployeeRoleAndRedirects()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var role = new Role { RoleID = 1, RoleName = "Project Alpha" };
            var employeeRole = new EmployeeRole { EmployeeID = 1, RoleID = 1 };

            context.Employees.Add(employee);
            context.Roles.Add(role);
            context.EmployeeRoles.Add(employeeRole);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteConfirmed(1, 1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(await context.EmployeeRoles.FindAsync(1, 1)); // Verify employee-role assignment was removed
        }
    }
}
