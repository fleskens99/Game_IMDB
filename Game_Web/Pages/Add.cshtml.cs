using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;


namespace MyApp.Web.Pages.Games
{
    public class AddModel : PageModel
    {
        private readonly IGameService _gameService;

        public AddModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            public string Title { get; set; } = string.Empty;
            public string? Description { get; set; }
            public string Category { get; set; } = string.Empty;
            public IFormFile? Picture { get; set; }
        }

        public void OnGet() 
        {
            
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return Page();

            byte[]? picture = null;

            if (Input.Picture != null && Input.Picture.Length > 0)
            {
                using var ms = new MemoryStream();
                await Input.Picture.CopyToAsync(ms, cancellationToken);
                picture = ms.ToArray();
            }

            var dto = new GameDTO
            {
                Name = Input.Title.Trim(),
                Description = Input.Description?.Trim(),
                Category = Input.Category.Trim(),
                Picture = picture 
            };

            try
            {
                var newId = _gameService.AddGame(dto);
                return RedirectToPage("/Index", new { id = newId });
            }
            catch (Exception ex)
            {
                throw; 
            }
        }
    }
}