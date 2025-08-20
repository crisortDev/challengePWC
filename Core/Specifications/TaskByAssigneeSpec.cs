using Challenge.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public sealed class TaskByAssigneeSpec : BaseSpecification<TaskItem>
    {
        public TaskByAssigneeSpec(Guid assigneeId)
        {
            Criteria = t => t.AssigneeId == assigneeId;
            AddInclude(t => t.Assignee);
        }
    }
}
