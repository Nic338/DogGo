namespace DogGo.Models
{
    public class Walks
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public int WalkerId { get; set; }
        public int DogId { get; set; }
        public Owner Owner { get; set; }
    }
}
