using Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.ViewModels;
using System.Security.Claims;

namespace Game_Web.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IUserService _users;
    public LoginModel(IUserService users) => _users = users;

    [BindProperty]
    public LogedInVm Input { get; set; } = new();

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        var user = _users.ValidateLogin(Input.Email, Input.Password);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return Page();
        }

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Value.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Value.Name),
            new Claim(ClaimTypes.Email, user.Value.Email),
        };

        ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).GetAwaiter().GetResult();

        return RedirectToPage("/Index");
    }
}
