using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FinalProject;
using FinalProject.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace FinalProject.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly FinalProject.Data.ApplicationDbContext _context;
        private readonly IAuthorizationService authorizationService;

        public DetailsModel(FinalProject.Data.ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            this.authorizationService = authorizationService;
        }

        public Post Post { get; set; }

        [BindProperty]
        public Comment Comment { get; set; }

        public bool CanEdit { get; private set; }
        public bool IsAdmin { get; private set; }

        public async Task<IActionResult> OnGetAsync(string child)
        {
            var authResult = await authorizationService.AuthorizeAsync(User, AuthPolicies.IsAdmin);
            IsAdmin = authResult.Succeeded;

            if (child == null)
            {
                return NotFound();
            }

            Post = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(m => m.Slug.ToLower() == child.ToLower());

            ViewData["PostId"] = new SelectList(_context.Posts, "ID", "Title");

            if (child == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int commentId, string slug)
        {
            var claim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            var currentUserName = claim.Value;
            Comment.Author = currentUserName;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Comments.Add(Comment);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { slug = slug });
        }
        public async Task<IActionResult> OnPostHideCommentAsync(int commentid, string slug)
        {
            var comment = await _context.Comments.FindAsync(commentid);

            if (comment != null)
            {
                comment.HideComment = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage(new { slug = slug });
        }
        public async Task<IActionResult> OnPostDeleteCommentAsync(int commentid)
        {
            var comment = await _context.Comments.FindAsync(commentid);
            return RedirectToPage("./DeleteComment", new {id = commentid});
        }

        public async Task<IActionResult> OnPostEditCommentAsync(int commentid)
        {
            var comment = await _context.Comments.FindAsync(commentid);
            return RedirectToPage("./EditComment", new { id = commentid });
        }
        //private bool BlogExists(int id)
        //{
        //    return _context.Posts.Any(e => e.ID == id);
        //}
    }
}
