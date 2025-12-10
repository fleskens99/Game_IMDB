using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;


namespace MyApp.Web.Pages.Games
{
    public class AddModel : PageModel
    {
        private readonly IAddGameService _gameService;

        public AddModel(IAddGameService gameService)
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

            byte[]? picture = null; // <-- byte[] instead of string

            if (Input.Picture != null && Input.Picture.Length > 0)
            {
                using var ms = new MemoryStream();
                await Input.Picture.CopyToAsync(ms, cancellationToken);
                picture = ms.ToArray(); // <-- raw bytes
            }

            var dto = new GameDTO
            {
                Name = Input.Title.Trim(),
                Description = Input.Description?.Trim(),
                Category = Input.Category.Trim(),
                Picture = picture // <-- now correct type (byte[])
            };

            try
            {
                var newId = await _gameService.AddGame(dto, cancellationToken);
                return RedirectToPage("/Games/Details", new { id = newId });
            }
            catch (Exception ex)
            {
                throw; // shows the real exception
            }
        }
    }
}