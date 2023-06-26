namespace DogGo.Models.ViewModels
{
    public class WalksViewModel
    {
        public Walks Walk { get; set; }
        public List<Walker> Walkers { get; set; }
        public List<Dog> Dogs { get; set; }
        //List of DogId's the user selects from the drop down
        public List<int> SelectedDogIds { get; set; }
    }
}