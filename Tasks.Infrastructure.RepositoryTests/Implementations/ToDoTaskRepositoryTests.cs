using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Tasks.Infrastructure.Persistence;
using Tasks.Infrastructure.Data.Model;
using Microsoft.Data.Sqlite;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Tasks.Infrastructure.Repository.Implementations.Tests
{
    [TestClass()]
    public class ToDoTaskRepositoryTests
    {
        private readonly DbContextOptions<ToDoTaskDbContext> _options;
        private readonly SqliteConnection _connection;

        public ToDoTaskRepositoryTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<ToDoTaskDbContext>()
                .UseSqlite("Data Source=ToDoTaskDbTest.db;")
                .Options;

            using var context = new ToDoTaskDbContext(_options);
            context.Database.EnsureCreated();
            context.Database.EnsureDeleted();
        }        

        [TestMethod]
        public async Task Update_ExistingTask_ReturnsUpdatedTask()
        {
            using (var context = new ToDoTaskDbContext(_options))
            {
                context.ToDoTasks.RemoveRange(context.ToDoTasks);
                var task = new ToDoTask { Id = 1, Description = "Task 1", Status = ToDoTaskStatus.NotStarted };
                context.ToDoTasks.Add(task);
                await context.SaveChangesAsync();
            }

            using (var context = new ToDoTaskDbContext(_options))
            {
                var taskToUpdate = new ToDoTask { Id = 1, Description = "Updated task 1", Status = ToDoTaskStatus.NotStarted };
                var sut = new ToDoTaskRepository(context);

                var result = await sut.Update(taskToUpdate);

                Assert.IsNotNull(result);
                Assert.AreEqual(taskToUpdate.Description, result.Description);
            }
        }

        [TestMethod]
        public async Task Update_NonExistingTask_ReturnsNull()
        {         
            using (var context = new ToDoTaskDbContext(_options))
            {
                context.ToDoTasks.RemoveRange(context.ToDoTasks);
                await context.SaveChangesAsync();
            }

            using (var context = new ToDoTaskDbContext(_options))
            {
                var taskToUpdate = new ToDoTask { Id = 1, Description = "Updated task 1", Status = ToDoTaskStatus.NotStarted };
                var sut = new ToDoTaskRepository(context);

                var result = await sut.Update(taskToUpdate);

                Assert.IsNull(result);
            }
        }
    }
}