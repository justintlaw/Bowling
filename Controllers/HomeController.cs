using Bowling.Infrastructure;
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

        [HttpPost]
        public IActionResult Category(int teamId)
        {
            
            Categories categories = HttpContext.Session.GetJson<Categories>("Categories") ?? new Categories();
            List<long> categoryIds = categories.SelectedTeamIds;

            if (categoryIds.Contains(teamId))
            {
                categories.RemoveCategory(teamId);
            }

            else
            {
                categories.AddCategory(teamId);
            }

            HttpContext.Session.SetJson("Categories", categories);

            return RedirectToAction("Index");
        }

        public IActionResult Index(int pageNum = 1)
        {
            // set the number of items for pagination
            const int pageSize = 5;

            //IQueryable<Bowler> query = _context.Bowlers;

            //if (teamId is not null && !_selectedTeamIds.Contains(teamId.Value))
            //{
            //    _selectedTeamIds.Add(teamId.Value);
            //}

            //if (teamId is not null)
            //{

            //    //IQueryable<Bowler> query = _context.Bowlers
            //    //    .Where(bowler => bowler.TeamId == teamId || teamId == null);

            //    query = _context.Bowlers
            //        .Where(b => _selectedTeamIds.Contains(b.TeamId.Value));
            //}

            // set the title of the page to the team tame if included
            if (false)
            {
                //ViewData["Title"] = $"{teamName}";
            }
            else
            {
                ViewData["Title"] = $"All";
            }

            IQueryable<Bowler> query = _context.Bowlers;
            Categories categories = HttpContext.Session.GetJson<Categories>("Categories") ?? new Categories();
            List<long> categoryIds = categories.SelectedTeamIds;

            if (categoryIds.Count() > 0)
            {
                query = query.Where(b => categoryIds.Contains(b.TeamId.Value));
            }

            // Return a view containing a list of bowlers and the page numbering info
            return View(new IndexViewModel
            {
                // Get bowlers for a certain team, or all teams if teamId is null
                //Bowlers = (_context.Bowlers
                //    .Where(bowler => bowler.TeamId == teamId || teamId == null)
                //    .OrderBy(bowler => bowler.BowlerLastName)
                //    .ThenBy(bowler => bowler.BowlerFirstName)
                //    .Skip((pageNum - 1) * pageSize)
                //    .Take(pageSize)
                //    .ToList()),
                Bowlers = (query
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
                    //TotalNumItems = teamId == null ? _context.Bowlers.Count() :
                    //    _context.Bowlers.Where(bowler => bowler.TeamId == teamId).Count()
                    TotalNumItems = query.Count()
                },
                TeamName = null
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
