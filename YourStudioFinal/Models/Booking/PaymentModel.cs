namespace YourStudioFinal.Models.Booking;

public class PaymentModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string fileName { get; set; }
    
    public string BookingId { get; set; } = null!;
    public BookingModel Booking { get; set; } = null!;
}