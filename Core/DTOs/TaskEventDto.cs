using Challenge.Core.Domain.Enums;
using TaskStatus = Challenge.Core.Domain.Enums.TaskStatus;

namespace Challenge.Core.DTOs
{
    public class TaskEventDto(
        Guid Id,
        Guid TaskItemId,
        string Type,
        string Message,
        TaskStatus? OldStatus,
        TaskStatus? NewStatus,
        Guid? OldAssigneeId,
        Guid? NewAssigneeId,
        DateTime OccurredAt
    );
}
