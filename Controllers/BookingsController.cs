using CleaningService.Web.Data;
using CleaningService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleaningService.Web.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BookingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("my")]
    public IActionResult MyBookings()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return Unauthorized();

        var list = _context.Bookings
            .Where(b => b.UserId == userId.Value)
            .OrderByDescending(b => b.CreatedAt)
            .ToList();

        return Ok(list);
    }

    [HttpPost]
    public IActionResult Create([FromForm] Booking booking)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return Unauthorized();

        // enforce ownership
        booking.UserId = userId.Value;

        // basic validation
        if (booking.EndTime <= booking.StartTime)
            return BadRequest(new { message = "EndTime must be after StartTime" });

        _context.Bookings.Add(booking);
        _context.SaveChanges();

        return StatusCode(201, new { message = "created", id = booking.Id });
    }
}