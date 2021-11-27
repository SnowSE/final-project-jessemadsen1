using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public void OnGet()
        {
        }

        [BindProperty]
        public Post NewPost { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            NewPost.Slug = NewPost.Title.GenerateSlug();
            if (ModelState.IsValid)
            {
                await dbContext.Posts.AddAsync(NewPost);
                await dbContext.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            return Page();
        }
    }
}
