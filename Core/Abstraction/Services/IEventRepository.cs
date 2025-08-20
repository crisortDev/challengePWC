using Challenge.Core.Abstraction;
using Challenge.Core.Domain.Entities;

namespace Core.Abstraction.Services
{
    public interface IEventRepository : IRepository<TaskEvent>
    {
        Task AddEventAsync(TaskEvent taskEvent, CancellationToken ct);
    }
}
