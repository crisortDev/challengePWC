using Challenge.Core.Abstraction;
using Challenge.Core.Domain;
using Challenge.Core.Domain.Entities;

namespace Core.Abstraction
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
    }
    public interface ITaskRepository : IRepository<TaskItem> { }
    public interface ITaskEventRepository : IRepository<TaskEvent> { }
}
