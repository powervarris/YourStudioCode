namespace YourStudioFinal.Models.Booking;

public class CalendarModel
{
    public int Year { get; set; }
    public List<DateTime> Dates { get; set; }

    public CalendarModel(int year)
    {
        Year = year;
        Dates = new List<DateTime>();

        for (int month = 1; month <= 12; month++)
        {
            int daysInMonth = DateTime.DaysInMonth(Year, month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                Dates.Add(new DateTime(Year, month, day));
            }
        }
    }
}