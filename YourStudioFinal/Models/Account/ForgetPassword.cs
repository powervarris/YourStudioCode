using System.ComponentModel.DataAnnotations;

namespace YourStudioFinal.Models;

public class ForgetPassword
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}