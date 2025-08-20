using AutoMapper;
using Challenge.Core.Abstraction;
using Challenge.Core.Abstraction.Services;
using Challenge.Core.Domain;
using Challenge.Core.DTOs.Users;
using Challenge.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class UserService : IUserService
    {
        // Inyección de dependencias
        private readonly IRepository<User> _repo;  // Repositorio de usuarios
        private readonly IMapper _mapper;           // Mapeador de objetos (AutoMapper)
        private readonly IUnitOfWork _uow;         // Unidad de trabajo (para guardar cambios en la base de datos)
        private readonly ILogger<UserService> _logger;  // Logger para registrar información y advertencias

        // Constructor de la clase
        public UserService(IRepository<User> repo, IMapper mapper, IUnitOfWork uow, ILogger<UserService> logger)
        {
            _repo = repo;  // Inicializa el repositorio de usuarios
            _mapper = mapper;  // Inicializa el mapeador de objetos
            _uow = uow;  // Inicializa la unidad de trabajo
            _logger = logger;  // Inicializa el logger
        }

        // Método para crear un nuevo usuario
        public async Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken ct = default)
        {
            // Log de información, indicando que se está creando un nuevo usuario
            _logger.LogInformation("Creating new user with email: {Email}", dto.Email);

            // Mapear el DTO a la entidad User
            var entity = _mapper.Map<User>(dto);

            // Guardar el nuevo usuario en la base de datos
            await _repo.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            // Log de éxito, indicando que el usuario fue creado correctamente
            _logger.LogInformation("User created successfully with ID: {UserId}", entity.Id);
            return _mapper.Map<UserDto>(entity);  // Retorna el DTO del usuario creado
        }

        // Método para obtener un usuario por su ID
        public async Task<UserDto?> GetAsync(Guid id, CancellationToken ct = default)
        {
            // Log de información, indicando que se está buscando un usuario por su ID
            _logger.LogInformation("Fetching user with ID: {UserId}", id);

            // Buscar el usuario por su ID
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity is null)
            {
                // Log de advertencia si el usuario no es encontrado
                _logger.LogWarning("User with ID {UserId} not found", id);
                return null;
            }

            // Log de éxito si el usuario fue encontrado
            _logger.LogInformation("User with ID {UserId} fetched successfully", id);
            return _mapper.Map<UserDto>(entity);  // Retorna el DTO del usuario
        }

        // Método para listar todos los usuarios
        public async Task<IReadOnlyList<UserDto>> ListAsync(CancellationToken ct = default)
        {
            // Log de información, indicando que se está obteniendo la lista de usuarios
            _logger.LogInformation("Fetching list of all users.");

            // Obtener la lista de todos los usuarios
            var list = await _repo.ListAsync(ct: ct);
            if (list.Count == 0)
            {
                // Log de advertencia si no se encuentran usuarios
                _logger.LogWarning("No users found.");
            }

            // Convertir la lista de entidades User a DTOs
            var userDtos = list.Select(x => _mapper.Map<UserDto>(x)).ToList();

            // Log de éxito, indicando cuántos usuarios se encontraron
            _logger.LogInformation("Fetched {Count} users.", userDtos.Count);
            return userDtos;  // Retorna la lista de DTOs
        }

        // Método para actualizar un usuario existente
        public async Task UpdateAsync(Guid id, UpdateUserDto dto, CancellationToken ct = default)
        {
            // Log de información, indicando que se está actualizando un usuario
            _logger.LogInformation("Updating user with ID: {UserId}", id);

            // Obtener el usuario por su ID
            var entity = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("User not found");
            _mapper.Map(dto, entity);  // Mapear el DTO a la entidad existente

            // Guardar los cambios en la base de datos
            await _repo.UpdateAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            // Log de éxito, indicando que el usuario fue actualizado correctamente
            _logger.LogInformation("User with ID: {UserId} updated successfully", id);
        }

        // Método para eliminar un usuario
        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            // Log de información, indicando que se está eliminando un usuario
            _logger.LogInformation("Deleting user with ID: {UserId}", id);

            // Obtener el usuario por su ID
            var entity = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("User not found");

            // Eliminar el usuario de la base de datos
            await _repo.DeleteAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            // Log de éxito, indicando que el usuario fue eliminado correctamente
            _logger.LogInformation("User with ID: {UserId} deleted successfully", id);
        }
    }
}
