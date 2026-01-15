
using GameList;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using ViewModels;
using VmMapper;

namespace MyApp.Web.Pages.Games
{
    [Authorize(Roles = "Admin")]
    public class AddModel : PageModel
    {
        private readonly IGameService _gameService;

        public AddModel(IGameService gameService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        [BindProperty]
        public AddGameViewModel Input { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public GameIndexViewModel Exists { get; set; } = new();

        public List<SelectListItem> CategoryOptions { get; private set; } = new();

        public IActionResult OnGet(int? id)
        {
            LoadCategoryOptions();

            if (id.HasValue)
            {
                Exists.Id = id.Value;

                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (!_gameService.CanEditGame(id.Value, currentUserId, User.IsInRole("Admin")))
                    return Forbid();

                var game = _gameService.GetGameById(id.Value);
                if (game == null)
                    return NotFound();

                Input = GameVmMapper.ToAddGameViewModel(game);
            }

            return Page();
        }


        public IActionResult OnPost()
        {
            LoadCategoryOptions();

            if (!ModelState.IsValid)
                return Page();

            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out var currentUserId) || currentUserId <= 0)
                return Challenge();

            Input.CreatedByUserId = currentUserId;

            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    using var ms = new MemoryStream();
                    file.CopyTo(ms);
                    Input.Picture = ms.ToArray();
                }
            }

            if (Exists.Id > 0 && !_gameService.CanEditGame(Exists.Id, currentUserId, User.IsInRole("Admin")))
                return Forbid();

            if (Exists.Id > 0 && (Input.Picture == null || Input.Picture.Length == 0))
            {
                var existingBlob = _gameService.GetImageBlob(Exists.Id);
                Input.Picture = existingBlob;
            }

            var dto = GameVmMapper.ToAddGameDTO(Input);

            if (Exists.Id > 0)
            {
                dto.Id = Exists.Id;
                _gameService.EditGame(dto);
                return RedirectToPage("/Details", new { id = Exists.Id });
            }
            else
            {
                var newId = _gameService.AddGame(dto);
                return RedirectToPage("/Details", new { id = newId });
            }
        }


        private void LoadCategoryOptions()
        {
            CategoryOptions = GameList.GameList.All
                .Select(c => new SelectListItem { Value = c, Text = c })
                .ToList();
        }

        public IActionResult OnPostDelete()
        {
            if (Exists.Id <= 0)
                return BadRequest();

            _gameService.DeleteGame(Exists.Id);
            return RedirectToPage("/Index");
        }
    }
}
