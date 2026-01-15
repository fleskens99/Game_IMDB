
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using ViewModels;
using VmMapper;

namespace Game_Web.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly IRatingService _ratingService;
        private readonly IUserService _userService;
        [BindProperty]
        public GameDetailsViewModel Game { get; set; } = null!;
        public bool CanEdit { get; private set; }
        public List<RatingOnGameViewModel> Comments { get; set; } = new();
        public string UserName { get; set; } = string.Empty;
        public bool HasUserCommented { get; set; }
        [BindProperty]
        public RatingOnGameViewModel Rating { get; set; } = new();

        public DetailsModel(IGameService gameService, IRatingService ratingService, IUserService userService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            _ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public IActionResult OnGet(int id)
        {
            var gameDto = _gameService.GetGameById(id);
            if (gameDto == null) return NotFound();

            var ratingDtos = _ratingService.GetRatingsByGame(id);
            Game = GameVmMapper.ToDetailsViewModel(gameDto, ratingDtos);

            Comments = ratingDtos.Select(RatingOnGameVmMapper.RatingDetailsViewModel).ToList();

            var userIds = Comments.Select(c => c.UserId).Distinct();
            var users = _userService.GetUsersByIds(userIds);
            var nameMap = users.ToDictionary(u => u.Id, u => u.Name);

            foreach (var comment in Comments)
            {
                comment.UserName = nameMap.ContainsKey(comment.UserId) ? nameMap[comment.UserId] : "Unknown";
            }

            int? currentUserId = null;
            if (User.Identity?.IsAuthenticated == true)
            {
                currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            }

            var isAdmin = User.IsInRole("Admin");
            CanEdit = (isAdmin && currentUserId.HasValue) || (currentUserId.HasValue && gameDto.CreatedByUserId == currentUserId.Value);

            if (User.Identity?.IsAuthenticated == true)
            {
                UserName = User.FindFirstValue(ClaimTypes.Name)!;
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                HasUserCommented = _ratingService.UserHasRated(userId, id);
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

            Rating.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (_ratingService.UserHasRated(Rating.UserId, Rating.GameId))
            {
                ModelState.AddModelError(string.Empty, "You already commented on this game.");
                return Page();
            }

            if (!ModelState.IsValid)
                return Page();

            var dto = RatingOnGameVmMapper.RatingDetailsDTO(Rating);
            _ratingService.AddRating(dto);

            return RedirectToPage(new { id = Rating.GameId });
        }
    }
}
