using DogGo.Models;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IWalksRepository
    {
        List<Walks> GetWalksByWalkerId(int walkerId);
        void AddWalks(Walks walk, List<int> SelectedDogIds);
        void DeleteWalks(List<int> SelectedWalkIds);
    }
}
