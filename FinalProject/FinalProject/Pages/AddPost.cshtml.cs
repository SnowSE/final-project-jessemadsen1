using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject.Pages
{
    [Authorize]
    public class AddPostModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        public AddPostModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [BindProperty]
        public Post NewPost { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var claim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            var currentUserName = claim.Value;
            NewPost.Author = currentUserName;
            NewPost.PostedOn = System.DateTime.Now;

            NewPost.Slug = NewPost.Title.GenerateSlug();

            if (ModelState.IsValid)
            {
                await dbContext.Posts.AddAsync(NewPost);
                await dbContext.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            return Page();
        }
        public IActionResult OnGet()
        {
            ViewData["TopicID"] = new SelectList(dbContext.Topics, "ID", "Title");
            return Page();
        }
    }
}
