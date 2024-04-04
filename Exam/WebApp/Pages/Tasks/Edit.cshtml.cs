using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;
using Humanizer;

namespace WebApp.Pages.Tasks
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ToDoTasks ToDoTasks { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todotasks =  await _context.Tasks.FirstOrDefaultAsync(m => m.Id == id);
            if (todotasks == null)
            {
                return NotFound();
            }
            ToDoTasks = todotasks;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingTask = await _context.Tasks.FindAsync(ToDoTasks.Id);
            if (existingTask == null)
            {
                return NotFound();
            }

            // Update the properties of the existing tracked entity
            existingTask.Description = ToDoTasks.Description;
            existingTask.Status = ToDoTasks.Status;
            
            

            // Attach the existing entity and mark it as modified
            _context.Attach(existingTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoTasksExists(ToDoTasks.Id))
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
}
