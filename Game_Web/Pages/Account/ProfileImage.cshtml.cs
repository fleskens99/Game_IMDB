using System.Security.Claims;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Game_Web.Pages.Account;

public class ProfileImageModel : PageModel
{
    private readonly IUserService _users;
    public ProfileImageModel(IUserService users) => _users = users;

    public IActionResult OnGet()
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
            return Redirect("/uploads/Pictures/Profile.png");

        string idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!long.TryParse(idStr, out var userId))
            return Redirect("/uploads/Pictures/Profile.png");

        byte[] bytes = _users.GetUserPicture(userId);
        if (bytes == null || bytes.Length == 0)
            return Redirect("/uploads/Pictures/Profile.png");

        return File(bytes, "image/png");
    }
}

