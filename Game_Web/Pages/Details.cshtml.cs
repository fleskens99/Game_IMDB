using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ViewModels;


namespace Game_Web.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IGameRepo _gameRepo;

        public GameDTO Game { get; set; } = null!;
        public RatingDTO Rating { get; set; } = null!;

        public DetailsModel(IGameRepo gameRepo)
        {
            _gameRepo = gameRepo;
        }

        public IActionResult OnGet(int id)
        {
            Game = _gameRepo.GetGameById(id);

            if (Game == null)
                return NotFound();

            return Page();
        }
    }

}
