using System.ComponentModel;

namespace DogGo.Models.ViewModels
{
    public class DeleteWalkFormViewModel
    {
        public List<Walks> DeletableWalks { get; set; }

        [DisplayName("Walks")]
        public List<int> SelectedWalkIds { get; set; }
    }
}
