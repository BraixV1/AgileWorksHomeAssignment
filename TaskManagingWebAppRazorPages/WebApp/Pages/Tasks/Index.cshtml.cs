using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;
using Humanizer;

namespace WebApp.Pages.Tasks
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<ToDoTasks> ToDoTasks { get;set; } = default!;
        

        public async Task OnGetAsync()
        {
            ToDoTasks = await _context.ToDoTasks.ToListAsync();
            ToDoTasks = ToDoTasks.OrderBy(m => m.HasToBeDoneAtDt).ToList();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }
            var todoTask = await _context.ToDoTasks.FindAsync(id);

            if (todoTask == null)
            {
                return RedirectToPage("./Index");
            }
            todoTask.CompletedAtDt = DateTime.Now;
            _context.ToDoTasks.Update(todoTask);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
