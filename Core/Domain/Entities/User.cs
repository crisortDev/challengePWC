﻿using Challenge.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Core.Domain
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
