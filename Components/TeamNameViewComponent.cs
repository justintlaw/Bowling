using Bowling.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bowling.Components
{
    public class TeamNameViewComponent : ViewComponent
    {
        private BowlingLeagueContext _context;

        
        // Create a VC object that has the database context
        public TeamNameViewComponent (BowlingLeagueContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Set the selected team
            ViewBag.SelectedType = RouteData?.Values["teamname"];

            return View(_context.Teams
                .Distinct()
                .OrderBy(team => team.TeamName));
        }
    }
}
