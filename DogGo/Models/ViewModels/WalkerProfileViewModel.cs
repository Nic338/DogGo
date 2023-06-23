using System;
using System.Collections.Generic;

namespace DogGo.Models.ViewModels
{
    public class WalkerProfileViewModel
    {
        public List<Walks> Walks { get; set; }
        public Owner Owner { get; set; }
        public List<Walker> Walkers { get; set; }
        public Walker Walker { get; set; }
        public List<Dog> Dogs { get; set; }
        public TimeSpan TotalWalkTime { get; set; }

    }
}