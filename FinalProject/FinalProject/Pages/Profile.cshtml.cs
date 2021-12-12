using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FinalProject;
using FinalProject.Data;
using Microsoft.AspNetCore.Authorization;

namespace FinalProject.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly FinalProject.Data.ApplicationDbContext _dbContext;
        private readonly IAuthorizationService authorizationService;
        public ProfileModel(FinalProject.Data.ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _dbContext = context;
            this.authorizationService = authorizationService;
        }

        public Author Author { get; set; }
        public Topic Topic { get; set; }
        public bool IsAdmin { get; private set; }
        public List<Post> PostList = new List<Post>();
        public async Task<IActionResult> OnGetAsync()
        {
            var authResult = await authorizationService.AuthorizeAsync(User, AuthPolicies.IsAdmin);
            IsAdmin = authResult.Succeeded;

            if (User.Identity.Name == null)
            {
                return NotFound();
            }

            Author = await _dbContext.Author.FirstOrDefaultAsync(m => m.UserName == User.Identity.Name);

            if (Author == null)
            {
                return RedirectToPage("./AddAuthor");
            }
            PostList =  await _dbContext.Posts
                   .Where(m => m.AuthorID == Author.ID).ToListAsync();

            return Page();
        }

        public Post Post { get; set; }

        public Channel Channel { get; set; }
        public async Task<IActionResult> OnPostRedirect(string child)
        {
            Post = await _dbContext.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(m => m.Slug.ToLower() == child.ToLower());
            Topic = await _dbContext.Topics
                    .FirstOrDefaultAsync(m => m.ID == Post.TopicId);
            Channel = await _dbContext.Channels
                    .FirstOrDefaultAsync(m => m.ID == Topic.ChannelId);


            return RedirectToPage("./TopicDetails", Channel.Slug);
        }
    }
}

