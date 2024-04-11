using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

// dotnet ef migrations add --project DAL --startup-project ConsoleApp InitialCreate
public class AppDbContext : DbContext
{ 
    
    public DbSet<ToDoTasks> ToDoTasks { get; set; } = default!;
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}