using System.Net.Http.Json;
using CleaningService.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleaningService.Web.Pages.Booking;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _http;

    public IndexModel(IHttpClientFactory http) => _http = http;

    public List<Models.Booking> Bookings { get; set; } = new();
    public string? ErrorMsg { get; set; }
    public string? SuccessMsg { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetInt32("UserId") == null)
            return RedirectToPage("/login");

        await LoadBookings();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetInt32("UserId") == null)
            return RedirectToPage("/login");

        var api = _http.CreateClient();
        api.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");

        var fields = new Dictionary<string, string>
        {
            ["Service"] = Request.Form["Service"],
            ["Notes"] = Request.Form["Notes"],
            ["BookingDate"] = Request.Form["BookingDate"],
            ["StartTime"] = Request.Form["StartTime"],
            ["EndTime"] = Request.Form["EndTime"],
        };

        var res = await api.PostAsync("/api/bookings", new FormUrlEncodedContent(fields));

        if (!res.IsSuccessStatusCode)
        {
            ErrorMsg = "Booking failed.";
            await LoadBookings();
            return Page();
        }

        SuccessMsg = "Booking created!";
        return RedirectToPage(); // refresh
    }

    private async Task LoadBookings()
    {
        var api = _http.CreateClient();
        api.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");

        Bookings = await api.GetFromJsonAsync<List<Models.Booking>>("/api/bookings/my") ?? new();
    }
}