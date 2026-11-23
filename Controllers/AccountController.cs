using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WasteLogin.Models;
using WasteLogin.ViewModels;

namespace WasteLogin.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login(string role = "Admin", string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        return View(new LoginViewModel
        {
            Role = role.Equals("User", StringComparison.OrdinalIgnoreCase) ? "User" : "Admin",
            ReturnUrl = returnUrl
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        model.Role = model.Role.Equals("User", StringComparison.OrdinalIgnoreCase) ? "User" : "Admin";

        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByNameAsync(model.Username);
        if (user is null)
        {
            ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است.");
            return View(model);
        }

        // Prevent a normal user from entering the admin login tab and vice versa.
        if (!await _userManager.IsInRoleAsync(user, model.Role))
        {
            ModelState.AddModelError("", "این حساب اجازه ورود از این بخش را ندارد.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user, model.Password, model.RememberMe, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            return model.Role == "Admin"
                ? RedirectToAction("Index", "Admin")
                : RedirectToAction("Index", "User");
        }

        if (result.IsLockedOut)
            ModelState.AddModelError("", "حساب به دلیل تلاش ناموفق متعدد موقتاً قفل شده است.");
        else
            ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است.");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    public IActionResult AccessDenied() => View();
}
