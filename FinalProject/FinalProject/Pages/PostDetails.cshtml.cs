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

        //public IList<Post> Posts { get; set; }

        [BindProperty]
        public Comment Comment { get; set; }

        public bool CanEdit { get; private set; }
        public bool IsAdmin { get; private set; }

        public Topic Topic { get; set; }
        public Channel Channel { get; set; }
        public async Task<IActionResult> OnGetAsync(string child)
        {
            var authResult = await authorizationService.AuthorizeAsync(User, AuthPolicies.IsAdmin);
            IsAdmin = authResult.Succeeded;

            if (child == null)
            {
                return NotFound();
            }

            Topic = await _dbContext.Topics
                .Include(p => p.Posts)
                .FirstOrDefaultAsync(m => m.Slug.ToLower() == child.ToLower());
            Channel = await _dbContext.Channels
                .Include(p => p.Topics)
                .FirstOrDefaultAsync(m => m.ID == Topic.ChannelId);


            if (Topic == null)
            {
                return NotFound();
            }
            return Page();
        }


        public async Task<IActionResult> OnPostAddPost(string slug)
        {
            if (slug == null)
            {
                return NotFound();
            }

            Topic = await _dbContext.Topics
                .FirstOrDefaultAsync(m => m.Slug.ToLower() == slug.ToLower());

            if (Topic == null)
            {
                return NotFound();
            }

            return RedirectToPage("./AddPost", Topic);
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

        }
    
}
