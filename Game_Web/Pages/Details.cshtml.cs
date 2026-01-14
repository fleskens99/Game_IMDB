using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace Game_Web.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IGameRepo _gameRepo;
        private readonly IRatingService _ratingService;
        private readonly IRatingRepo _ratingRepo;

        public GameDTO Game { get; set; } = null!;
        public List<RatingDTO> Comments { get; set; } = new();
        public string UserName { get; set; } = string.Empty;
        public bool HasUserCommented { get; set; }


        [BindProperty]
        public RatingDTO Rating { get; set; } = new RatingDTO();

        public DetailsModel(IGameRepo gameRepo, IRatingService ratingService, IRatingRepo ratingRepo)
        {
            _gameRepo = gameRepo;
            _ratingService = ratingService;
            _ratingRepo = ratingRepo;
        }

        public IActionResult OnGet(int id)
        {
            Game = _gameRepo.GetGameById(id);
            Comments = _ratingRepo.GetRatingsByGame(id);

            if (Game == null)
                return NotFound();

            if (User.Identity!.IsAuthenticated)
            {
                UserName = User.FindFirstValue(ClaimTypes.Name)!;

                int userId = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                HasUserCommented = _ratingRepo.UserHasRated(userId, id);
            }
            else
            {
                UserName = "Guest";
            }

            Rating.GameId = id;
            return Page();
        }


        public IActionResult OnPost()
        {
            if (!User.Identity!.IsAuthenticated)
                return Unauthorized();

            Rating.UserId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            if (_ratingRepo.UserHasRated(Rating.UserId, Rating.GameId))
            {
                ModelState.AddModelError(string.Empty, "You already commented on this game.");
                return Page();
            }

            if (!ModelState.IsValid)
                return Page();

            _ratingService.AddRating(Rating);

            return RedirectToPage(new { id = Rating.GameId });
        }


    }
}
