using AutoMapper;
using Challenge.Core.Abstraction;
using Challenge.Core.Abstraction.Services;
using Challenge.Core.Domain;
using Challenge.Core.Domain.Entities;
using Challenge.Core.Domain.Events;
using Challenge.Core.DTOs.Tasks;
using Core.Abstraction.Services;
using Core.Domain.Exceptions;
using Core.Specifications;
using Microsoft.Extensions.Logging;  // Asegúrate de importar este espacio de nombres

namespace Challenge.Core.Services
{
    public class TaskService : ITaskService
    {
        // Inyecciones de dependencias
        private readonly IRepository<TaskItem> _tasks;
        private readonly IRepository<User> _users;
        private readonly IEventRepository _events;  // Cambiar de IRepository<TaskEvent> a IEventRepository
        private readonly IEventDispatcher _dispatcher;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<TaskService> _logger;  // Inyectando el logger para registrar información

        // Constructor de la clase
        public TaskService(
           IRepository<TaskItem> tasks,
            IRepository<User> users,
            IEventRepository eventsRepo,  // Cambiar de IRepository<TaskEvent> a IEventRepository
            IEventDispatcher dispatcher,
            IMapper mapper,
            IUnitOfWork uow,
            ILogger<TaskService> logger)  // Inyección del logger
        {
            _tasks = tasks;
            _users = users;
            _events = eventsRepo;  // Inicialización de IEventRepository
            _dispatcher = dispatcher;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;  // Asignación del logger
        }

        // Método para crear una nueva tarea
        public async Task<TaskDto> CreateAsync(CreateTaskDto dto, CancellationToken ct = default)
        {
            // Registrar la creación de la tarea
            _logger.LogInformation("Creating task with title: {Title} for assignee {AssigneeId}", dto.Title, dto.AssigneeId);

            // Validar que el asignado (assignee) exista
            var assignee = await _users.GetByIdAsync(dto.AssigneeId, ct);
            if (assignee == null)
            {
                _logger.LogWarning("Assignee with ID {AssigneeId} not found", dto.AssigneeId);  // Log de advertencia
                throw new KeyNotFoundException("Assignee not found");
            }

            // Mapear el DTO a la entidad TaskItem
            var entity = _mapper.Map<TaskItem>(dto);
            await _tasks.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            // Log de éxito
            _logger.LogInformation("Task {TaskId} created successfully", entity.Id);
            return _mapper.Map<TaskDto>(entity);
        }

        // Método para obtener una tarea por su ID
        public async Task<TaskDto?> GetAsync(Guid id, CancellationToken ct = default)
        {
            // Log de inicio
            _logger.LogInformation("Fetching task with ID {TaskId}", id);

            // Buscar la tarea con el ID proporcionado
            var spec = new TaskWithUserAndEventsSpec(id);
            var entity = (await _tasks.ListAsync(spec, ct)).FirstOrDefault();

            if (entity == null)
            {
                // Log de advertencia si la tarea no se encuentra
                _logger.LogWarning("Task with ID {TaskId} not found", id);
                return null;
            }

            // Log de éxito
            _logger.LogInformation("Task {TaskId} fetched successfully", id);
            return _mapper.Map<TaskDto>(entity);
        }

        // Método para listar todas las tareas asignadas a un usuario específico
        public async Task<IReadOnlyList<TaskDto>> ListAsync(Guid? assigneeId = null, CancellationToken ct = default)
        {
            // Log de inicio
            _logger.LogInformation("Fetching task list for assignee {AssigneeId}", assigneeId);

            // Si el ID del asignado es proporcionado, filtramos las tareas por ese asignado
            var spec = assigneeId.HasValue ? new TaskByAssigneeSpec(assigneeId.Value) : null;
            var items = await _tasks.ListAsync(spec, ct);
            var taskDtos = items.Select(x => _mapper.Map<TaskDto>(x)).ToList();

            // Log de la cantidad de tareas obtenidas
            _logger.LogInformation("Fetched {Count} tasks for assignee {AssigneeId}", taskDtos.Count, assigneeId);
            return taskDtos;
        }

        // Método para actualizar una tarea existente
        public async Task UpdateAsync(Guid id, UpdateTaskDto dto, CancellationToken ct = default)
        {
            // Log de inicio
            _logger.LogInformation("Updating task with ID {TaskId}", id);

            // Obtener la tarea por su ID
            var entity = await _tasks.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Task not found");
            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.UtcNow;

            // Guardar los cambios
            await _tasks.UpdateAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            // Log de éxito
            _logger.LogInformation("Task {TaskId} updated successfully", id);
        }

        // Método para cambiar el estado de una tarea
        public async Task ChangeStatusAsync(Guid id, ChangeTaskStatusDto dto, CancellationToken ct = default)
        {
            // Log de inicio
            _logger.LogInformation("Changing status of task {TaskId} to {NewStatus}", id, dto.NewStatus);

            // Obtener la tarea o lanzar una excepción si no se encuentra
            var entity = await GetTaskOrThrow(id, ct);
            var oldStatus = entity.Status;

            // Validar que el nuevo estado esté dentro de los valores definidos en el enum
            if (!Enum.IsDefined(typeof(TaskStatus), dto.NewStatus))
            {
                // Log de advertencia si el estado es inválido
                _logger.LogWarning("Invalid status value: {NewStatus} for task {TaskId}. Allowed values are 0 (Pending), 1 (InProgress), or 2 (Completed).", dto.NewStatus, id);
                throw new ArgumentException($"Invalid status value: {dto.NewStatus}. Allowed values are: {string.Join(", ", Enum.GetNames(typeof(TaskStatus)))}");
            }

            // Si el estado no cambia, no hacer nada
            if (oldStatus == dto.NewStatus) return;

            // Actualizar el estado de la tarea
            entity.Status = dto.NewStatus;
            entity.UpdatedAt = DateTime.UtcNow;
            await _tasks.UpdateAsync(entity, ct);

            // Crear el evento de cambio de estado
            var ev = new TaskEvent
            {
                TaskItemId = entity.Id,
                Type = "StatusChanged",
                Message = $"Status changed from {oldStatus} to {entity.Status}",
                OldStatus = oldStatus,
                NewStatus = entity.Status
            };

            // Guardar el evento en la base de datos
            await _events.AddEventAsync(ev, ct);

            // Log de auditoría
            _logger.LogInformation("Task {TaskId} status changed from {OldStatus} to {NewStatus}", entity.Id, oldStatus, entity.Status);

            // Guardar cambios y publicar el evento
            await _uow.SaveChangesAsync(ct);
            await _dispatcher.PublishAsync(new TaskStatusChangedEvent(entity.Id, oldStatus, entity.Status), ct);
        }

        // Método privado para obtener una tarea o lanzar una excepción si no se encuentra
        private async Task<TaskItem> GetTaskOrThrow(Guid taskId, CancellationToken ct)
        {
            var task = await _tasks.GetByIdAsync(taskId, ct);
            if (task == null)
            {
                // Log de advertencia si la tarea no se encuentra
                _logger.LogWarning("Task with ID {TaskId} not found", taskId);
                throw new TaskNotFoundException(taskId); // Excepción personalizada
            }
            return task;
        }

        // Método para reasignar una tarea a otro usuario
        public async Task ReassignAsync(Guid id, ReassignTaskDto dto, CancellationToken ct = default)
        {
            // Log de inicio
            _logger.LogInformation("Reassigning task {TaskId} to user {NewAssigneeId}", id, dto.NewAssigneeId);

            // Obtener la tarea y el nuevo asignado
            var entity = await _tasks.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Task not found");
            _ = await _users.GetByIdAsync(dto.NewAssigneeId, ct) ?? throw new KeyNotFoundException("Assignee not found");

            var old = entity.AssigneeId;
            if (old == dto.NewAssigneeId) return;

            // Actualizar el asignado
            entity.AssigneeId = dto.NewAssigneeId;
            entity.UpdatedAt = DateTime.UtcNow;
            await _tasks.UpdateAsync(entity, ct);

            // Crear el evento de reasignación
            var ev = new TaskEvent
            {
                TaskItemId = entity.Id,
                Type = "Reassigned",
                Message = $"Task reassigned from {old} to {entity.AssigneeId}",
                OldAssigneeId = old,
                NewAssigneeId = entity.AssigneeId
            };

            // Guardar el evento en la base de datos
            await _events.AddEventAsync(ev, ct);

            // Log de éxito
            _logger.LogInformation("Task {TaskId} reassigned from {OldAssigneeId} to {NewAssigneeId}", entity.Id, old, entity.AssigneeId);

            // Guardar cambios y publicar el evento
            await _uow.SaveChangesAsync(ct);
            await _dispatcher.PublishAsync(new TaskReassignedEvent(entity.Id, old, entity.AssigneeId), ct);
        }

        // Método para eliminar una tarea
        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            // Log de inicio
            _logger.LogInformation("Deleting task with ID {TaskId}", id);

            var entity = await _tasks.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Task not found");
            await _tasks.DeleteAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            // Log de éxito
            _logger.LogInformation("Task {TaskId} deleted successfully", id);
        }
    }
}
