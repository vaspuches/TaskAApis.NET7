using Tasks.Domain.Entities;
using Tasks.Infrastructure.Data.Model;

namespace Tasks.Application.Services.Interfaces
{
    public interface IToDoTaskService
    {
        public Task<ToDoTaskEntity> GetByIdAsync(int id);
        public Task<IEnumerable<ToDoTaskEntity>> GetAllAsync(int offset, int limit);
        public Task<IEnumerable<ToDoTaskEntity>> GetByStatusAndDates(ToDoTaskStatus status, DateTime? startDate, DateTime? endDate, int offset, int limit);
        public Task<ToDoTaskEntity> UpdateAsync(ToDoTaskEntity entity);
        public Task<ToDoTaskEntity> InsertAsync(ToDoTaskEntity entity);
        public Task<bool> DeleteAsync(int id);
        public Task<int> DeleteByStatusAndDates(ToDoTaskStatus status, DateTime? startDate, DateTime? endDate, int offset, int limit);
    }
}