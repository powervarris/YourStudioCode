using System.ComponentModel.DataAnnotations;

namespace YourStudioFinal.Models.Booking;

public class BookingModel
{
    
    [Key] //primary key
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string userID { get; set; } //userID from user table
    
    public User accountUser{ get; set; }
    
    // public string slctTime { get; set; }
    public DateTime dateCreated { get; set; }
    
    public string status { get; set; }
    
    public string date { get; set; }
    public string time { get; set; }
    
    public string packages { get; set; }
    
    public string addOns { get; set; }
    
    // public string totalPrice { get; set; }
    
    public string? paymentID { get; set; }
    public PaymentModel payment { get; set; }

}