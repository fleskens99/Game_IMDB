using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Game_Web.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IGameRepo _gameRepo;
        private readonly IRatingService _ratingService;

        public GameDTO Game { get; set; } = null!;

        [BindProperty]
        public RatingDTO Rating { get; set; } = new RatingDTO();

        public DetailsModel(IGameRepo gameRepo, IRatingService ratingService)
        {
            _gameRepo = gameRepo;
            _ratingService = ratingService;
        }

        public IActionResult OnGet(int id)
        {
            Game = _gameRepo.GetGameById(id);

            if (Game == null)
                return NotFound();

            Rating.GameId = id;

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _ratingService.AddRating(Rating);

            return RedirectToPage("/Index");
        }
    }
}
