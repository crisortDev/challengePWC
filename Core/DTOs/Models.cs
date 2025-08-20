using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Core.DTOs
{
    public record UserCreateDto(string Email, string FullName);
    public record UserDto(Guid Id, string Email, string FullName, bool IsActive);

    public record TaskCreateDto(string Title, string? Description, Guid AssigneeId);
    public record TaskUpdateDto(string? Title, string? Description);
    public record TaskAssignDto(Guid AssigneeId);
    public record TaskChangeStatusDto(TaskStatus Status);

    public record TaskDto(Guid Id, string Title, string? Description, TaskStatus Status, Guid AssigneeId);
    //public record TaskEventDto(Guid Id, Guid TaskId, DateTime CreatedAt, string Type, string Description);
}
