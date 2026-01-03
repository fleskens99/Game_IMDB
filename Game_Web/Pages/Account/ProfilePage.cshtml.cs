using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Game_Web.Pages.Account;

[Authorize]
public class ProfilePageModel : PageModel
{
    public string DisplayName { get; private set; } = "User";
    public string Email { get; private set; } = "—";
    public string UserId { get; private set; } = "—";

    public void OnGet()
    {
        DisplayName = User.Identity?.Name ?? "User";

        Email = User.FindFirstValue(ClaimTypes.Email) ?? "—";

        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        UserId = string.IsNullOrWhiteSpace(id) ? "—" : id;
    }
}
