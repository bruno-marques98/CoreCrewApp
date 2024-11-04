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
    public class EmployeeTrainingControllerTests
    {
        private (EmployeeTrainingController Controller, AppDbContext Context) CreateController()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
                .Options;

            var context = new AppDbContext(options);
            var controller = new EmployeeTrainingController(context);

            // Clear tables at the start of each test
            context.EmployeeTrainings.RemoveRange(context.EmployeeTrainings);
            context.Employees.RemoveRange(context.Employees);
            context.TrainingPrograms.RemoveRange(context.TrainingPrograms);
            context.SaveChanges();

            return (controller, context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithEmployeeTrainings()
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
            var trainingProgram = new TrainingProgram { TrainingProgramID = 1, ProgramName = "Safety Training", Description = "Safety Training", StartDate = DateTime.Now, TrainerID = 1 };
            var employeeTraining = new EmployeeTraining { EmployeeID = 1, TrainingProgramID = 1, EnrollmentDate = DateTime.Now };

            context.Employees.Add(employee);
            context.TrainingPrograms.Add(trainingProgram);
            context.EmployeeTrainings.Add(employeeTraining);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<EmployeeTraining>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ReturnsViewWithEmployeeTraining_WhenExists()
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
            var trainingProgram = new TrainingProgram { TrainingProgramID = 1, ProgramName = "Safety Training", Description = "Safety Training", StartDate = DateTime.Now, TrainerID = 1 };
            var employeeTraining = new EmployeeTraining { EmployeeID = 1, TrainingProgramID = 1, EnrollmentDate = DateTime.Now };

            context.Employees.Add(employee);
            context.TrainingPrograms.Add(trainingProgram);
            context.EmployeeTrainings.Add(employeeTraining);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Details(1, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<EmployeeTraining>(viewResult.Model);
            Assert.Equal(1, model.EmployeeID);
            Assert.Equal(1, model.TrainingProgramID);
        }

        [Fact]
        public async Task Create_AddsEmployeeTrainingAndRedirects_WhenModelIsValid()
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
            var trainingProgram = new TrainingProgram { TrainingProgramID = 1, ProgramName = "Safety Training" , Description = "Safety Training",StartDate = DateTime.Now,TrainerID = 1};
            context.Employees.Add(employee);
            context.TrainingPrograms.Add(trainingProgram);
            await context.SaveChangesAsync();

            // Create EmployeeTraining
            var employeeTraining = new EmployeeTraining
            {
                EmployeeID = 1,
                TrainingProgramID = 1,
                EnrollmentDate = DateTime.Now
            };

            // Act
            var result = await controller.Create(employeeTraining);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(context.EmployeeTrainings);
        }

        [Fact]
        public async Task Edit_UpdatesEmployeeTrainingAndRedirects_WhenModelIsValid()
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
            var trainingProgram1 = new TrainingProgram { TrainingProgramID = 1, ProgramName = "Safety Training", Description = "Safety Training", StartDate = DateTime.Now, TrainerID = 1 };
            var trainingProgram2 = new TrainingProgram { TrainingProgramID = 2, ProgramName = "Advanced Safety Training", Description = "Advanced Safety Training", StartDate = DateTime.Now, TrainerID = 1 };

            context.Employees.Add(employee);
            context.TrainingPrograms.AddRange(trainingProgram1, trainingProgram2);
            var employeeTraining = new EmployeeTraining { EmployeeID = 1, TrainingProgramID = 1, EnrollmentDate = DateTime.Now };
            context.EmployeeTrainings.Add(employeeTraining);
            await context.SaveChangesAsync();

            // Update to a new program
            DateTime completitionDate = DateTime.Now;
            employeeTraining.CompletionDate = completitionDate;

            // Act
            var result = await controller.Edit(1, 1, employeeTraining);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal(completitionDate, context.EmployeeTrainings.First().CompletionDate);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesEmployeeTrainingAndRedirects()
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
            var trainingProgram = new TrainingProgram { TrainingProgramID = 1, ProgramName = "Safety Training", Description = "Safety Training", StartDate = DateTime.Now, TrainerID = 1 };
            var employeeTraining = new EmployeeTraining { EmployeeID = 1, TrainingProgramID = 1, EnrollmentDate = DateTime.Now };

            context.Employees.Add(employee);
            context.TrainingPrograms.Add(trainingProgram);
            context.EmployeeTrainings.Add(employeeTraining);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteConfirmed(1, 1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(await context.EmployeeTrainings.FindAsync(1, 1));
        }
    }
}
