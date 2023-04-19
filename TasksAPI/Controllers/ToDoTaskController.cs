using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tasks.Application.Services.Interfaces;
using Tasks.Domain.Entities;
using Tasks.Infrastructure.Data.Model;
using Tasks.Web.Dto;

namespace Tasks.Web.Api.Controllers
{
    [Route("/")]
    [ApiController]
    public class ToDoTaskController : ControllerBase
    {
        private readonly IToDoTaskService _iToDoTaskService;
        private readonly IMapper _mapper;

        public ToDoTaskController(IToDoTaskService iToDoTaskService, IMapper mapper)
        {
            _iToDoTaskService = iToDoTaskService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets ToDoTask related to the given id
        /// </summary>       
        [HttpGet("api/todotask/Get All Tasks")]
        [ProducesResponseType(typeof(IEnumerable<ToDoTaskDto>), StatusCodes.Status200OK)]        
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        public async Task<IActionResult> GetAllToDoTasks(int offset = 0, int limit = 100)
        {
            var result = _mapper.Map<List<ToDoTaskDto>>(await _iToDoTaskService.GetAllAsync(offset, limit));
            
            return Ok(result);
        }

        /// <summary>
        /// Gets ToDoTask related to the given id
        /// </summary>
        /// <param name="id">The id of the requested ToDoTask</param>
        [HttpGet("api/todotask/{id}")]
        [ProducesResponseType(typeof(ToDoTaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]       
        public async Task<IActionResult> GetToDoTaskById(int id)
        {
            var result = _mapper.Map<ToDoTaskDto>(await _iToDoTaskService.GetByIdAsync(id));
            
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of ToDoTasks with the specified status and optional date range.
        /// </summary>
        /// <param name="status">The status of the ToDoTasks to be retrieved. Should match a valid ToDoTaskStatus enumeration value.</param>
        /// <param name="startDate">Optional. The start date of the date range for tasks to be retrieved. If not specified, the default is the minimum DateTime value.</param>
        /// <param name="endDate">Optional. The end date of the date range for tasks to be retrieved. If not specified, the default is the maximum DateTime value.</param>
        /// <param name="offset">Optional. The number of tasks to skip before starting to retrieve. Default value is 0.</param>
        /// <param name="limit">Optional. The maximum number of tasks to retrieve. Default value is 100.</param>
        /// <returns>
        /// A Task representing an IActionResult.
        /// - If the provided status is valid and the retrieval is successful, it returns 200 OK with a list of ToDoTaskDto objects.
        /// - If the provided status is invalid, it returns 400 Bad Request with an error message.
        /// - If an internal server error occurs, it returns 500 Internal Server Error.
        /// </returns>
        /// <remarks>
        /// GET api/ToDoTasks/status/{status}
        /// </remarks>
        [HttpGet("api/todotask/status/{status}")]
        [ProducesResponseType(typeof(IEnumerable<ToDoTaskDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetToDoTaskByStatusAndDueDate(string status, DateTime? startDate, DateTime? endDate, int offset = 0, int limit = 100)
        {
            if (Enum.TryParse(status, true, out ToDoTaskStatus toDoTaskStatus))
            {
                var tasks = await _iToDoTaskService.GetByStatusAndDates(toDoTaskStatus, startDate, endDate, offset, limit);

                return Ok(tasks);
            }
            else
            {
                return BadRequest("Invalid status value");
            }
        }

        /// <summary>
        /// Creates new ToDoTask and returns the new ToDoTask
        /// </summary>        
        [HttpPost("api/todotask")]
        [ProducesResponseType(typeof(ToDoTaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]              
        public async Task<IActionResult> InsertToDoTask([FromBody] ToDoTaskDto toDoTaskDto)
        {
            var result = _mapper.Map<ToDoTaskDto>(await _iToDoTaskService.InsertAsync(_mapper.Map<ToDoTaskEntity>(toDoTaskDto)));
            
            return Ok(result);
        }

        /// <summary>
        /// Updates ToDoTask and returns the new ToDoTask
        /// </summary>
        [HttpPut("api/todotask/{id}")]
        [ProducesResponseType(typeof(ToDoTaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] ToDoTaskDto toDoTaskDto)
        {
            toDoTaskDto.Id = id;
            var result = _mapper.Map<ToDoTaskDto>(await _iToDoTaskService.UpdateAsync(_mapper.Map<ToDoTaskEntity>(toDoTaskDto)));
            
            return Ok(result);
        }

        /// <summary>
        /// Deletes ToDoTask and returns true/false result
        /// </summary>
        [HttpDelete("api/todotask/{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        public async Task<IActionResult> DeleteToDoTaskById(int id)
        {
            var result = _mapper.Map<ToDoTaskDto>(await _iToDoTaskService.DeleteAsync(id));
            
            return Ok(result);
        }

        /// <summary>
        /// Deletes ToDo tasks with the specified status and optional date range.
        /// </summary>
        /// <param name="status">The status of the ToDo tasks to be deleted. Should match a valid ToDoTaskStatus enumeration value.</param>
        /// <param name="startDate">Optional. The start date of the date range for tasks to be deleted. If not specified, the default is the minimum DateTime value.</param>
        /// <param name="endDate">Optional. The end date of the date range for tasks to be deleted. If not specified, the default is the maximum DateTime value.</param>
        /// <param name="offset">Optional. The number of tasks to skip before starting to delete. Default value is 0.</param>
        /// <param name="limit">Optional. The maximum number of tasks to delete. Default value is 100.</param>
        /// <returns>
        /// A Task representing an IActionResult.
        /// - If the provided status is valid and the deletion is successful, it returns 200 OK with the number of deleted tasks.
        /// - If the provided status is invalid, it returns 400 Bad Request with an error message.
        /// - If an internal server error occurs, it returns 500 Internal Server Error.
        /// </returns>
        /// <remarks>
        /// DELETE api/ToDoTasks/deleteByStatus/{status}
        /// </remarks>
        [HttpDelete("api/todotask/deleteByStatus/{status}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        public async Task<IActionResult> DeleteToDoTaskByStatusAndDates(string status, DateTime? startDate, DateTime? endDate, int offset = 0, int limit = 100)
        {
            if (Enum.TryParse(status, true, out ToDoTaskStatus toDoTaskStatus))
            {
                int tasks = await _iToDoTaskService.DeleteByStatusAndDates(toDoTaskStatus, startDate, endDate, offset, limit);

                return Ok(tasks);
            }
            else
            {
                return BadRequest("Invalid status value");
            }
        }
    }
}