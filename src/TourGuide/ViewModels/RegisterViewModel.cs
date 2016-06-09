using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TourGuide.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(15, MinimumLength =4)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string RePassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
