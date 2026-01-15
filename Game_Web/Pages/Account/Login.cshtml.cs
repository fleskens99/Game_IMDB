using Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.ViewModels.Account;
using System.Security.Claims;

namespace Game_Web.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public LogedInVm Input { get; set; } = new();

        public IActionResult OnGet() 
        {

            if (User?.Identity?.IsAuthenticated == true)
            return RedirectToPage("/Index");

            return Page();

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var user = _userService.ValidateLogin(Input.Email, Input.Password);
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

            if (user.Value.Admin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }


            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).GetAwaiter().GetResult();

            return RedirectToPage("/Index");
        }
    }
}

