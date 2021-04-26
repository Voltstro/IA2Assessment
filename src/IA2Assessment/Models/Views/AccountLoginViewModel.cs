using System.ComponentModel.DataAnnotations;

namespace IA2Assessment.Models.Views
{
    public class AccountLoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}