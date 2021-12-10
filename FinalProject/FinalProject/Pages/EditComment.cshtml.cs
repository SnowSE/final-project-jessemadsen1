using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject;
using FinalProject.Data;
using Microsoft.Extensions.Logging;

namespace FinalProject.Pages
{
    public class EditCommentModel : PageModel
    {
        private readonly FinalProject.Data.ApplicationDbContext _context;
        private readonly ILogger<EditCommentModel> log;

        public EditCommentModel(FinalProject.Data.ApplicationDbContext context, ILogger<EditCommentModel> log)
        {
            _context = context;
            this.log = log;
        }

        [BindProperty]
        public Comment Comment { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Comment = await _context.Comments
                .Include(c => c.Post).FirstOrDefaultAsync(m => m.ID == id);
            MyGlobalVariables.LastRoute = Request.Headers["Referer"].ToString();
            if (Comment == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var claim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
                var currentUserName = claim.Value;
                Comment.Author = currentUserName;
                Comment.PostedOn = System.DateTime.Now;

                if (!CommentExists(Comment.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            log.LogInformation("Comment Edited by {User}", User.Identity.Name);
            return Redirect(MyGlobalVariables.LastRoute);
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.ID == id);
        }
    }
}
