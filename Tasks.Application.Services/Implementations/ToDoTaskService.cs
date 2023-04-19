using AutoMapper;
using Tasks.Application.Services.Interfaces;
using Tasks.Domain.Entities;
using Tasks.Infrastructure.Data.Model;
using Tasks.Infrastructure.Repository.Interfaces;
using ToDoTaskStatus = Tasks.Infrastructure.Data.Model.ToDoTaskStatus;

namespace Tasks.Application.Services.Implementations
{
    public class ToDoTaskService : IToDoTaskService
    {
        private readonly IToDoTaskRepository _toDoTaskRepository;
        private readonly IMapper _mapper;

        public ToDoTaskService(IToDoTaskRepository toDoTaskRepository, IMapper mapper)
        {
            _toDoTaskRepository = toDoTaskRepository;
            _mapper = mapper;         
        }

        public async Task<IEnumerable<ToDoTaskEntity>> GetAllAsync(int offset, int limit)
        {
            var toDoTasks = await _toDoTaskRepository.GetAllAsync(offset, limit);
            
            return _mapper.Map<IEnumerable<ToDoTaskEntity>>(toDoTasks);           
        }

        public async Task<ToDoTaskEntity> GetByIdAsync(int id)
        {
            var toDoTask = await _toDoTaskRepository.GetByIdAsync(id);
            
            return _mapper.Map<ToDoTaskEntity>(toDoTask);
        }

        public async Task<ToDoTaskEntity> InsertAsync(ToDoTaskEntity entity)
        {
            ValidateStatus(entity);

            var toDoTask = await _toDoTaskRepository.InsertAsync(
                _mapper.Map<ToDoTask>(entity));

            return _mapper.Map<ToDoTaskEntity>(toDoTask);
        }        

        public async Task<ToDoTaskEntity> UpdateAsync(ToDoTaskEntity entity)
        {
            ValidateStatus(entity);

            var toDoTask = await _toDoTaskRepository.Update(_mapper.Map<ToDoTask>(entity));
            
            return _mapper.Map<ToDoTaskEntity>(toDoTask);            
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var toDoTask = await _toDoTaskRepository.GetByIdAsync(id);

            return toDoTask != null && await _toDoTaskRepository.DeleteAsync(id);           
        }

        public static void ValidateStatus(ToDoTaskEntity? entity)
        {                        
            if (entity == null || entity.Status == null || !Enum.IsDefined(typeof(ToDoTaskStatus), entity.Status))
            {
                throw new ArgumentException("StatusTask value NOT correct");
            }
        }        

        public async Task<IEnumerable<ToDoTaskEntity>> GetByStatusAndDates(ToDoTaskStatus status, DateTime? startDate, DateTime? endDate, int offset, int limit)
        {
            var toDoTasks = await _toDoTaskRepository.GetByStatusAndDates(status, startDate, endDate, offset, limit);

            return _mapper.Map<IEnumerable<ToDoTaskEntity>>(toDoTasks);
        }

        public async Task<int> DeleteByStatusAndDates(ToDoTaskStatus status, DateTime? startDate, DateTime? endDate, int offset, int limit)
        {
            int affectedRows = await _toDoTaskRepository.DeleteByStatusAndDates(status, startDate, endDate, offset, limit);

            return affectedRows;
        }
    }
}