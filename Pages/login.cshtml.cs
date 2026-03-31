using CleaningService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleaningService.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter both email and password.";
                return Page();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == Email && u.Password == Password);

            if (user == null)
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Role", user.Role ?? "User");
            HttpContext.Session.SetString("Name", user.Name ?? "");

            if (string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToPage("/Admin/Index");
            }

            return RedirectToPage("/Booking/Index");
        }
    }
}