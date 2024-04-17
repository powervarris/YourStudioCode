namespace YourStudioFinal.Models.Booking;

public class FilterBookingModel
{
    public FilterBookingModel(string Pending, List<BookingModel> toList)
    {
        filter = Pending;
        bookings = toList;
    }

    public string filter { get; set; }
    public List<BookingModel> bookings { get; set; }
}