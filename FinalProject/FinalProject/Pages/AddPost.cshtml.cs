using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FinalProject.Pages
{
    [Authorize]
    public class AddPostModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<AddPostModel> log;
        public AddPostModel(ApplicationDbContext dbContext, ILogger<AddPostModel> log)
        {
            this.dbContext = dbContext;
            this.log = log;
        }


        [BindProperty]
        public Post NewPost { get; set; }
        [BindProperty]
        public Topic Topic { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            var claim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            var currentUserName = claim.Value;
            NewPost.Author = currentUserName;
            NewPost.PostedOn = System.DateTime.Now;
            NewPost.TopicId = Topic.ID;
            NewPost.Slug = NewPost.Title.GenerateSlug();
            

            if (ModelState.IsValid)
            {
                await dbContext.Posts.AddAsync(NewPost);
                await dbContext.SaveChangesAsync();
                log.LogInformation("New Post Created by: {Name} To {Topic}", User.Identity.Name, Topic.Title);
                return Redirect(MyGlobalVariables.LastRoute);
            }
            return Page();
        }
        public void OnGet(Topic topic)
        {
            Topic = topic;
            MyGlobalVariables.LastRoute = Request.Headers["Referer"].ToString();
        }
    }
}
