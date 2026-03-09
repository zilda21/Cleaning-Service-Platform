using System.Linq;
using CleaningService.Web.Data;
using CleaningService.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace CleaningService.Web.Pages.Booking;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;

    public IndexModel(ApplicationDbContext db)
    {
        _db = db;
    }

    public List<Models.Booking> Bookings { get; set; } = new();
    public string? ErrorMsg { get; set; }

    [TempData]
public string? SuccessMsg { get; set; }

    public IActionResult OnGet()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToPage("/login");

        LoadBookings(userId.Value);
        return Page();
    }

    public IActionResult OnPost()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToPage("/login");

        // Read form fields
        var service = Request.Form["Service"].ToString();
        var notes = Request.Form["Notes"].ToString();
        var bookingDateStr = Request.Form["BookingDate"].ToString();
        var startTimeStr = Request.Form["StartTime"].ToString();
        var endTimeStr = Request.Form["EndTime"].ToString();

        // Basic validation
        if (string.IsNullOrWhiteSpace(service) ||
            string.IsNullOrWhiteSpace(bookingDateStr) ||
            string.IsNullOrWhiteSpace(startTimeStr) ||
            string.IsNullOrWhiteSpace(endTimeStr))
        {
            ErrorMsg = "Please fill all required fields.";
            LoadBookings(userId.Value);
            return Page();
        }

        if (!DateOnly.TryParse(bookingDateStr, out var bookingDate))
        {
            ErrorMsg = "Invalid date.";
            LoadBookings(userId.Value);
            return Page();
        }

        if (!TimeOnly.TryParse(startTimeStr, out var startTime) ||
            !TimeOnly.TryParse(endTimeStr, out var endTime))
        {
            ErrorMsg = "Invalid time.";
            LoadBookings(userId.Value);
            return Page();
        }

        if (endTime <= startTime)
        {
            ErrorMsg = "End time must be after start time.";
            LoadBookings(userId.Value);
            return Page();
        }

        // Create booking object
        var booking = new Models.Booking
        {
            UserId = userId.Value,
            Service = service,
            Notes = string.IsNullOrWhiteSpace(notes) ? null : notes,
            BookingDate = bookingDate,
            StartTime = startTime,
            EndTime = endTime,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _db.Bookings.Add(booking);
        _db.SaveChanges();

        return RedirectToPage(); // reload list
    }

    private void LoadBookings(int userId)
    {
        Bookings = _db.Bookings
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt)
            .ToList();
    }
}