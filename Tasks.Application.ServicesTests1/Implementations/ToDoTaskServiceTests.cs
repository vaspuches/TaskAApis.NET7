using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tasks.Application.Services.Implementations;
using Tasks.Application.Services.Interfaces;
using Tasks.Domain.Entities;
using Tasks.Infrastructure.Data.Model;
using Tasks.Infrastructure.Repository.Interfaces;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using ToDoTaskStatus = Tasks.Infrastructure.Data.Model.ToDoTaskStatus;

namespace Tasks.Application.Services.Tests
{
    [TestClass]
    public class ToDoTaskServiceTests
    {
               
        private readonly Mock<IToDoTaskRepository> _toDoTaskRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IToDoTaskService _toDoTaskService;

        public ToDoTaskServiceTests()
        {
            _toDoTaskRepositoryMock = new Mock<IToDoTaskRepository>();
            _mapperMock = new Mock<IMapper>();
            _toDoTaskService = new ToDoTaskService(_toDoTaskRepositoryMock.Object, _mapperMock.Object);
        }        

        [TestMethod]
        public async Task GetAllAsync_ReturnsMappedToDoTasks()
        {            
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

            var result = await _toDoTaskService.GetAllAsync(0, 10);

            Assert.AreEqual(toDoTaskEntities, result);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsMappedToDoTask()
        {

            var id = 1;
            var toDoTask = new ToDoTask { Id = id, Description = "Task 1", Status = ToDoTaskStatus.NotStarted };
            _toDoTaskRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(toDoTask);

            var toDoTaskEntity = new ToDoTaskEntity { Id = id, Description = "Task 1", Status = "NotStarted" };
            _mapperMock.Setup(mapper => mapper.Map<ToDoTaskEntity>(toDoTask)).Returns(toDoTaskEntity);


            var result = await _toDoTaskService.GetByIdAsync(id);

            Assert.AreEqual(toDoTaskEntity, result);
        }

        [TestMethod]
        public async Task InsertAsync_InsertsMappedToDoTask()
        {

            var toDoTaskEntity = new ToDoTaskEntity { Description = "Task 1", Status = "NotStarted" };
            var toDoTask = new ToDoTask();
            _mapperMock.Setup(mapper => mapper.Map<ToDoTask>(toDoTaskEntity)).Returns(toDoTask);

            _toDoTaskRepositoryMock.Setup(repo => repo.InsertAsync(toDoTask)).ReturnsAsync(toDoTask);

            var insertedToDoTaskEntity = new ToDoTaskEntity { Id = 1, Description = "Task 1", Status = "NotStarted" };
            _mapperMock.Setup(mapper => mapper.Map<ToDoTaskEntity>(toDoTask)).Returns(insertedToDoTaskEntity);

            var result = await _toDoTaskService.InsertAsync(toDoTaskEntity);

            Assert.AreEqual(insertedToDoTaskEntity, result);
            _mapperMock.Verify(mapper => mapper.Map<ToDoTask>(toDoTaskEntity), Times.Once);
            _toDoTaskRepositoryMock.Verify(repo => repo.InsertAsync(toDoTask), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<ToDoTaskEntity>(toDoTask), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_UpdatesMappedToDoTask()
        {
            var toDoTaskEntity = new ToDoTaskEntity { Id = 1, Description = "Task 1", Status = "NotStarted" };
            var toDoTask = new ToDoTask();
            _mapperMock.Setup(mapper => mapper.Map<ToDoTask>(toDoTaskEntity)).Returns(toDoTask);

            _toDoTaskRepositoryMock.Setup(repo => repo.Update(toDoTask)).ReturnsAsync(toDoTask);

            var updatedToDoTaskEntity = new ToDoTaskEntity { Id = 1, Description = "Task 1", Status = "NotStarted" };
            _mapperMock.Setup(mapper => mapper.Map<ToDoTaskEntity>(toDoTask)).Returns(updatedToDoTaskEntity);

            var result = await _toDoTaskService.UpdateAsync(toDoTaskEntity);

            Assert.AreEqual(updatedToDoTaskEntity, result);
            _mapperMock.Verify(mapper => mapper.Map<ToDoTask>(toDoTaskEntity), Times.Once);
            _toDoTaskRepositoryMock.Verify(repo => repo.Update(toDoTask), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<ToDoTaskEntity>(toDoTask), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_InvalidToDoTaskEntity_ThrowsArgumentException()
        {

            var toDoTaskEntity = new ToDoTaskEntity { Id = 1, Description = "Task 1", Status = "Invalid" };
            
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _toDoTaskService.UpdateAsync(toDoTaskEntity));
        }      

        [TestMethod]
        public async Task DeleteAsync_DeletesExistingToDoTask()
        {

            var id = 1;
            var toDoTask = new ToDoTask { Id = id, Description = "Task 1", Status = ToDoTaskStatus.NotStarted };
            _toDoTaskRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(toDoTask);
            _toDoTaskRepositoryMock.Setup(repo => repo.DeleteAsync(id)).ReturnsAsync(true);

            var result = await _toDoTaskService.DeleteAsync(id);

            Assert.IsTrue(result);
            _toDoTaskRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
            _toDoTaskRepositoryMock.Verify(repo => repo.DeleteAsync(id), Times.Once);
        }

        [TestMethod]
        public void ValidateStatus_NullEntity_ThrowsArgumentException()
        {

            ToDoTaskEntity? entity = null;

            Assert.ThrowsException<ArgumentException>(() => ToDoTaskService.ValidateStatus(entity));
        }

        [TestMethod]
        public void ValidateStatus_NullStatus_ThrowsArgumentException()
        {

            var entity = new ToDoTaskEntity { Id = 1, Description = "Task 1", Status = null };

            Assert.ThrowsException<ArgumentException>(() => ToDoTaskService.ValidateStatus(entity));
        }

        [TestMethod]
        public void ValidateStatus_InvalidStatus_ThrowsArgumentException()
        {

            var entity = new ToDoTaskEntity { Id = 1, Description = "Task 1", Status = "InvalidStatus" };

            Assert.ThrowsException<ArgumentException>(() => ToDoTaskService.ValidateStatus(entity));
        }        

        [TestMethod]
        public async Task GetByStatusAndDates_ReturnsMappedToDoTasks()
        {

            var status = ToDoTaskStatus.InProgress;
            var startDate = DateTime.Now.AddDays(-7);
            var endDate = DateTime.Now;
            var toDoTasks = new List<ToDoTask>
            {
                new ToDoTask { Id = 1, Description = "Task 1", Status = status, DueDate = startDate.AddDays(-1) },
                new ToDoTask { Id = 2, Description = "Task 2", Status = status, DueDate = endDate.AddDays(1) }
            };
            _toDoTaskRepositoryMock.Setup(repo => repo.GetByStatusAndDates(status, startDate, endDate, It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(toDoTasks);

            var toDoTaskEntities = new List<ToDoTaskEntity>
            {
                new ToDoTaskEntity { Id = 1, Description = "Task 1", Status = "InProgress", DueDate = startDate.AddDays(-1) },
                new ToDoTaskEntity { Id = 2, Description = "Task 2", Status = "InProgress", DueDate = endDate.AddDays(1) }
            };
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<ToDoTaskEntity>>(toDoTasks)).Returns(toDoTaskEntities);

            var result = await _toDoTaskService.GetByStatusAndDates(status, startDate, endDate, 0, 10);

            Assert.AreEqual(toDoTaskEntities, result);
        }

        [TestMethod]
        public async Task DeleteByStatusAndDates_DeletesMatchingToDoTasks()
        {

            var status = ToDoTaskStatus.InProgress;
            var startDate = DateTime.Now.AddDays(-7);
            var endDate = DateTime.Now;
            var affectedRows = 2;
            _toDoTaskRepositoryMock.Setup(repo => repo.DeleteByStatusAndDates(status, startDate, endDate, It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(affectedRows);

            var result = await _toDoTaskService.DeleteByStatusAndDates(status, startDate, endDate, 0, 10);

            Assert.AreEqual(affectedRows, result);
            _toDoTaskRepositoryMock.Verify(repo => repo.DeleteByStatusAndDates(status, startDate, endDate, It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
    }
}