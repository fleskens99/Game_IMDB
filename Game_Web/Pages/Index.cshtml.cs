using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Interfaces;
using DTOs;

namespace Game_Web.Pages;

public class IndexModel : PageModel
{
    private readonly IGameRepo _gameRepo;
    private readonly IRatingRepo _ratingRepo;
    public double AverageRating { get; set; }
    [BindProperty]
    public List<GameDTO> Games { get; set; } = new();


    public IndexModel(IGameRepo gameRepo, IRatingRepo ratingRepo)
    {
        _gameRepo = gameRepo;
        _ratingRepo = ratingRepo;
    }


    public void OnGet() 
    {
        Games = _gameRepo.GetGames();

        foreach (var game in Games)
        {
            AverageRating = _ratingRepo.GetAverageRatingForGame(game.Id);
        }
    }
    public IActionResult OnPost()
    {
        return RedirectToPage("/Details");
    }
    public IActionResult OnGetImage(int id)
    {
        var imageData = _gameRepo.GetImageBlob(id);

        if (imageData == null || imageData.Length == 0)
            return NotFound();

        return File(imageData, "image/jpeg"); 
    }

}
