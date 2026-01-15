using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ViewModels;
using VmMapper;

namespace Game_Web.Pages;

public class IndexModel : PageModel
{
    private readonly IGameService _gameService;
    private readonly IRatingService _ratingService;
    [BindProperty]
    public List<GameIndexViewModel> Games { get; set; } = new();


    public IndexModel(IGameService gameService, IRatingService ratingService)
    {
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        _ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
    }


    public void OnGet()
    {
        var games = _gameService.GetGames();
        Games = new List<GameIndexViewModel>();

        foreach (var game in games)
        {
            double avg = _ratingService.GetAverageScoreFromGames(game.Id);

            Games.Add(GameVmMapper.ToIndexViewModel(game, avg));
        }
    }


    public IActionResult OnPost()
    {
        return RedirectToPage("/Details");
    }
    public IActionResult OnGetImage(int id)
    {
        var imageData = _gameService.GetImageBlob(id);

        if (imageData == null || imageData.Length == 0)
            return NotFound();

        return File(imageData, "image/jpeg"); 
    }

}
