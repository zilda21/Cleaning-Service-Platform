namespace CleaningService.Web.Models;

public class Booking
{
    public int Id { get; set; }

    public int UserId { get; set; }   // FK to User

    public DateOnly BookingDate { get; set; }

    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CancelledAt { get; set; }
}
