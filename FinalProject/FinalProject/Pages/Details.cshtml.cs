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
        private readonly FinalProject.Data.ApplicationDbContext _dbcontext;
        private readonly IAuthorizationService authorizationService;

        public DetailsModel(FinalProject.Data.ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _dbcontext = context;
            this.authorizationService = authorizationService;
        }

        [BindProperty]
        public Post Post { get; set; }

        [BindProperty]
        public Comment Comment { get; set; }

        public bool CanEdit { get; private set; }
        public bool IsAdmin { get; private set; }

        public Author Author { get; set; }
        public async Task<IActionResult> OnGetAsync(string child)
        {
            var authResult = await authorizationService.AuthorizeAsync(User, AuthPolicies.IsAdmin);
            IsAdmin = authResult.Succeeded;

            if (child == null)
            {
                return NotFound();
            }

            Post = await _dbcontext.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(m => m.Slug.ToLower() == child.ToLower());

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
            Author = await _dbcontext.Author.FirstOrDefaultAsync(m => m.UserName == currentUserName);
            Comment.Author = Author.UserName;
            Comment.AvatarFileName = Author.AvatarFileName;
            Comment.PostedOn = System.DateTime.Now;
            Comment.PostId = Post.ID;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _dbcontext.Comments.Add(Comment);
            await _dbcontext.SaveChangesAsync();

            return RedirectToPage(new { slug = slug });
        }

        public async Task<IActionResult> OnPostAddComment(string slug)
        {
            if (slug == null)
            {
                return NotFound();
            }

            Post = await _dbcontext.Posts
                .FirstOrDefaultAsync(m => m.Slug.ToLower() == slug.ToLower());


            if (Post == null)
            {
                return NotFound();
            }

            return RedirectToPage("./AddComment", Post);
        }
    }
}
