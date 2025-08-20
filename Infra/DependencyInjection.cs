// Challenge.Infra/DependencyInjection.cs
using Challenge.Core.Abstraction;
using Challenge.Infra.Repositories;
using Infra.Data;
using Infra.Events;
using Infra.Repositories;
using Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Challenge.Infra
{
    public static class DependencyInjection
    {
        // Mantengo tu firma original que recibe el connection string
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string sqliteConnection)
        {
            services.AddDbContext<AppDbContext>(o => o.UseSqlite(sqliteConnection));

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEventDispatcher, EventDispatcher>();

            // ⛔ Nada de AutoMapper ni servicios de dominio acá
            return services;
        }
    }
}
