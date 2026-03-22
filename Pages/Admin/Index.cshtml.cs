using System.Net.Http.Json;
using CleaningService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleaningService.Pages.Admin;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _http;

    public IndexModel(IHttpClientFactory http)
    {
        _http = http;
    }

    public List<User> Users { get; set; } = new();
    public string? ErrorMsg { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        // Optional: block non-admins if you use Session role
        // if (HttpContext.Session.GetString("Role") != "Admin")
        //     return RedirectToPage("/login");
  if (HttpContext.Session.GetString("Role") != "Admin")
        return RedirectToPage("/login");
        try
        {
            var api = _http.CreateClient();
            api.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");

            // Your API returns {Id, Name, Email, Role} — this still deserializes into User fine
            Users = await api.GetFromJsonAsync<List<User>>("/api/auth/users") ?? new();
        }
        catch
        {
            ErrorMsg = "Failed to load users.";
        }

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {

          if (HttpContext.Session.GetString("Role") != "Admin")
        return RedirectToPage("/login");

        try
        {
            var api = _http.CreateClient();
            api.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");

            var res = await api.DeleteAsync($"/api/auth/users/{id}");
            if (!res.IsSuccessStatusCode)
            {
                ErrorMsg = $"Delete failed ({(int)res.StatusCode})";
                await OnGetAsync();
                return Page();
            }

    

            return RedirectToPage();
        }
        catch
        {
            ErrorMsg = "Delete failed.";
            await OnGetAsync();
            return Page();
        }
    }
}