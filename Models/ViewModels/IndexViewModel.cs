using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bowling.Models.ViewModels
{
    public class IndexViewModel
    {
        // View Model for the Index page
        public IEnumerable<Bowler> Bowlers { get; set; }
        public PageNumberingInfo PageNumberingInfo { get; set; }
        public string TeamName { get; set; }
    }
}
