using System.Net.Http.Json;
using System.Reflection;
using CleaningService.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleaningService.Web.Pages.Booking;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _http;

    public IndexModel(IHttpClientFactory http)
    {
        _http = http;
    }

    public List<Models.Booking> Bookings { get; set; } = new();
    public string? ErrorMsg { get; set; }

    // Used by the page to render inputs dynamically
    public PropertyInfo[] BookingProps { get; } =
        typeof(Models.Booking).GetProperties(BindingFlags.Public | BindingFlags.Instance);

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var api = _http.CreateClient();
            api.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");

            Bookings = await api.GetFromJsonAsync<List<Models.Booking>>("/api/bookings") ?? new();
        }
        catch
        {
            ErrorMsg = "Failed to load bookings.";
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var api = _http.CreateClient();
            api.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");

            // Build form fields from posted inputs (matches Booking property names)
            var fields = new Dictionary<string, string>();

            foreach (var p in BookingProps)
            {
                if (string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase))
                    continue;

                // checkbox values come as "on" or missing; treat missing as false
                if (p.PropertyType == typeof(bool) || p.PropertyType == typeof(bool?))
                {
                    fields[p.Name] = Request.Form.ContainsKey(p.Name) ? "true" : "false";
                    continue;
                }

                var val = Request.Form[p.Name].ToString();
                if (!string.IsNullOrWhiteSpace(val))
                    fields[p.Name] = val;
            }

            var res = await api.PostAsync("/api/bookings", new FormUrlEncodedContent(fields));
            if (!res.IsSuccessStatusCode)
            {
                ErrorMsg = $"Create booking failed ({(int)res.StatusCode})";
                await OnGetAsync();
                return Page();
            }

            return RedirectToPage();
        }
        catch
        {
            ErrorMsg = "Create booking failed.";
            await OnGetAsync();
            return Page();
        }
    }
}