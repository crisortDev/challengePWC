

namespace Challenge.Core.DTOs.Tasks
{
    public record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    TaskStatus Status,
    Guid AssigneeId,
    string AssigneeName,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
}
