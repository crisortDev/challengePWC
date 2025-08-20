using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Core.Domain.Entities
{
    public class TaskEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; } = default!;

        public string Type { get; set; } = default!; // StatusChanged | Reassigned
        public string Message { get; set; } = default!;
        public TaskStatus? OldStatus { get; set; }
        public TaskStatus? NewStatus { get; set; }
        public Guid? OldAssigneeId { get; set; }
        public Guid? NewAssigneeId { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}
