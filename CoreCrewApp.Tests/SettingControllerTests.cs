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
    public class SettingControllerTests
    {
        private (SettingController Controller, AppDbContext Context) CreateController()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);
            var controller = new SettingController(context);

            // Clear tables at the start of each test
            context.Settings.RemoveRange(context.Settings);
            context.SaveChanges();

            return (controller, context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithSettings()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var setting = new Setting { SettingID = 1, Name = "TestSetting", Value = "TestValue", Type = "String",Description="Test setting" };
            context.Settings.Add(setting);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Setting>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ReturnsViewWithSetting_WhenSettingExists()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var setting = new Setting { SettingID = 1, Name = "TestSetting", Value = "TestValue", Type = "String", Description = "Test setting" };
            context.Settings.Add(setting);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Setting>(viewResult.Model);
            Assert.Equal(1, model.SettingID);
        }

        [Fact]
        public async Task Create_AddsSettingAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            var setting = new Setting { SettingID = 1, Name = "NewSetting", Value = "NewValue", Type = "String", Description = "Test setting" };

            // Act
            var result = await controller.Create(setting);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(context.Settings);
        }

        [Fact]
        public async Task Edit_UpdatesSettingAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var setting = new Setting { SettingID = 1, Name = "OldSetting", Value = "OldValue", Type = "String", Description = "Test setting" };
            context.Settings.Add(setting);
            await context.SaveChangesAsync();

            // Update setting
            setting.Value = "UpdatedValue";

            // Act
            var result = await controller.Edit(1, setting);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("UpdatedValue", context.Settings.First().Value);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesSettingAndRedirects()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var setting = new Setting { SettingID = 1, Name = "DeleteSetting", Value = "DeleteValue", Type = "String", Description = "Test setting" };
            context.Settings.Add(setting);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(await context.Settings.FindAsync(1)); // Verify deletion
        }
    }
}
