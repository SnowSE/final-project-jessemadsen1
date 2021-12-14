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
    [Authorize]
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
        public List<Comment> CommentList = new List<Comment>();
        public IEnumerable<Vote> Votes { get; set; }
        public int CountVotes { get; private set; }
        public async Task<IActionResult> OnGetAsync(string profileName)
        {
            var authResult = await authorizationService.AuthorizeAsync(User, AuthPolicies.IsAdmin);
            IsAdmin = authResult.Succeeded;

            if (User.Identity.Name == null)
            {
                return NotFound();
            }

            Author = await _dbContext.Author.FirstOrDefaultAsync(m => m.UserName == profileName);

            if (Author == null)
            {
                return RedirectToPage("./AddAuthor");
            }
            PostList =  await _dbContext.Posts
                   .Where(m => m.AuthorID == Author.ID).ToListAsync();

            CommentList = await _dbContext.Comments
                    .Where(m => m.AuthorID == Author.ID).ToListAsync();

            Votes = await _dbContext.Vote
                .Where(v => v.Author == Author.UserName)
                .ToListAsync();

            CountVotes = Votes.Count();
            Author.VoteTotal = CountVotes;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _dbContext.Attach(Author).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(Author.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

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
         private bool AuthorExists(int id)
         {
             return _dbContext.Author.Any(e => e.ID == id);
         }
}
    }

