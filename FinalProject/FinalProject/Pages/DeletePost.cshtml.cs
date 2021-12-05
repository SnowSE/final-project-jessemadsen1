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
using Microsoft.Extensions.Logging;

namespace FinalProject.Pages
{
    public class DeletePostModel : PageModel
    {
        private readonly FinalProject.Data.ApplicationDbContext _dbcontext;
        private readonly IAuthorizationService authorizationService;
        private readonly ILogger<DeletePostModel> log;
        public DeletePostModel(FinalProject.Data.ApplicationDbContext context, IAuthorizationService authorizationService, ILogger<DeletePostModel> log)
        {
            _dbcontext = context;
            this.authorizationService = authorizationService;
            this.log = log;
        }

        public Post Post { get; set; }

        public async Task<IActionResult> OnGetAsync(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            Post = await _dbcontext.Posts.FirstOrDefaultAsync(m => m.ID == ID);

            if (Post == null)
            {
                return NotFound();
            }

            MyGlobalVariables.LastRoute = Request.Headers["Referer"].ToString();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? ID)
        {

            if (ID == null)
            {
                return NotFound();
            }

            Post = await _dbcontext.Posts.FirstOrDefaultAsync(m => m.ID == ID);

            if (Post != null)
            {
                _dbcontext.Posts.Remove(Post);
                await _dbcontext.SaveChangesAsync();
                log.LogInformation("{Post} deleted by {User}", Post.Title, User.Identity.Name);
                return Redirect(MyGlobalVariables.LastRoute);
            }
            return Page();
        }

    }

}
