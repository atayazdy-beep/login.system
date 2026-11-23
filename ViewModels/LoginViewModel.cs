using System.ComponentModel.DataAnnotations;

namespace WasteLogin.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "نام کاربری را وارد کنید")]
    [Display(Name = "نام کاربری")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "رمز عبور را وارد کنید")]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور")]
    public string Password { get; set; } = "";

    [Display(Name = "مرا به خاطر بسپار")]
    public bool RememberMe { get; set; }

    public string Role { get; set; } = "Admin";
    public string? ReturnUrl { get; set; }
}
