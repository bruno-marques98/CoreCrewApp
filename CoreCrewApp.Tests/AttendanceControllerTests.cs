using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCrewApp.Controllers;
using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CoreCrewApp.Tests.Controllers
{
    public class AttendanceControllerTests
    {
        private (AttendanceController Controller, AppDbContext Context) CreateController()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase") // Fixed name to persist across test methods
                .Options;

            var context = new AppDbContext(options);
            var controller = new AttendanceController(context);
            return (controller, context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithAttendances()
        {
            // Arrange
            var (controller, context) = CreateController();

            // Adding employee to relate to attendance
            var employee = new Employee { EmployeeID = 1, DepartmentID = 1, FirstName = "John", LastName = "Doe", Email = "test@test.com", HireDate = new DateTime(1985, 1, 1) };
            await context.Employees.AddAsync(employee);

            var attendances = new List<Attendance>
            {
                new Attendance { AttendanceId = 1, Employee = employee, AttendanceStatus = AttendanceStatus.Present,CheckInTime = new DateTime(2024, 10, 10,10,0,0),CheckOutTime = new DateTime(2024, 10, 10,11,0,0),CreatedAt=DateTime.Now,UpdatedAt=DateTime.Now },
                new Attendance { AttendanceId = 2, Employee = employee, AttendanceStatus = AttendanceStatus.Absent,CheckInTime = new DateTime(2024, 10, 11,10,0,0),CheckOutTime = new DateTime(2024, 10, 11,11,0,0),CreatedAt=DateTime.Now,UpdatedAt=DateTime.Now  }
            };

            await context.Attendances.AddRangeAsync(attendances);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Attendance>>(viewResult.Model);

            // Verify the count of attendances
            Assert.Equal(2, model.Count); // Should be 2 now
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
        public async Task Details_ReturnsNotFound_WhenAttendanceDoesNotExist()
        {
            // Arrange
            var (controller, _) = CreateController();

            // Act
            var result = await controller.Details(1); // ID that does not exist

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsRedirect_WhenModelIsValid()
        {
            // Arrange
            var (controller, context) = CreateController();
            var attendance = new Attendance { AttendanceId = 3, AttendanceStatus = AttendanceStatus.Present }; // Changed ID to avoid conflicts

            // Act
            var result = await controller.Create(attendance);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(context.Attendances); // Verify one attendance was added
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesAttendanceAndRedirects()
        {
            // Arrange
            var (controller, context) = CreateController();

            var attendance = new Attendance { AttendanceId = 1 };
            await context.Attendances.AddAsync(attendance);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteConfirmed(attendance.AttendanceId);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            // Reload context to confirm deletion
            var deletedAttendance = await context.Attendances.FindAsync(attendance.AttendanceId);
            Assert.Null(deletedAttendance); // Assert attendance was deleted
        }

    }
}
