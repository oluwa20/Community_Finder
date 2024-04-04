using Community_Finder2.Data;
using Community_Finder2.Models;
using Community_Finder2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Community_Finder2.Controllers
{
    public class CommunityController : Controller
    {
        private readonly Community_Finder2DbContext _context;

        public CommunityController(Community_Finder2DbContext context)
        {
            _context = context;
        }

        // GET: Community/Create
        public IActionResult Create()
        {
            ViewBag.Locations = new SelectList(_context.Locations, "Id", "Name");
            return View();
        }

        // POST: Community/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,LocationId")] Community community)
        {
            if (ModelState.IsValid)
            {
                _context.Add(community);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Locations = new SelectList(_context.Locations, "Id", "Name", community.LocationId);
            return View(community);
        }

        // GET: Community/Index
        public IActionResult Index()
        {
            var communities = _context.Communities.Include(c => c.Location).ToList();
            return View(communities);
        }

        // POST: Community/SearchCommunities
        [HttpPost]
        public IActionResult SearchCommunities(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction(nameof(Index));
            }

            var targetCommunity = _context.Communities
                .Include(c => c.Location)
                .FirstOrDefault(c => c.Name.Contains(searchTerm));

            if (targetCommunity == null)
            {
                return View("NotFound");
            }

            var nearestCommunities = GetNearestCommunities(targetCommunity, 20);
            return View(nameof(SearchResults), new SearchResultsViewModel
            {
                TargetCommunity = targetCommunity,
                NearestCommunities = nearestCommunities
            });
        }

        // Method to get nearest communities within a specified radius
        private List<Community> GetNearestCommunities(Community targetCommunity, double radiusInKm)
        {
            var nearestCommunities = new List<Community>();

            var allCommunities = _context.Communities
                .Include(c => c.Location)
                .Where(c => c.Id != targetCommunity.Id)
                .ToList();

            foreach (var community in allCommunities)
            {
                var distance = CalculateDistance(targetCommunity.Location, community.Location);
                if (distance <= radiusInKm)
                {
                    nearestCommunities.Add(community);
                }
            }

            return nearestCommunities;
        }

        // Method to calculate distance between two locations
        private double CalculateDistance(Location location1, Location location2)
        {
            const double radiusOfEarth = 6371; // Earth's radius in kilometers

            var lat1 = ToRadians(location1.Latitude);
            var lon1 = ToRadians(location1.Longitude);
            var lat2 = ToRadians(location2.Latitude);
            var lon2 = ToRadians(location2.Longitude);

            var dLat = lat2 - lat1;
            var dLon = lon2 - lon1;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = radiusOfEarth * c;

            return distance;
        }

        // Method to convert degrees to radians
        private double ToRadians(double angle)
        {
            return angle * (Math.PI / 180);
        }

        // GET: Community/SearchResults
        public IActionResult SearchResults(SearchResultsViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}
