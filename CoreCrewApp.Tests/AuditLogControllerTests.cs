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
    public class AuditLogControllerTests
    {
        private (AuditLogController, AppDbContext) CreateController()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            var context = new AppDbContext(options);
            var controller = new AuditLogController(context);

            return (controller, context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithAuditLogs()
        {
            // Arrange
            var (controller, context) = CreateController();
            var auditLogs = new List<AuditLog>
        {
            new AuditLog { AuditLogID = 1, Action = "Create", TableName = "Users", UserName = "Admin", Timestamp = DateTime.Now, Details = "Created user" },
            new AuditLog { AuditLogID = 2, Action = "Delete", TableName = "Orders", UserName = "Admin", Timestamp = DateTime.Now, Details = "Deleted order" }
        };
            await context.AuditLogs.AddRangeAsync(auditLogs);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<AuditLog>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Details_ReturnsViewWithAuditLog_WhenIdIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Clear existing entries to avoid conflicts
            context.AuditLogs.RemoveRange(context.AuditLogs);
            await context.SaveChangesAsync();

            // Add a new audit log entry without setting AuditLogID
            var auditLog = new AuditLog
            {
                Action = "ViewDetails",
                TableName = "Users",
                UserName = "TestUser",
                Timestamp = DateTime.Now,
                Details = "Viewed user details"
            };

            await context.AuditLogs.AddAsync(auditLog);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Details(auditLog.AuditLogID);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AuditLog>(viewResult.Model);
            Assert.Equal(auditLog.AuditLogID, model.AuditLogID); // Check that the correct entry is returned
        }


        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var (controller, _) = CreateController();

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_Post_ReturnsRedirectToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Clear the AuditLogs table
            context.AuditLogs.RemoveRange(context.AuditLogs);
            await context.SaveChangesAsync();

            var auditLog = new AuditLog
            {
                Action = "Create",
                TableName = "Users",
                UserName = "Admin",
                Timestamp = DateTime.Now,
                Details = "Created user"
            };

            // Act
            var result = await controller.Create(auditLog);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(context.AuditLogs); // Check if only one auditLog was added
        }


        [Fact]
        public async Task Create_Post_ReturnsViewWithModel_WhenModelStateIsInvalid()
        {
            // Arrange
            var (controller, _) = CreateController();
            controller.ModelState.AddModelError("Action", "Required"); // Simulate invalid model state
            var auditLog = new AuditLog { AuditLogID = 1 };

            // Act
            var result = await controller.Create(auditLog);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(auditLog, viewResult.Model);
        }

        [Fact]
        public async Task Edit_Post_ReturnsNotFound_WhenIdDoesNotMatchAuditLogID()
        {
            // Arrange
            var (controller, _) = CreateController();
            var auditLog = new AuditLog { AuditLogID = 1, Action = "Update", TableName = "Users", UserName = "Admin", Timestamp = DateTime.Now, Details = "Updated user" };

            // Act
            var result = await controller.Edit(2, auditLog); // ID does not match auditLogID

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_ReturnsRedirectToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();
            var auditLog = new AuditLog { AuditLogID = 1, Action = "Create", TableName = "Users", UserName = "Admin", Timestamp = DateTime.Now, Details = "Created user" };
            await context.AuditLogs.AddAsync(auditLog);
            await context.SaveChangesAsync();

            // Modify auditLog for edit
            auditLog.Action = "Update";

            // Act
            var result = await controller.Edit(auditLog.AuditLogID, auditLog);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Update", context.AuditLogs.First().Action); // Confirm change
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesAuditLogAndRedirects()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Clear the AuditLogs table to ensure no leftover data
            context.AuditLogs.RemoveRange(context.AuditLogs);
            await context.SaveChangesAsync();

            // Add a new audit log entry without setting AuditLogID
            var auditLog = new AuditLog
            {
                Action = "Delete",
                TableName = "Orders",
                UserName = "Admin",
                Timestamp = DateTime.Now,
                Details = "Deleted order"
            };

            await context.AuditLogs.AddAsync(auditLog);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteConfirmed(auditLog.AuditLogID);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Empty(context.AuditLogs); // Confirm the audit log was removed
        }

    }
}