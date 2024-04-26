using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using YourStudioFinal.Models.Booking;

namespace YourStudioFinal.Models;

    public class User : IdentityUser
    {
        public List<Inquiry> Inquiries { get; set; }
        
        public List<BookingModel> Bookings { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password should be between 8 and 20 characters")]
        public new string password { get; set; }

        [Required(ErrorMessage = "Valid Mobile Number is required")]
        [StringLength(11, ErrorMessage = "The Required Length is 11")]
        public string mobileNumber { get; set; }
        
        public string? Fname { get; set; }
        
        public string? Lname { get; set; }
        
        public DateTime dateCreated { get; set; }
    }
