using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CoreCrewApp.Controllers;
using CoreCrewApp.Models;
using CoreCrewApp.Data;

namespace CoreCrewApp.Tests.Controllers
{
    public class DepartmentControllerTests : IAsyncLifetime
    {
        private readonly AppDbContext _context;
        private readonly DepartmentController _controller;

        public DepartmentControllerTests()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _controller = new DepartmentController(_context);
        }

        public async Task InitializeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();

            // Clear any existing departments for a fresh start
            _context.Departments.RemoveRange(_context.Departments);
            await _context.SaveChangesAsync();
        }


        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        [Fact]
        public async Task Index_ReturnsViewWithDepartmentList()
        {
            // Arrange
            _context.Departments.Add(new Department { DepartmentId = 1, DepartmentName = "HR" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            var model = Assert.IsAssignableFrom<IEnumerable<Department>>(result?.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewWithDepartment_WhenIdIsValid()
        {
            // Arrange
            var department = new Department { DepartmentId = 1, DepartmentName = "IT" };
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Details(1) as ViewResult;

            // Assert
            var model = Assert.IsType<Department>(result?.Model);
            Assert.Equal("IT", model.DepartmentName);
        }

        [Fact]
        public async Task Create_Post_ReturnsRedirectToIndex_WhenModelIsValid()
        {
            // Arrange
            var department = new Department { DepartmentName = "Finance" };

            // Act
            var result = await _controller.Create(department);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(1, _context.Departments.Count());
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = await _controller.Edit(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_ReturnsRedirectToIndex_WhenModelIsValid()
        {
            // Arrange
            var department = new Department { DepartmentId = 1, DepartmentName = "Operations" };
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            // Act
            department.DepartmentName = "Updated Operations";
            var result = await _controller.Edit(1, department);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Updated Operations", _context.Departments.First().DepartmentName);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = await _controller.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesDepartment_WhenIdIsValid()
        {
            // Arrange
            var department = new Department { DepartmentId = 1, DepartmentName = "Marketing" };
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", ((RedirectToActionResult)result).ActionName);
            Assert.Empty(_context.Departments);
        }
    }
}
