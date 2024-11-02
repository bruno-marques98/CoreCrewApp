using CoreCrewApp.Controllers;
using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreCrewApp.Tests.Controllers
{
    public class BenefitControllerTests
    {
        private (BenefitController, AppDbContext) CreateController()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "BenefitTestDb")
                .Options;

            var context = new AppDbContext(options);
            var controller = new BenefitController(context);

            return (controller, context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithBenefits()
        {
            // Arrange
            var (controller, context) = CreateController();
            var benefits = new List<Benefit>
            {
                new Benefit { Name = "Health Insurance", Description = "Medical coverage", Cost = 200 },
                new Benefit { Name = "Retirement Plan", Description = "Pension benefits", Cost = 100 }
            };
            await context.Benefits.AddRangeAsync(benefits);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Benefit>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Details_ReturnsViewWithBenefit_WhenIdIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();
            var benefit = new Benefit { Name = "Health Insurance", Description = "Medical coverage", Cost = 200 };
            await context.Benefits.AddAsync(benefit);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Details(benefit.BenefitID);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Benefit>(viewResult.Model);
            Assert.Equal(benefit.BenefitID, model.BenefitID);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsInvalid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Act
            var result = await controller.Details(-1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_Post_ReturnsRedirectToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();
            context.Benefits.RemoveRange(context.Benefits); // Clear previous records
            await context.SaveChangesAsync();

            var benefit = new Benefit { Name = "Health Insurance", Description = "Medical coverage", Cost = 200 };

            // Act
            var result = await controller.Create(benefit);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(context.Benefits); // Check only one entry exists
        }


        [Fact]
        public async Task Create_Post_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange
            var (controller, context) = CreateController();
            context.Benefits.RemoveRange(context.Benefits); // Clear previous records
            await context.SaveChangesAsync(); // Ensure the context is updated
            var benefit = new Benefit { Name = null, Description = "Medical coverage", Cost = 200 }; // Invalid because Name is required

            // Act
            controller.ModelState.AddModelError("Name", "The Name field is required."); // Simulate an invalid state
            var result = await controller.Create(benefit);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(benefit, viewResult.Model); // Check that the model returned is the same as what was passed in
            Assert.Empty(context.Benefits); // Ensure no benefits were added to the database
        }



        [Fact]
        public async Task Edit_Get_ReturnsViewWithBenefit_WhenIdIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();
            var benefit = new Benefit { Name = "Health Insurance", Description = "Medical coverage", Cost = 200 };
            await context.Benefits.AddAsync(benefit);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Edit(benefit.BenefitID);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Benefit>(viewResult.Model);
            Assert.Equal(benefit.BenefitID, model.BenefitID);
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFound_WhenIdIsInvalid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Act
            var result = await controller.Edit(-1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_ReturnsRedirectToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();
            context.Benefits.RemoveRange(context.Benefits); // Clear previous records
            await context.SaveChangesAsync(); // Ensure the context is updated
            var benefit = new Benefit { Name = "Health Insurance", Description = "Medical coverage", Cost = 200 };
            await context.Benefits.AddAsync(benefit);
            await context.SaveChangesAsync();

            benefit.Name = "Updated Benefit";

            // Act
            var result = await controller.Edit(benefit.BenefitID, benefit);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Updated Benefit", context.Benefits.First().Name);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesBenefitAndRedirects()
        {
            // Arrange
            var (controller, context) = CreateController();
            context.Benefits.RemoveRange(context.Benefits); // Clear previous records
            await context.SaveChangesAsync(); // Ensure the context is updated
            var benefit = new Benefit { Name = "Health Insurance", Description = "Medical coverage", Cost = 200 };
            await context.Benefits.AddAsync(benefit);
            await context.SaveChangesAsync(); // Make sure the benefit is saved before deletion

            // Act
            var result = await controller.DeleteConfirmed(benefit.BenefitID);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            // Refresh the context to get the current state
            await context.SaveChangesAsync(); // Refresh context
            Assert.Empty(context.Benefits); // Ensure no benefits were added to the database
        }


        [Fact]
        public async Task Delete_ReturnsNotFound_WhenIdIsInvalid()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Act
            var result = await controller.Delete(-1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
