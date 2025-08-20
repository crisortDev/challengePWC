using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Exceptions
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(Guid taskId)
            : base($"Task with ID {taskId} not found.")
        {
        }
    }
}
