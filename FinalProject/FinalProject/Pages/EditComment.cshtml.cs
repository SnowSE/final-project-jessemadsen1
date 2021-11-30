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

namespace FinalProject.Pages
{
    public class EditCommentModel : PageModel
    {
        private readonly FinalProject.Data.ApplicationDbContext _context;

        public EditCommentModel(FinalProject.Data.ApplicationDbContext context)
        {
            _context = context;
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

            if (Comment == null)
            {
                return NotFound();
            }
           ViewData["PostId"] = new SelectList(_context.Posts, "ID", "Body");
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
                Comment.LastEditedon = System.DateTime.Now;

                if (!CommentExists(Comment.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.ID == id);
        }
    }
}
