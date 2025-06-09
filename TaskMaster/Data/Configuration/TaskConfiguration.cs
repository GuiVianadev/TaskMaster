using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = TaskMaster.Entities.Task;

namespace TaskMaster.Data.Configuration;

public class TaskConfiguration : IEntityTypeConfiguration<Entities.Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
       builder.ToTable("Tasks");
       
       builder.HasKey(x => x.Id);
       
       builder.Property(t => t.Title)
           .IsRequired()
           .HasMaxLength(100);
       builder.Property(t => t.Description)
           .HasMaxLength(500);
       
       builder.Property(t => t.Status)
           .HasConversion<string>();
        
       builder.Property(t => t.Priority)
           .HasConversion<string>();

       builder.Property(t => t.UserId)
           .IsRequired();
       
       builder.HasOne(t => t.User)
           .WithMany(t => t.Tasks)
           .HasForeignKey(t => t.UserId)
           .OnDelete(DeleteBehavior.Restrict);

    }
}