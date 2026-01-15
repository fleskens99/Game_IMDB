using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.ViewModels.Account;

namespace Game_Web.Pages.Account
{
    [AllowAnonymous]
    public class Sign_UpModel : PageModel
    {

        private readonly IUserService _userService;

        public Sign_UpModel(IUserService userService)
        {
            _userService = userService;
        }
        [BindProperty]
        public RegisteredUserVM Input { get; set; } = new();

        public IActionResult OnGet()
        {
            if (User?.Identity?.IsAuthenticated == true) return RedirectToPage("/Index");

            return Page();

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            byte[]? pictureBytes = null;

            try
            {
                _userService.RegisterUser(Input.Name, Input.Email, Input.Password, pictureBytes);
                return RedirectToPage("/Account/Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
