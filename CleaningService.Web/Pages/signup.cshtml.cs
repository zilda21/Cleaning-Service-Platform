using CleaningService.Web.Data;
using CleaningService.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleaningService.Web.Pages
{
    public class SignupModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SignupModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User NewUser { get; set; }

        public List<User> Users { get; set; } = new();

        public IActionResult OnPost()
        {

              if(!ModelState.IsValid){
            return Page();
            }
            
            _context.Add(NewUser);
            _context.SaveChanges();
     return Page();
          
        }
    }
}
