using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace FinalProject.Pages
{
    public class AddCommentModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AddPostModel> log;


        public AddCommentModel(ApplicationDbContext dbContext, ILogger<AddPostModel> log)
        {
            this._dbContext = dbContext;
            this.log = log;
        }


        [BindProperty]
        public Comment Comment { get; set; }
        [BindProperty]
        public Post Post { get; set; }
        public Author Author { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var claim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            var currentUserName = claim.Value;
            Author = await _dbContext.Author.FirstOrDefaultAsync(m => m.UserName == currentUserName);
            Comment.Author = Author.UserName;
            Comment.AvatarFileName = Author.AvatarFileName;
            Comment.PostedOn = System.DateTime.Now;
            Comment.PostId = Post.ID;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _dbContext.Comments.Add(Comment);
            await _dbContext.SaveChangesAsync();

            //return RedirectToPage("./Details",new { slug = MyGlobalVariables.LastRoute.Split('/').Last() });
            return RedirectToPage("./CommentAddRedirect");
        }
        public async Task OnGetAsync(int ID)
        {
            Post = await _dbContext.Posts.FirstOrDefaultAsync(m => m.ID == ID);

            MyGlobalVariables.LastRoute = Request.Headers["Referer"].ToString();
        }
    }
}
