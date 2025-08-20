using Challenge.Core.DTOs.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Core.Abstraction.Services
{
    public interface ITaskService
    {
        Task<TaskDto> CreateAsync(CreateTaskDto dto, CancellationToken ct = default);
        Task<TaskDto?> GetAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<TaskDto>> ListAsync(Guid? assigneeId = null, CancellationToken ct = default);
        Task UpdateAsync(Guid id, UpdateTaskDto dto, CancellationToken ct = default);
        Task ChangeStatusAsync(Guid id, ChangeTaskStatusDto dto, CancellationToken ct = default);
        Task ReassignAsync(Guid id, ReassignTaskDto dto, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
