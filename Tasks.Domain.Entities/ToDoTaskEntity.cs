namespace Tasks.Domain.Entities
{
    public class ToDoTaskEntity
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate{ get; set; }
        public String? Status { get; set; }
    }
}