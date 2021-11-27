using System;
using System.Collections.Generic;
using FinalProject;
using FinalProject.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace FinalProject.Pages
{
    public class PostDetailsModel : PageModel
    {

        private readonly FinalProject.Data.ApplicationDbContext _dbContext;
        private readonly IAuthorizationService authorizationService;

        public PostDetailsModel(FinalProject.Data.ApplicationDbContext context, IAuthorizationService authorizationService)
            {
                _dbContext = context;
                this.authorizationService = authorizationService;
            }

        public IList<Post> Posts { get; set; }

        [BindProperty]
            public Comment Comment { get; set; }

            public bool CanEdit { get; private set; }
            public bool IsAdmin { get; private set; }

            public async Task<IActionResult> OnGetAsync(string slug)
            {
                var authResult = await authorizationService.AuthorizeAsync(User, AuthPolicies.IsAdmin);
                IsAdmin = authResult.Succeeded;

                if (slug == null)
                {
                    return NotFound();
                }

            Posts = await _dbContext.Posts.ToListAsync();

            ViewData["PostId"] = new SelectList(_dbContext.Posts, "ID", "Title");

                if (slug == null)
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

                _dbContext.Comments.Add(Comment);
                await _dbContext.SaveChangesAsync();

                return RedirectToPage(new { slug = slug });
            }
            public async Task<IActionResult> OnPostHideCommentAsync(int commentid, string slug)
            {
                var comment = await _dbContext.Comments.FindAsync(commentid);

                if (comment != null)
                {
                    comment.HideComment = true;
                    await _dbContext.SaveChangesAsync();
                }
                return RedirectToPage(new { slug = slug });
            }
            public async Task<IActionResult> OnPostDeleteCommentAsync(int commentid)
            {
                var comment = await _dbContext.Comments.FindAsync(commentid);
                return RedirectToPage("./DeleteComment", new { id = commentid });
            }

            public async Task<IActionResult> OnPostEditCommentAsync(int commentid)
            {
                var comment = await _dbContext.Comments.FindAsync(commentid);
                return RedirectToPage("./EditComment", new { id = commentid });
            }
            private bool PostExists(int id)
            {
                return _dbContext.Posts.Any(e => e.ID == id);
            }
        }
    
}
