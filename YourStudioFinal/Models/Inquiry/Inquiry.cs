using System.ComponentModel.DataAnnotations;

namespace YourStudioFinal.Models;

public class Inquiry
{
    [Key] //primary key
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string userID { get; set; } //userID from user table
    public User accountUser{ get; set; }
    public string Name { get; set; }
    public string Message { get; set; }
}