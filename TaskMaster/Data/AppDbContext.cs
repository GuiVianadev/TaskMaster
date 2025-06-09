using Microsoft.EntityFrameworkCore;
using TaskMaster.Data.Configuration;

namespace TaskMaster.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public DbSet<Entities.Task> Tasks { get; set; }
    public DbSet<Entities.User> Users { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}