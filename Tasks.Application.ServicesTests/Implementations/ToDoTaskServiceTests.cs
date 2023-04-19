
using AutoMapper;
using Moq;
using NUnit.Framework;
using Tasks.Application.Services.Implementations;
using Tasks.Application.Services.Interfaces;
using Tasks.Domain.Entities;
using Tasks.Infrastructure.Data.Model;
using Tasks.Infrastructure.Repository.Interfaces;
using ToDoTaskStatus = Tasks.Infrastructure.Data.Model.ToDoTaskStatus;

namespace Tasks.Application.Services.Tests
{
    [TestFixture]
    public class ToDoTaskServiceTests
    {
        private Mock<IToDoTaskRepository>? _toDoTaskRepositoryMock;
        private Mock<IMapper>? _mapperMock;
        private IToDoTaskService? _toDoTaskService;

        [SetUp]
        public void SetUp()
        {
            _toDoTaskRepositoryMock = new Mock<IToDoTaskRepository>();
            _mapperMock = new Mock<IMapper>();
            _toDoTaskService = new ToDoTaskService(_toDoTaskRepositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetAllAsync_ReturnsMappedToDoTasks()
        {
            // Arrange
            var toDoTasks = new List<ToDoTask>
            {
                new ToDoTask { Id = 1, Description = "Task 1", Status = ToDoTaskStatus.NotStarted },
                new ToDoTask { Id = 2, Description = "Task 2", Status = ToDoTaskStatus.InProgress }
            };
            _toDoTaskRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(toDoTasks);

            var toDoTaskEntities = new List<ToDoTaskEntity>
            {
                new ToDoTaskEntity { Id = 1, Description = "Task 1", Status = "NotStarted" },
                new ToDoTaskEntity { Id = 2, Description = "Task 2", Status = "InProgress" }
            };
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<ToDoTaskEntity>>(toDoTasks)).Returns(toDoTaskEntities);

            // Act
            var result = await _toDoTaskService.GetAllAsync(0, 10);

            // Assert
            Assert.AreEqual(toDoTaskEntities, result);
        }       
    }
}