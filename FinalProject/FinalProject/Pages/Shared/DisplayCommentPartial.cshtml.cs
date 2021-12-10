using FinalProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalProject.Pages.Shared
{
    public class DisplayCommentPartialModel : PageModel
    {
        public void OnGet()
        {
        }
        public Comment Comment {get; set;}
    }
}
