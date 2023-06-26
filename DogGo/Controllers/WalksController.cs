using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
    public class WalksController : Controller
    {
        private readonly IWalksRepository _walksRepo;
        private readonly IOwnerRepository _ownerRepo;
        private readonly IDogRepository _dogRepo;
        private readonly IWalkerRepository _walkerRepo;
        private readonly INeighborhoodRepository _neighborhoodRepo;
        public WalksController(IWalkerRepository walkerRepository, IOwnerRepository _ownerRepository, IDogRepository _dogRepository, IWalksRepository _walksRepository, INeighborhoodRepository neighborhoodRepo)
        {
            _walksRepo = _walksRepository;
            _ownerRepo = _ownerRepository;
            _dogRepo = _dogRepository;
            _walkerRepo = walkerRepository;
            _neighborhoodRepo = neighborhoodRepo;
        }
        // GET: HomeController1
        public ActionResult Index()
        {
            return View();
        }

        // GET: HomeController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController1/Create
        public ActionResult Create()
        {

            List<Dog> dogs = _dogRepo.GetAllDogs();
            List<Walker> walkers = _walkerRepo.GetAllWalkers();

            WalksViewModel vm = new WalksViewModel()
            {
                Walk = new Walks(),
                Dogs = dogs,
                Walkers = walkers,
                SelectedDogIds = new List<int>()
            };

            //set a default date to now
            vm.Walk.Date = DateTime.Now;

            return View(vm);
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Walks walk, List<int> SelectedDogIds)
        {
            try
            {
                _walksRepo.AddWalks(walk, SelectedDogIds);

                return RedirectToAction($"Details", "Walkers", new { id = walk.WalkerId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Walkers");
            }
        }

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
