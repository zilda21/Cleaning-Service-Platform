using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleaningService.Web.Pages
{
    public class SignupModel : PageModel
    {
        public void OnGet() { }

        public IActionResult OnPost()
        {
            
            return Page();
        }
    }
}