using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.DTOs
{
   public  class UpdateUserDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Squad { get; set; }
        [Required]
        public string Stack { get; set; } 

        [Required]
        [Display(Name = "Phone number")]
        [MaxLength(14, ErrorMessage = "Phone number must not be 14 characters")]
        [RegularExpression(@"^\+\d{3}\d{9,10}$", ErrorMessage = "Must have country-code and must be 13, 14 chars long e.g. +2348050000000")]

        public string  PhoneNumber { get; set; }
    }
}
