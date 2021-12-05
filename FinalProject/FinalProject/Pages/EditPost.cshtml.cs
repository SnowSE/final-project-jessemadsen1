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

   public class EditPostModel : PageModel
   {
       private readonly ApplicationDbContext dbContext;
       private readonly ILogger<EditPostModel> log;
       public EditPostModel(ApplicationDbContext dbContext, ILogger<EditPostModel> log)
       {
           this.dbContext = dbContext;
           this.log = log;
       }

       [BindProperty]
       public Post Post { get; set; }

       public Topic Topic { get; set; }


        public async Task<IActionResult> OnGetAsync(int? ID)
       {
           if (ID == null)
           {
               return NotFound();
           }

           Post = await dbContext.Posts.FirstOrDefaultAsync(m => m.ID == ID);
           Topic = await dbContext.Topics.FirstOrDefaultAsync(m => m.ID == Post.TopicId);
           MyGlobalVariables.LastRoute = Request.Headers["Referer"].ToString();
           if (Post == null)
           {
               return NotFound();
           }
           return Page();
       }
       public async Task<IActionResult> OnPostAsync()
       {
            var claim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            var currentUserName = claim.Value;
            Post.Author = currentUserName;
            Post.PostedOn = System.DateTime.Now;
            Post.Slug = Post.Title.GenerateSlug();

            if (ModelState.IsValid)
           {
               dbContext.Attach(Post).State = EntityState.Modified;
               try
               {
                   await dbContext.SaveChangesAsync();
               }
               catch (DbUpdateConcurrencyException)
               {

                   throw;

               }
               log.LogInformation("Post Edited by {User}", User.Identity.Name);
               return Redirect(MyGlobalVariables.LastRoute);
           }
           return Page();
       }
   }
    
}
