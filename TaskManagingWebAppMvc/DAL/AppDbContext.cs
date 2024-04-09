using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;



public class AppDbContext : DbContext
{
    public DbSet<ToDoTask> ToDoTasks { get; set; } = default!;
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    
}