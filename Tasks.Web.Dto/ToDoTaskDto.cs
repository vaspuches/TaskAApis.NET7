using Swashbuckle.AspNetCore.Annotations;

namespace Tasks.Web.Dto
{
    public class ToDoTaskDto
    {
        [SwaggerSchema(Nullable = true, ReadOnly = true)]
        public int? Id { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public String? Status { get; set; }
    }
}