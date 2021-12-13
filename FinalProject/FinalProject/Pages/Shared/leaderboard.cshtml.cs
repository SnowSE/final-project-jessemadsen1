using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalProject.Pages.Shared
{
    public class leaderboardModel : PageModel
    {
        public IEnumerable<Vote> Votes { get; set; }
        public int CountVotes { get; private set; }
        public void OnGet()
        {
            var model = new leaderboardModel();
        }
    }
}
