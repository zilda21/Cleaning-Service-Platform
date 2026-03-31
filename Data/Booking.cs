namespace CleaningService.Models;

public class Booking 
{
    public int Id { get; set; }

    public int UserId { get; set; } // FK to User

    public string Service { get; set; } = "";     // ✅ add
    public string? Notes { get; set; }            // ✅ add

    public DateOnly BookingDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CancelledAt { get; set; }
}