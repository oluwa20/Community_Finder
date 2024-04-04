using Community_Finder2.Data;
using Community_Finder2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Community_Finder2.Controllers
{
    public class LocationController : Controller
    {
        private readonly Community_Finder2DbContext _context;

        public LocationController(Community_Finder2DbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Locations.Add(location);
                _context.SaveChanges();
                return RedirectToAction(nameof(ViewLocations));
            }
            return View(location);
        }

        public IActionResult ViewLocations()
        {
            var locations = _context.Locations.ToList();
            return View(locations);
        }
    }
}

