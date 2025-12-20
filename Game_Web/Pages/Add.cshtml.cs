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
            public byte[] Picture { get; set; }
        }

        public void OnGet() 
        {
            
        }

        public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return Page();

            byte[]? picture = null;

            if (Input.Picture != null && Input.Picture.Length > 0)
            {
                picture = Input.Picture;
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
                var newId = await _gameService.AddGame(dto, cancellationToken);
                return RedirectToPage("/Games/Details", new { id = newId });
            }
            catch (Exception ex)
            {
                throw; 
            }
        }
    }
}