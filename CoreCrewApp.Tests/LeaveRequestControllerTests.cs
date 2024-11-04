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
    public class LeaveRequestControllerTests
    {
        private (LeaveRequestController Controller, AppDbContext Context) CreateController()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);
            var controller = new LeaveRequestController(context);

            // Clear tables at the start of each test
            context.LeaveRequests.RemoveRange(context.LeaveRequests);
            context.Employees.RemoveRange(context.Employees);
            context.SaveChanges();

            return (controller, context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithLeaveRequests()
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
            var leaveRequest = new LeaveRequest
            {
                LeaveRequestID = 1,
                EmployeeID = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                Reason = "Family event",
                Status = LeaveStatus.Pending,
                Employee = employee
            };

            context.Employees.Add(employee);
            context.LeaveRequests.Add(leaveRequest);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<LeaveRequest>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ReturnsViewWithLeaveRequest_WhenExists()
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
            var leaveRequest = new LeaveRequest
            {
                LeaveRequestID = 1,
                EmployeeID = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                Reason = "Family event",
                Status = LeaveStatus.Pending,
                Employee = employee
            };

            context.Employees.Add(employee);
            context.LeaveRequests.Add(leaveRequest);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<LeaveRequest>(viewResult.Model);
            Assert.Equal(1, model.LeaveRequestID);
        }

        [Fact]
        public async Task Create_AddsLeaveRequestAndRedirects_WhenModelIsValid()
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
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            // Create LeaveRequest
            var leaveRequest = new LeaveRequest
            {
                EmployeeID = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                Reason = "Family event",
                Status = LeaveStatus.Pending
            };

            // Act
            var result = await controller.Create(leaveRequest);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(context.LeaveRequests);
        }

        [Fact]
        public async Task Edit_UpdatesLeaveRequestAndRedirects_WhenModelIsValid()
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
            var leaveRequest = new LeaveRequest
            {
                LeaveRequestID = 1,
                EmployeeID = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                Reason = "Family event",
                Status = LeaveStatus.Pending
            };

            context.Employees.Add(employee);
            context.LeaveRequests.Add(leaveRequest);
            await context.SaveChangesAsync();

            // Update leave request
            leaveRequest.Reason = "Updated reason";
            leaveRequest.Status = LeaveStatus.Approved;

            // Act
            var result = await controller.Edit(1, leaveRequest);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Updated reason", context.LeaveRequests.First().Reason);
            Assert.Equal(LeaveStatus.Approved, context.LeaveRequests.First().Status);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesLeaveRequestAndRedirects()
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
            var leaveRequest = new LeaveRequest
            {
                LeaveRequestID = 1,
                EmployeeID = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                Reason = "Family event",
                Status = LeaveStatus.Pending
            };

            context.Employees.Add(employee);
            context.LeaveRequests.Add(leaveRequest);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(await context.LeaveRequests.FindAsync(1)); // Verify the record was deleted
        }
    }
}
