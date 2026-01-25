using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleaningService.Web.Pages
{
    public class UsersModel : PageModel
    {
        public void OnGet() { }

        public IActionResult OnPost()
        {
            // DEMO MODE – backend disabled
            return Page();
        }
    }
}