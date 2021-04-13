using Bowling.Infrastructure;
using Bowling.Models;
using Bowling.Models.ViewModels;
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
        private CategoryViewModel _categoryInfo;

        
        // Create a VC object that has the database context
        public TeamNameViewComponent (BowlingLeagueContext context)
        {
            _context = context;
            _categoryInfo = new CategoryViewModel(_context.Teams);
        }

        public IViewComponentResult Invoke()
        {
            // Set the selected team
            //ViewBag.SelectedType = RouteData?.Values["teamname"];

            _categoryInfo.Teams = _context.Teams
                .Distinct()
                .OrderBy(team => team.TeamName);

            Categories categories = HttpContext.Session.GetJson<Categories>("Categories") ?? new Categories();
            _categoryInfo.SelectedTeamIds = categories.SelectedTeamIds;

            return View(_categoryInfo);
        }
    }
}
