// Challenge.Core.Abstraction/IRepository.cs
using Challenge.Core.Domain;
using Challenge.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Challenge.Core.Abstraction
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T>? spec = null, CancellationToken ct = default);
        Task<T> AddAsync(T entity, CancellationToken ct = default);
        Task UpdateAsync(T entity, CancellationToken ct = default);
        Task DeleteAsync(T entity, CancellationToken ct = default);
    }

    //public interface IUserRepository : IRepository<User>
    //{
    //    Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
    //}

    //public interface ITaskRepository : IRepository<TaskItem> { }
    //public interface ITaskEventRepository : IRepository<TaskEvent> { }

    //public interface IUnitOfWork
    //{
    //    Task<int> SaveChangesAsync(CancellationToken ct = default);
    //}

    //public interface IEventPublisher
    //{
    //    void Publish(TaskEvent @event);
    //}

    //public interface IEventSubscriber
    //{
    //    void Handle(TaskEvent @event);
    //}
}