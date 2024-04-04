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
            ToDoTasks = await _context.Tasks.ToListAsync();
            ToDoTasks = ToDoTasks.OrderBy(m => m.HasToBeDoneDt).ToList();
        }
    }
}
