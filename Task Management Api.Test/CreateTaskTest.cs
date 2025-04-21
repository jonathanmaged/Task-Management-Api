using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Task_Management_Api.AutoMapper;
using Task_Management_Api.Data;
using Task_Management_Api.Dtos;
using Task_Management_Api.Services;
using Task_Management_Api.Shared;

namespace Task_Management_Api.Test
{
    public class CreateTaskTest
    {
        private readonly IMapper _mapper;

        public CreateTaskTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProfileMapping>();
            });
            _mapper = config.CreateMapper();
        }

        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique DB per test
                .Options;

            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        [Fact]
        public async Task CreateTaskAsync_ValidTaskDto_CreatesTaskAndComments()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new TaskService(context, _mapper);

            var dto = new TaskDto
            {
                Name = "Test Task",
                Description = "This is a test task.",
                UsersId = new List<int> { 1, 2 }
            };

            // Act
            var result = await service.CreateTaskAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(State.Success, result.State);

            var savedTask = context.Tasks.Include(t => t.TaskComments).FirstOrDefault();
            Assert.NotNull(savedTask);
            Assert.Equal("Test Task", savedTask.Name);
            Assert.Equal(2, savedTask.TaskComments.Count); // 2 users linked
        }

        [Fact]
        public async Task CreateTaskAsync_InvalidUserId_ReturnsNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new TaskService(context, _mapper);

            var dto = new TaskDto
            {
                Name = "Invalid User Task",
                Description = "Should fail",
                UsersId = new List<int> { 99 } // Invalid user ID
            };

            // Act
            var result = await service.CreateTaskAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(State.NotFound, result.State);
            Assert.Equal("user with id 99 doesnt exist", result.Message);
            Assert.Empty(context.Tasks); // Should not be added
        }
    }
}
