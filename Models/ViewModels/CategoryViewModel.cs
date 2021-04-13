using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bowling.Models.ViewModels
{
    public class CategoryViewModel
    {
        public IEnumerable<Team> Teams { get; set; }
        public List<long> SelectedTeamIds { get; set; }

        public CategoryViewModel(IEnumerable<Team> teams)
        {
            Teams = teams;
            SelectedTeamIds = new List<long>();
        }
    }
}
