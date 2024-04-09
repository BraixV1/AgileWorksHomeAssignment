using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : Controller
{

    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("AllTasks")]
    public async Task<IEnumerable<ToDoTask>> getAllTasks()
    {
        return await _context.ToDoTasks.ToListAsync();
    } 
    
    [HttpPost]
    [Route("AddTask")]
    public async Task<ToDoTask> AddTask(ToDoTask objTask)
    {
        await _context.ToDoTasks.AddAsync(objTask);
        await _context.SaveChangesAsync();
        return objTask;
    }

    [HttpPatch]
    [Route("UpdateTask/{id}")]
    public async Task<ToDoTask> UpdateTask(ToDoTask objTask)
    {
        _context.Attach(objTask).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return objTask;
    }

    [HttpPatch]
    [Route("CompleteTask/{id}")]
    public async Task<bool> CompleteTask(Guid id)
    {
        var toDoTask = await _context.ToDoTasks.FindAsync(id);
        if (toDoTask != null)
        {
            toDoTask.CompletedAtDt = DateTime.Now;
            _context.Attach(toDoTask).State = EntityState.Modified;
            return true;
        }

        return false;

    }
        
    
    [HttpDelete]
    [Route("DeleteTask/{id}")]
    public async Task<bool> DeleteTask(Guid id)
    {
        var toDoTask = await _context.ToDoTasks.FindAsync(id);
        if (toDoTask == null) return false;
        _context.ToDoTasks.Remove(toDoTask);
        await _context.SaveChangesAsync();
        return true;

    }

    [HttpGet]
    [Route("GetTask/{id}")]
    public async Task<ToDoTask> GetTask(Guid id)
    {
        var toDoTask = await _context.ToDoTasks.FindAsync(id);
        if (toDoTask == null) return new ToDoTask();

        return toDoTask;
    }
}