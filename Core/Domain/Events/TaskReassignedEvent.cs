using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Core.Domain.Events
{
    public record TaskReassignedEvent(Guid TaskId, Guid OldAssigneeId, Guid NewAssigneeId);

}
