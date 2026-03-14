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

        var service = Request.Form["Service"].ToString();
        var notes = Request.Form["Notes"].ToString();
        var bookingDateStr = Request.Form["BookingDate"].ToString();
        var startTimeStr = Request.Form["StartTime"].ToString();
        var endTimeStr = Request.Form["EndTime"].ToString();

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

        // Today's date
        var today = DateOnly.FromDateTime(DateTime.Now);

        // 1) Prevent past date
        if (bookingDate < today)
        {
            ErrorMsg = "Booking date cannot be in the past.";
            LoadBookings(userId.Value);
            return Page();
        }

        // 2) Prevent past time if booking is for today
        var nowTime = TimeOnly.FromDateTime(DateTime.Now);
        if (bookingDate == today && startTime <= nowTime)
        {
            ErrorMsg = "Start time must be in the future for today's booking.";
            LoadBookings(userId.Value);
            return Page();
        }

        // 3) Business hours validation
        var openingTime = new TimeOnly(8, 0);   // 08:00
        var closingTime = new TimeOnly(18, 0);  // 18:00

        if (startTime < openingTime || endTime > closingTime)
        {
            ErrorMsg = "Bookings are only allowed between 08:00 and 18:00.";
            LoadBookings(userId.Value);
            return Page();
        }

        // 4) Weekend validation
        var bookingDateTime = bookingDate.ToDateTime(TimeOnly.MinValue);
        if (bookingDateTime.DayOfWeek == DayOfWeek.Saturday ||
            bookingDateTime.DayOfWeek == DayOfWeek.Sunday)
        {
            ErrorMsg = "Bookings are only allowed on weekdays.";
            LoadBookings(userId.Value);
            return Page();
        }

        // 5) Prevent overlap for same user and same date
        bool hasOverlap = _db.Bookings.Any(b =>
            b.UserId == userId.Value &&
            b.BookingDate == bookingDate &&
            b.Status != "Cancelled" &&
            startTime < b.EndTime &&
            endTime > b.StartTime
        );

        if (hasOverlap)
        {
            ErrorMsg = "You already have another booking that overlaps with this time.";
            LoadBookings(userId.Value);
            return Page();
        }

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

        SuccessMsg = "Booking created successfully.";
        return RedirectToPage();
    }

    private void LoadBookings(int userId)
    {
        Bookings = _db.Bookings
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt)
            .ToList();
    }
}