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
    public class PerformanceReviewControllerTests
    {
        private (PerformanceReviewController Controller, AppDbContext Context) CreateController()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);
            var controller = new PerformanceReviewController(context);

            // Clear tables at the start of each test
            context.PerformanceReviews.RemoveRange(context.PerformanceReviews);
            context.Employees.RemoveRange(context.Employees);
            context.SaveChanges();

            return (controller, context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithPerformanceReviews()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var review = new PerformanceReview
            {
                PerformanceReviewID = 1,
                EmployeeID = 1,
                ReviewDate = DateTime.UtcNow,
                ReviewComments = "Good performance.",
                Rating = 4,
                Employee = employee
            };

            context.Employees.Add(employee);
            context.PerformanceReviews.Add(review);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<PerformanceReview>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ReturnsViewWithPerformanceReview_WhenExists()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var review = new PerformanceReview
            {
                PerformanceReviewID = 1,
                EmployeeID = 1,
                ReviewDate = DateTime.UtcNow,
                ReviewComments = "Excellent performance.",
                Rating = 5,
                Employee = employee
            };

            context.Employees.Add(employee);
            context.PerformanceReviews.Add(review);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PerformanceReview>(viewResult.Model);
            Assert.Equal(1, model.PerformanceReviewID);
        }

        [Fact]
        public async Task Create_AddsPerformanceReviewAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            var review = new PerformanceReview
            {
                EmployeeID = 1,
                ReviewDate = DateTime.UtcNow,
                ReviewComments = "Average performance.",
                Rating = 3
            };

            // Act
            var result = await controller.Create(review);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(context.PerformanceReviews);
        }

        [Fact]
        public async Task Edit_UpdatesPerformanceReviewAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var review = new PerformanceReview
            {
                PerformanceReviewID = 1,
                EmployeeID = 1,
                ReviewDate = DateTime.UtcNow,
                ReviewComments = "Needs improvement.",
                Rating = 2
            };

            context.Employees.Add(employee);
            context.PerformanceReviews.Add(review);
            await context.SaveChangesAsync();

            // Update review
            review.ReviewComments = "Improved performance.";
            review.Rating = 4;

            // Act
            var result = await controller.Edit(1, review);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Improved performance.", context.PerformanceReviews.First().ReviewComments);
            Assert.Equal(4, context.PerformanceReviews.First().Rating);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesPerformanceReviewAndRedirects()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Seed data
            var employee = new Employee { EmployeeID = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var review = new PerformanceReview
            {
                PerformanceReviewID = 1,
                EmployeeID = 1,
                ReviewDate = DateTime.UtcNow,
                ReviewComments = "Satisfactory.",
                Rating = 3
            };

            context.Employees.Add(employee);
            context.PerformanceReviews.Add(review);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(await context.PerformanceReviews.FindAsync(1)); // Verify deletion
        }
    }
}
