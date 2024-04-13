using System.Net;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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


    /// <summary>
    /// Returns all tasks that have entered to database
    /// </summary>
    /// <returns>list of tasks</returns>
    [HttpGet]
    [Route("AllTasks")]
    [ProducesResponseType<IEnumerable<ToDoTask>>((int)HttpStatusCode.OK)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<IEnumerable<ToDoTask>>> GetAllTasks()
    {
        var found = await _context.ToDoTasks.ToListAsync();
        return Ok(found.OrderByDescending(task => task.HasToBeDoneAtDt));
    }


    /// <summary>
    /// Add new task to database
    /// </summary>
    /// <returns>the added object or returns 400 Bad Request and lets user know what is missing</returns>
    [HttpPost]
    [Route("AddTask")]
    [ProducesResponseType<ToDoTask>((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<ToDoTask>> AddTask(ToDoTask objTask)
    {
        List<string> failures = new();
        try
        {
            if (objTask.CreatedAtDt == default) failures.Add("Created at date not added");
            if (objTask.HasToBeDoneAtDt == default) failures.Add("Has to be done at data not added");
            if (failures.Count > 0) return BadRequest($"Following errors were found {failures}");
            await _context.ToDoTasks.AddAsync(objTask);
            await _context.SaveChangesAsync();
            return Ok(objTask);
        }
        catch (Exception e)
        {
            return BadRequest($"An error occured during adding a task: {e.Message}");
        }
    }


    /// <summary>
    /// Update the task that was given
    /// </summary>
    /// <returns>returns the updated object or 400 Bad Request</returns>
    [HttpPatch]
    [Route("UpdateTask/{id}")]
    [ProducesResponseType<ToDoTask>((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<ToDoTask>> UpdateTask(ToDoTask updatedTask)
    {
        try
        {
            var existingTask = await _context.ToDoTasks.FindAsync(updatedTask.Id);
            if (existingTask == null) return NotFound($"Task with ID {updatedTask.Id} was not found.");

            existingTask.Description = updatedTask.Description;
            existingTask.HasToBeDoneAtDt = updatedTask.HasToBeDoneAtDt;
            existingTask.CompletedAtDt = updatedTask.CompletedAtDt;

            await _context.SaveChangesAsync();

            return Ok(existingTask);
        }
        catch (Exception e)
        {
            return BadRequest($"An error occurred during the update: {e.Message}");
        }
    }


    /// <summary>
    /// Will give not completed task a completed at time stamp so it will marked as completed
    /// </summary>
    /// <returns>200 if it managed to find the task and set it's completeAtDt to server time if it  fails returns 404</returns>
    [HttpPatch]
    [Route("CompleteTask/{id}")]
    [ProducesResponseType<bool>((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult> CompleteTask(Guid id)
    {
        var toDoTask = await _context.ToDoTasks.FindAsync(id);
        if (toDoTask == null) return NotFound("Task was not found");

        toDoTask.CompletedAtDt = DateTime.Now;
        _context.Entry(toDoTask).Property(x => x.CompletedAtDt).IsModified = true;

        await _context.SaveChangesAsync();

        return Ok();
    }


    /// <summary>
    /// Deletes task that has same id as given
    /// </summary>
    /// <returns>returns 200 if managed to delete it and 404 if it didn't find the task</returns>
    [HttpDelete]
    [Route("DeleteTask/{id}")]
    [ProducesResponseType<ToDoTask>((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult> DeleteTask(Guid id)
    {
        var toDoTask = await _context.ToDoTasks.FindAsync(id);
        if (toDoTask == null) return NotFound($"Task with id {id} was not found");
        _context.ToDoTasks.Remove(toDoTask);
        await _context.SaveChangesAsync();
        return Ok();
    }


    /// <summary>
    /// Looks for the task based on the id given
    /// </summary>
    /// <returns>object that was sea</returns>
    [HttpGet]
    [Route("GetTask/{id}")]
    [ProducesResponseType<ToDoTask>((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<ToDoTask>> GetTask(Guid id)
    {
        var toDoTask = await _context.ToDoTasks.FindAsync(id);
        if (toDoTask == null) return NotFound("Task not found");
        return Ok(toDoTask);
    }
}