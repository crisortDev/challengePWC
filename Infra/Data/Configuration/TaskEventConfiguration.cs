using Challenge.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Configuration
{
    public class TaskEventConfiguration : IEntityTypeConfiguration<TaskEvent>
    {
        public void Configure(EntityTypeBuilder<TaskEvent> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Type).IsRequired().HasMaxLength(60);
            b.Property(x => x.Message).IsRequired().HasMaxLength(300);
        }
    }
}
