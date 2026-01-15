using Interfaces;
using ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Game_Web.Pages.Account;

[Authorize]
public class ProfilePageModel : PageModel
{
    private readonly IUserService _userService;

    public ProfilePageModel(IUserService userService)
    {
        _userService = userService;
    }

    public UserAccountViewModel Details { get; set; } = new UserAccountViewModel();

    [BindProperty]
    public UserChangePasswordViewModel PasswordChange { get; set; } = new UserChangePasswordViewModel();

    public void OnGet()
    {
        Details.Name = User.Identity?.Name ?? "User";
        Details.Email = User.FindFirstValue(ClaimTypes.Email) ?? "—";
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme)
            .GetAwaiter().GetResult();

        return RedirectToPage("/Index");
    }

    public IActionResult OnPostChangePassword()
    {
        Details.Name = User.Identity?.Name ?? "User";
        Details.Email = User.FindFirstValue(ClaimTypes.Email) ?? "—";

        if (string.IsNullOrWhiteSpace(PasswordChange.OldPassword) || string.IsNullOrWhiteSpace(PasswordChange.NewPassword))
        {
            ModelState.AddModelError(string.Empty, "Please fill in both password fields.");
            return Page();
        }

        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!long.TryParse(idStr, out var userId))
        {
            ModelState.AddModelError(string.Empty, "Could not identify your account.");
            return Page();
        }

        try
        {
            _userService.ChangePassword(userId, PasswordChange.OldPassword, PasswordChange.NewPassword);
            return RedirectToPage(); 
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
