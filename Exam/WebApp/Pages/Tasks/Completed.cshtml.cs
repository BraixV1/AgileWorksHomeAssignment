using System.Runtime.InteropServices;
using DAL;
using Domain;
using Domain.HelperEnums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Tasks;

public class CompletedModel : PageModel
{
    private readonly AppDbContext _context;
    public CompletedModel(AppDbContext dbContext)
    {
        _context = dbContext;
    }
    
    public  async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var toDoTask = await _context.Tasks.FirstOrDefaultAsync(m => m.Id == id);

        if (toDoTask == null)
        {
            return NotFound();
        }

        toDoTask.Status = EStatus.Completed;
        
        _context.Attach(toDoTask).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ToDoTasksExists(toDoTask.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
        
    }
    private bool ToDoTasksExists(Guid id)
    {
        return _context.Tasks.Any(e => e.Id == id);
    }
}