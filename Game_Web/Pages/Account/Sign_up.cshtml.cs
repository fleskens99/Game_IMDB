using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ViewModels;

namespace Game_Web.Pages.Account
{
    public class Sign_UpModel : PageModel
    {

        private readonly IUserService _userService;

        public Sign_UpModel(IUserService userService)
        {
            _userService = userService;
        }
        [BindProperty]
        public RegisteredUserVM Input { get; set; } = new();

        public void OnGet()
        {
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
