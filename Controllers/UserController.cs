using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WasteLogin.Controllers;

[Authorize(Roles = "User")]
public class UserController : Controller
{
    public IActionResult Index() => View();
}
