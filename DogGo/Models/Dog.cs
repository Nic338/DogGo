using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Dog
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Hmmm... You should really add a Name...")]
        [MaxLength(35)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Your dog must have an owner! NO STRAYS")]
        [DisplayName("Owner Id #")]
        public int OwnerId { get; set; }      
        public string Breed { get; set; }
        public string Notes { get; set; }
        [Required(ErrorMessage = "LET ME SEE YOUR DOGGO")]
        [DisplayName("Photo of your doggo")]
        public string ImageUrl { get; set; }
        public Owner Owner { get; set; }
    }
}
