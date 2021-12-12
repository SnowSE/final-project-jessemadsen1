using FinalProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace FinalProject.Pages.Shared
{
    public class DisplayCommentPartialModel : PageModel
    {
        public Comment Comment {get; set;}
        public Vote Vote { get; set; }

    }
}
