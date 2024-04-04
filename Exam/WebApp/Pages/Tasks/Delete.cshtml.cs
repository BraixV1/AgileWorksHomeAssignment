using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Tasks
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
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

            var todotasks = await _context.Tasks.FirstOrDefaultAsync(m => m.Id == id);

            if (todotasks == null)
            {
                return NotFound();
            }
            else
            {
                ToDoTasks = todotasks;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todotasks = await _context.Tasks.FindAsync(id);
            if (todotasks != null)
            {
                ToDoTasks = todotasks;
                _context.Tasks.Remove(ToDoTasks);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
