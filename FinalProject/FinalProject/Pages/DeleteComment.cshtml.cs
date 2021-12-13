using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FinalProject;
using FinalProject.Data;
using Microsoft.Extensions.Logging;

namespace FinalProject.Pages.Shared
{
    public class DeleteCommentModel : PageModel
    {
        private readonly FinalProject.Data.ApplicationDbContext _dbcontext;
        private readonly ILogger<DeleteCommentModel> log;
        public DeleteCommentModel(FinalProject.Data.ApplicationDbContext context, ILogger<DeleteCommentModel> log)
        {
            _dbcontext = context;
            this.log = log;
        }

        [BindProperty]
        public Comment Comment { get; set; }

        public async Task<IActionResult> OnGetAsync(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }


            Comment = await _dbcontext.Comments.FirstOrDefaultAsync(m => m.ID == ID);

            if (Comment == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Comment = await _dbcontext.Comments.FindAsync(id);

            if (Comment != null)
            {
                _dbcontext.Comments.Remove(Comment);
                await _dbcontext.SaveChangesAsync();
                log.LogInformation("Comment deleted by {User}", User.Identity.Name);
                return RedirectToPage("./DeleteCommentSuccessfull");
            }
            return Page();
        }
    }
}
