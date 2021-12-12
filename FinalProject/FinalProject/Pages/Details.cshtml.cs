using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FinalProject;
using FinalProject.Data;
using FinalProject.Pages.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace FinalProject.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly FinalProject.Data.ApplicationDbContext _dbcontext;
        private readonly IAuthorizationService authorizationService;
        private readonly ILogger<DetailsModel> log;

        public DetailsModel(FinalProject.Data.ApplicationDbContext context, IAuthorizationService authorizationService, ILogger<DetailsModel> log)
        {
            _dbcontext = context;
            this.authorizationService = authorizationService;
            this.log = log;
        }

        public Post Post { get; set; }

        public Comment Comment { get; set; }

        public bool CanEdit { get; private set; }
        public bool IsAdmin { get; private set; }

        public Author Author { get; set; }
        public List<Comment> Comments { get; set; }
        [BindProperty]
        public Vote Vote { get; set; }
        public AddCommentPartialModel AddCommentModel { get; set; } = new();

        public IEnumerable<Vote> Votes { get; set; }

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
            Author = await _dbcontext.Author
                 .FirstOrDefaultAsync(m => m.ID == Post.AuthorID);

            Comments = await _dbcontext.Comments
                    .Include( c => c.ChildComment)
                    .Include(c => c.ChildComment)
                    .Include(c => c.ChildComment)
                    .Include(c => c.ChildComment)
                    .Where(s => s.PostId == Post.ID)
                    .OrderBy(s => s.PostedOn)
                    .ToListAsync();

            Votes = await _dbcontext.Vote.ToListAsync();


            return Page();
        }
        public async Task<IActionResult> OnPostAsync(Comment NewComment, int ParentCommentId, String Child, String Slug)
        {
            var claim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            var currentUserName = claim.Value;
            Author = await _dbcontext.Author.FirstOrDefaultAsync(m => m.UserName == currentUserName);
            Post = await _dbcontext.Posts.FirstOrDefaultAsync(m => m.Slug == Child);
            NewComment.Author = Author.UserName;
            NewComment.AuthorID = Author.ID;
            NewComment.AvatarFileName = Author.AvatarFileName;
            NewComment.PostedOn = System.DateTime.Now;
            NewComment.PostId = Post.ID;

            if (ParentCommentId == 0)
            {
                NewComment.ParentCommentId = null;
            }
            else
            {
                NewComment.ParentCommentId = ParentCommentId;

            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _dbcontext.Comments.Add(NewComment);
            await _dbcontext.SaveChangesAsync();
            log.LogInformation("Vote added by {Name}", User.Identity.Name);
            return RedirectToPage(new { slug = Slug });
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
        public DisplayCommentPartialModel CommentNestedPartialModel(Comment Comment)
        {
            var model = new DisplayCommentPartialModel();
            model.Comment = Comment;
            model.Vote = Vote;
            return model;
        }
        public async Task<IActionResult> OnPostIlikeit(int Commentid, String Slug, String Parent, String Child)
        {
            var claim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            var currentUserName = claim.Value;
            var Vote2 = await _dbcontext.Vote.FirstOrDefaultAsync();
            Vote.Author = currentUserName;
            Vote.CommentId = Commentid;
            _dbcontext.Vote.Add(Vote);
            await _dbcontext.SaveChangesAsync();

            return RedirectToPage("./Details", Slug, Parent, Child);
        }

    public async Task<IActionResult> OnPostIdont(int commentid, String Slug, String Parent, String Child)
    {

            Vote = await _dbcontext.Vote
                .FirstOrDefaultAsync(m => (m.CommentId == commentid) && (m.Author == User.Identity.Name));

            if (Vote != null)
            {
                _dbcontext.Vote.Remove(Vote);
                await _dbcontext.SaveChangesAsync();
                return RedirectToPage("./Details", Slug, Parent ,Child );
            }
                return RedirectToPage("./Index");

        }

    }
}
