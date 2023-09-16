using Microsoft.EntityFrameworkCore;
using Pipelines.CleanArchitecture.Domain;
using Pipelines.CleanArchitecture.Infrastructure.Persistance.Tables;

namespace Pipelines.CleanArchitecture.Infrastructure.Persistance;

internal class ToDoDbContext : DbContext
{
    public DbSet<ToDo> ToDos { get; set; }
    
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options){ }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ToDoEntityConfiguration());
    }
}