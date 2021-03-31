using Bowling.Models;
using Bowling.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Bowling.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private BowlingLeagueContext _context { get; set; }

        public HomeController(ILogger<HomeController> logger, BowlingLeagueContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(int? teamId, string teamName, int pageNum = 1)
        {
            // set the number of items for pagination
            const int pageSize = 5;

            // set the title of the page to the team tame if included
            if (teamId is not null)
            {
                ViewData["Title"] = $"{teamName}";
            }
            else
            {
                ViewData["Title"] = $"All";
            }

            // Return a view containing a list of bowlers and the page numbering info
            return View(new IndexViewModel
            {
                // Get bowlers for a certain team, or all teams if teamId is null
                Bowlers = (_context.Bowlers
                    .Where(bowler => bowler.TeamId == teamId || teamId == null)
                    .OrderBy(bowler => bowler.BowlerLastName)
                    .ThenBy(bowler => bowler.BowlerFirstName)
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)
                    .ToList()),
                // Page numbering info for pagination
                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,
                    TotalNumItems = teamId == null ? _context.Bowlers.Count() :
                        _context.Bowlers.Where(bowler => bowler.TeamId == teamId).Count()
                },
                TeamName = teamName
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
