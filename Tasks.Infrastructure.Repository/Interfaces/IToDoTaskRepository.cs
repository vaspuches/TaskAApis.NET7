using Tasks.Infrastructure.Data.Model;

namespace Tasks.Infrastructure.Repository.Interfaces
{
    public interface IToDoTaskRepository {
        public Task<IEnumerable<ToDoTask>> GetByDueDateAsync(DateTime dueDate);
        public Task<IEnumerable<ToDoTask>> GetByStatusAndDates(ToDoTaskStatus status, DateTime? startDate, DateTime? endDate, int offset, int limit);
        public Task<bool> DeleteAsync(int id);
        public Task<int> DeleteByStatusAndDates(ToDoTaskStatus status, DateTime? startDate, DateTime? endDate, int offset, int limit);
        public Task<IEnumerable<ToDoTask>> GetAllAsync(int offset, int limit);
        public Task<ToDoTask?> GetByIdAsync(int id);
        public Task<ToDoTask> InsertAsync(ToDoTask toDoTask);
        public Task<ToDoTask?> Update(ToDoTask toDoTask);
    }
}