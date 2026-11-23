using Microsoft.AspNetCore.Identity;

namespace WasteLogin.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = "";
}
