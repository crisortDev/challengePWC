using Challenge.Core.Abstraction;
using Challenge.Core.Domain.Entities;
using Core.Abstraction.Services;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _db;

        public EventRepository(AppDbContext db)
        {
            _db = db;
        }

        // Implementación de AddEventAsync, que es un método específico para agregar eventos
        public async Task AddEventAsync(TaskEvent taskEvent, CancellationToken ct)
        {
            // Agregar el evento a la base de datos
            await _db.TaskEvents.AddAsync(taskEvent, ct);
            await _db.SaveChangesAsync(ct); // Guardamos los cambios
        }

        // Implementación de los métodos heredados de IRepository<TaskEvent> (si es necesario)
        public async Task<TaskEvent?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.TaskEvents.FindAsync(new object?[] { id }, ct);
        }

        public async Task<IReadOnlyList<TaskEvent>> ListAsync(ISpecification<TaskEvent>? spec = null, CancellationToken ct = default)
        {
            var query = _db.TaskEvents.AsQueryable();

            // Aplicar las especificaciones si existen
            if (spec?.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            return await query.ToListAsync(ct);
        }

        public Task UpdateAsync(TaskEvent entity, CancellationToken ct)
        {
            _db.TaskEvents.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TaskEvent entity, CancellationToken ct)
        {
            _db.TaskEvents.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<TaskEvent> AddAsync(TaskEvent entity, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
