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
    public class NotificationControllerTests
    {
        private (NotificationController Controller, AppDbContext Context) CreateController()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);
            var controller = new NotificationController(context);

            // Clear tables at the start of each test
            context.Notifications.RemoveRange(context.Notifications);
            context.Employees.RemoveRange(context.Employees);
            context.SaveChanges();

            return (controller, context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithNotifications()
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
            var notification = new Notification
            {
                NotificationID = 1,
                EmployeeID = 1,
                Title = "Test Notification",
                Message = "This is a test notification.",
                Timestamp = DateTime.UtcNow,
                IsRead = false,
                Employee = employee
            };

            context.Employees.Add(employee);
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Notification>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ReturnsViewWithNotification_WhenExists()
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
            var notification = new Notification
            {
                NotificationID = 1,
                EmployeeID = 1,
                Title = "Test Notification",
                Message = "This is a test notification.",
                Timestamp = DateTime.UtcNow,
                IsRead = false,
                Employee = employee
            };

            context.Employees.Add(employee);
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Notification>(viewResult.Model);
            Assert.Equal(1, model.NotificationID);
        }

        [Fact]
        public async Task Create_AddsNotificationAndRedirects_WhenModelIsValid()
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

            // Create Notification
            var notification = new Notification
            {
                EmployeeID = 1,
                Title = "New Notification",
                Message = "This is a new notification.",
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };

            // Act
            var result = await controller.Create(notification);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(context.Notifications);
        }

        [Fact]
        public async Task Edit_UpdatesNotificationAndRedirects_WhenModelIsValid()
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
            var notification = new Notification
            {
                NotificationID = 1,
                EmployeeID = 1,
                Title = "Test Notification",
                Message = "This is a test notification.",
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };

            context.Employees.Add(employee);
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            // Update notification
            notification.Message = "Updated Message";
            notification.IsRead = true;

            // Act
            var result = await controller.Edit(1, notification);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Updated Message", context.Notifications.First().Message);
            Assert.True(context.Notifications.First().IsRead);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesNotificationAndRedirects()
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
            var notification = new Notification
            {
                NotificationID = 1,
                EmployeeID = 1,
                Title = "Test Notification",
                Message = "This is a test notification.",
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };

            context.Employees.Add(employee);
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(await context.Notifications.FindAsync(1)); // Verify the record was deleted
        }
    }
}
