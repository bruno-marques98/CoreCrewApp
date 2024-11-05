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
    public class TrainingProgramControllerTests
    {
        private (TrainingProgramController Controller, AppDbContext Context) CreateController()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);

            // Seed with initial data
            var employee = new Employee { EmployeeID = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com" };
            context.Employees.Add(employee);
            context.SaveChanges();

            var controller = new TrainingProgramController(context);
            return (controller, context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithTrainingPrograms()
        {
            // Arrange
            var (controller, context) = CreateController();

            var trainingProgram = new TrainingProgram
            {
                TrainingProgramID = 1,
                ProgramName = "Program1",
                Description = "Description1",
                StartDate = DateTime.Now,
                TrainerID = 1
            };
            context.TrainingPrograms.Add(trainingProgram);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<TrainingProgram>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ReturnsViewWithTrainingProgram_WhenTrainingProgramExists()
        {
            // Arrange
            var (controller, context) = CreateController();

            var trainingProgram = new TrainingProgram
            {
                TrainingProgramID = 1,
                ProgramName = "Program1",
                Description = "Description1",
                StartDate = DateTime.Now,
                TrainerID = 1
            };
            context.TrainingPrograms.Add(trainingProgram);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<TrainingProgram>(viewResult.Model);
            Assert.Equal(1, model.TrainingProgramID);
        }

        [Fact]
        public async Task Create_AddsTrainingProgramAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            var trainingProgram = new TrainingProgram
            {
                TrainingProgramID = 1,
                ProgramName = "NewProgram",
                Description = "NewDescription",
                StartDate = DateTime.Now,
                TrainerID = 1
            };

            // Act
            var result = await controller.Create(trainingProgram);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(context.TrainingPrograms);
        }

        [Fact]
        public async Task Edit_UpdatesTrainingProgramAndRedirects_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();

            var trainingProgram = new TrainingProgram
            {
                TrainingProgramID = 1,
                ProgramName = "OldProgram",
                Description = "OldDescription",
                StartDate = DateTime.Now,
                TrainerID = 1
            };
            context.TrainingPrograms.Add(trainingProgram);
            await context.SaveChangesAsync();

            // Modify the program details
            trainingProgram.ProgramName = "UpdatedProgram";

            // Act
            var result = await controller.Edit(1, trainingProgram);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("UpdatedProgram", context.TrainingPrograms.First().ProgramName);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesTrainingProgramAndRedirects()
        {
            // Arrange
            var (controller, context) = CreateController();

            var trainingProgram = new TrainingProgram
            {
                TrainingProgramID = 1,
                ProgramName = "DeleteProgram",
                Description = "DeleteDescription",
                StartDate = DateTime.Now,
                TrainerID = 1
            };
            context.TrainingPrograms.Add(trainingProgram);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(await context.TrainingPrograms.FindAsync(1));
        }
    }
}
