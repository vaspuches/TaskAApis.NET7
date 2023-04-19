using Microsoft.EntityFrameworkCore;
using Tasks.Infrastructure.Data.Model;
using Tasks.Infrastructure.Persistence;
using Tasks.Infrastructure.Repository.Interfaces;

namespace Tasks.Infrastructure.Repository.Implementations
{
    public class ToDoTaskRepository : IToDoTaskRepository
    {
        public readonly ToDoTaskDbContext toDoTaskContext;

        public ToDoTaskRepository(ToDoTaskDbContext toDoTaskContext)
        {
            this.toDoTaskContext = toDoTaskContext;            
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await toDoTaskContext.ToDoTasks.FirstOrDefaultAsync(x => x.Id == id);
            if (task != null)
            {
                toDoTaskContext.Remove(task);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<ToDoTask>> GetAllAsync(int offset = 0, int limit = 100)
        {
            var result = await toDoTaskContext.ToDoTasks.Skip(offset).Take(limit).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<ToDoTask>> GetByDueDateAsync(DateTime dueDate)
        {
            return await toDoTaskContext.ToDoTasks.Where(x => x.DueDate == dueDate).ToListAsync();
        }

        public async Task<ToDoTask?> GetByIdAsync(int id)
        {
            var task = await toDoTaskContext.ToDoTasks.FirstOrDefaultAsync(x => x.Id == id);

            return task ?? null;
        }      
        
        public async Task<IEnumerable<ToDoTask>> GetByStatusAndDates(ToDoTaskStatus status, DateTime? startDate, DateTime? endDate, int offset = 0, int limit = 100)
        {           
            if(startDate != null && endDate != null)
            {
                return await toDoTaskContext.ToDoTasks.Where(
                    x => x.Status == status && x.DueDate >= startDate && x.DueDate <= endDate).Skip(offset).Take(limit).ToListAsync();
            }
            if(startDate != null) {
                return await toDoTaskContext.ToDoTasks
                    .Where(x => x.Status == status && x.DueDate >= startDate).Skip(offset).Take(limit).ToListAsync();
            }
            if (endDate != null)
            {
                return await toDoTaskContext.ToDoTasks
                    .Where(x => x.Status == status && x.DueDate <= endDate).Skip(offset).Take(limit).ToListAsync();
            }

            return await toDoTaskContext.ToDoTasks
                .Where(x => x.Status == status).Skip(offset).Take(limit).ToListAsync();
        }

        public async Task<int> DeleteByStatusAndDates(ToDoTaskStatus status, DateTime? startDate, DateTime? endDate, int offset = 0, int limit = 100)
        {
            IQueryable<ToDoTask> tasksToDelete;

            if (startDate != null && endDate != null)
            {
                tasksToDelete = toDoTaskContext.ToDoTasks.Where(
                    x => x.Status == status && x.DueDate >= startDate && x.DueDate <= endDate);
            }
            else if (startDate != null)
            {
                tasksToDelete = toDoTaskContext.ToDoTasks
                    .Where(x => x.Status == status && x.DueDate >= startDate);
            }
            else if (endDate != null)
            {
                tasksToDelete = toDoTaskContext.ToDoTasks
                    .Where(x => x.Status == status && x.DueDate <= endDate);
            }
            else
            {
                tasksToDelete = toDoTaskContext.ToDoTasks
                    .Where(x => x.Status == status);
            }

            tasksToDelete = tasksToDelete.Skip(offset).Take(limit);
            toDoTaskContext.ToDoTasks.RemoveRange(tasksToDelete);

            int affectedRows = await toDoTaskContext.SaveChangesAsync();

            return affectedRows;
        }

        public async Task<ToDoTask> InsertAsync(ToDoTask toDoTask)
        {
            await toDoTaskContext.ToDoTasks.AddAsync(toDoTask);
            await toDoTaskContext.SaveChangesAsync();
            
            return toDoTask;
        }

        public async Task<ToDoTask?> Update(ToDoTask toDoTask)
        {
            var existingTask = await toDoTaskContext.ToDoTasks.FirstOrDefaultAsync(t => t.Id == toDoTask.Id);
            
            if (existingTask == null)
            {
                return null;
            }

            toDoTaskContext.Entry(existingTask).CurrentValues.SetValues(toDoTask);
            toDoTaskContext.SaveChanges();

            return existingTask;
        }        
    }
}