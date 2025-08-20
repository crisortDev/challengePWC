using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Core.Domain.Events
{
    public record TaskStatusChangedEvent(Guid TaskId, TaskStatus OldStatus, TaskStatus NewStatus);

}
