

namespace Challenge.Core.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public TaskStatus Status { get; set; } = (TaskStatus)Core.Domain.Enums.TaskStatus.Pending;

        // Asignación obligatoria
        public Guid AssigneeId { get; set; }
        public User Assignee { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<TaskEvent> Events { get; set; } = new List<TaskEvent>();
    }
}
