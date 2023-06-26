using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {
        private readonly IOwnerRepository _ownerRepo;
        private readonly IDogRepository _dogRepo;
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalksRepository _walksRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public WalkersController(IWalkerRepository walkerRepository, IOwnerRepository _ownerRepository, IDogRepository _dogRepository, IWalksRepository _walksRepository)
        {
            _walksRepo = _walksRepository;
            _ownerRepo = _ownerRepository;
            _dogRepo = _dogRepository;
            _walkerRepo = walkerRepository;
        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
        // GET: Walkers
        public ActionResult Index()
        {
            try
            {
                int LoggedInUser = GetCurrentUserId();
                Owner LoggedInOwner = _ownerRepo.GetOwnerById(LoggedInUser);
                List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(LoggedInOwner.NeighborhoodId);
                return View(walkers);
            }
            catch (Exception ex)
            {
                List<Walker> walkers = _walkerRepo.GetAllWalkers();
                return View(walkers);
            }
        }

        // GET: Walkers/Details/5
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);

            if (walker == null)
            {
                return NotFound();
            }

            List<Walks> walks = _walksRepo.GetWalksByWalkerId(id);


            WalkerProfileViewModel vm = new WalkerProfileViewModel
            {
                Walker = walker,
                Walks = walks                
            };

            return View(vm);
        }
        // GET: WalkerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WalkerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: WalkerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalkerController/Edit/5
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

        // GET: WalkerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalkerController/Delete/5
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
