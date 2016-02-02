using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TourGuide.ViewModels
{
    public class ContactViewModel
    {
        [Required]
        [StringLength(20, MinimumLength =2)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(255, MinimumLength =10)]
        public string Message { get; set; }
    }
}
