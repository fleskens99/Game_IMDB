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
        // If not logged in, show default icon
        if (!(User.Identity?.IsAuthenticated ?? false))
            return Redirect("/uploads/Pictures/Profile.png");

        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!long.TryParse(idStr, out var userId))
            return Redirect("/uploads/Pictures/Profile.png");

        var bytes = _users.GetUserPicture(userId);

        // If user has no picture in DB, show default
        if (bytes == null || bytes.Length == 0)
            return Redirect("/uploads/Pictures/Profile.png");

        // If you always save as PNG, keep this. Otherwise you’d also store the content type.
        return File(bytes, "image/png");
    }
}

