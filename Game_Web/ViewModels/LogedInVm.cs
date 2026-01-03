using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class LogedInVm
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
    }
}
