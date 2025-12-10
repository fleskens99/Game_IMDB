using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ViewModels;
using Interfaces;
using DTOs;

namespace Game_Web.Pages;

public class IndexModel : PageModel
{
    private readonly IGameRepo _gameRepo;
    [BindProperty]
    public GameVM Game { get; set; } = new();
    public IFormFile? Picture { get; set; }
    public List<GameDTO> Games { get; set; } = new();

    public IndexModel(IGameRepo gameRepo)
    {
        _gameRepo = gameRepo;
    }


    public void OnGet() 
    {
        Games = _gameRepo.GetGames();
    }
    public IActionResult OnPost()
    {
        return RedirectToPage("/Details");
    }
}
