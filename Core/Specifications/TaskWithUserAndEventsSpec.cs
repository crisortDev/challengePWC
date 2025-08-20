using Challenge.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public sealed class TaskWithUserAndEventsSpec : BaseSpecification<TaskItem>
    {
        public TaskWithUserAndEventsSpec(Guid taskId)
        {
            Criteria = t => t.Id == taskId;
            AddInclude(t => t.Assignee);
            AddInclude(t => t.Events);
        }
    }
}
