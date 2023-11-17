using System.ComponentModel.DataAnnotations;

namespace TestProjectLatvia.ViewModels;

public class RegisterModel
{
    public string Name { get; set; }
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}