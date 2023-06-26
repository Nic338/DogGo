using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Walker
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Hmmm... You should really add a Name...")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Neighborhood")]
        public int NeighborhoodId { get; set; }
        [Required(ErrorMessage = "Please include a photo of yourself!")]
        [DisplayName("Avatar")]
        public string ImageUrl { get; set; }
        public Neighborhood Neighborhood { get; set; }
    }
}