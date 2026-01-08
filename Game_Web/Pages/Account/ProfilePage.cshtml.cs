using Interfaces;
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

    public string DisplayName { get; private set; } = "User";
    public string Email { get; private set; } = "—";

    [BindProperty]
    public string OldPassword { get; set; } = "";

    [BindProperty]
    public string NewPassword { get; set; } = "";

    public void OnGet()
    {
        DisplayName = User.Identity?.Name ?? "User";
        Email = User.FindFirstValue(ClaimTypes.Email) ?? "—";
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme)
            .GetAwaiter().GetResult();

        return RedirectToPage("/Index");
    }

    public IActionResult OnPostChangePassword()
    {
        DisplayName = User.Identity?.Name ?? "User";
        Email = User.FindFirstValue(ClaimTypes.Email) ?? "—";

        if (string.IsNullOrWhiteSpace(OldPassword) || string.IsNullOrWhiteSpace(NewPassword))
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
            _userService.ChangePassword(userId, OldPassword, NewPassword);
            return RedirectToPage(); 
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
