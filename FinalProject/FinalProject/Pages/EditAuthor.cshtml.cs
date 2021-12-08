using System.IO;
using FinalProject.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FinalProject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using FinalProject.Services;
using Microsoft.Extensions.Logging;


namespace FinalProject.Pages
{
    public class EditAuthorModel : PageModel
    {
        private readonly FinalProject.Data.ApplicationDbContext dbcontext;
        private readonly FinalProject.Services.IUserService userService;
        public const string AvatarFolder = "images/";
        private readonly ILogger<EditAuthorModel> log;

        public EditAuthorModel(FinalProject.Data.ApplicationDbContext context, IUserService userService, ILogger<EditAuthorModel> log)
        {
            dbcontext = context;
            this.userService = userService;
            this.log = log;
        }

        [BindProperty]
        public Author Author { get; set; }

        public string Name { get; set; }
        public string AvatarPath { get; set; }

        public IFormFile FormFile { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Name = User.Identity.Name;
            var userInfo = await userService.GetUserAsync(User.Identity.Name);
            if (userInfo?.AvatarFileName != null)
            {
                AvatarPath = Path.Combine(AvatarFolder, userInfo.AvatarFileName);
            }

            if (id == null)
            {
                return NotFound();
            }

            Author = await dbcontext.Author.FirstOrDefaultAsync(m => m.ID == id);

            if (Author == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var claim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            var currentUserName = claim.Value;
            Author.UserName = User.Identity.Name;
            Author.LastEditedon = System.DateTime.Now;


            if (!ModelState.IsValid)
            {
                return Page();
            }

            dbcontext.Attach(Author).State = EntityState.Modified;

            try
            {
                await dbcontext.SaveChangesAsync();
                await userService.SaveAvatarAsync(FormFile, User.Identity.Name);
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

            log.LogInformation("Author update/created: {Name}, Avatar file {avatar}", User.Identity.Name, Author.AvatarFileName);
            return Redirect("./Index");
        }

        private bool AuthorExists(int id)
        {
            return dbcontext.Author.Any(e => e.ID == id);
        }
    }
}
