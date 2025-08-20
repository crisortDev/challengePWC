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
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Title).IsRequired().HasMaxLength(160);

            b.Property(x => x.AssigneeId).IsRequired();

            b.Property(x => x.Status)
             .IsRequired()
             .HasConversion<string>()
             .HasMaxLength(20);

            b.HasOne(x => x.Assignee)
             .WithMany(u => u.Tasks)
             .HasForeignKey(x => x.AssigneeId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(x => x.Events)
             .WithOne(e => e.TaskItem)
             .HasForeignKey(e => e.TaskItemId);
            // Índices útiles
            b.HasIndex(x => x.AssigneeId);
            b.HasIndex(x => x.Status);


        }
    }
}
