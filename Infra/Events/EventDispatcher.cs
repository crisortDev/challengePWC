using Challenge.Core.Abstraction;
using Microsoft.Extensions.Logging;

namespace Infra.Events
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly ILogger<EventDispatcher> _logger;
        public EventDispatcher(ILogger<EventDispatcher> logger) => _logger = logger;

        public Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default)
        {
            _logger.LogInformation("DomainEvent published: {Event}", @event);
            return Task.CompletedTask;
        }
    }
}
