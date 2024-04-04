using DAL;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(AppDbContext context, ILogger<IndexModel> logger)
    {
        _logger = logger;
    }
    
    [BindProperty(SupportsGet = true)]
    public string Search { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public bool InJob { get; set; }
        
    [BindProperty(SupportsGet = true)]
    public bool InVan { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public bool InCategory { get; set; }
    

    public async Task<IActionResult> OnPostAsync()
    {
        return Page();
    }

    public async Task<IActionResult> OnGetAsync()
    {
        return RedirectToPage("/Tasks/Index");
    }
}