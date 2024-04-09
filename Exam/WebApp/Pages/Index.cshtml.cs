using DAL;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class IndexModel : PageModel
{

    
    public async Task<IActionResult> OnGetAsync()
    {
        return await Task.Run( () => RedirectToPage("/Tasks/Index"));
    }
}