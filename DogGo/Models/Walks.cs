using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Walks
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        [DisplayName("Walker")]
        public int WalkerId { get; set; }
        [DisplayName("Dog(s) to walk")]
        public int DogId { get; set; }
        public Owner Owner { get; set; }
    }
}
