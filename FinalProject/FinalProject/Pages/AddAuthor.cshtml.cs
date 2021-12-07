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
    [Authorize]
    public class AddAuthorModel : PageModel
    {
        private readonly FinalProject.Data.ApplicationDbContext dbcontext;
        private readonly FinalProject.Services.IUserService userService;
        public const string AvatarFolder = "images/";
        private readonly ILogger<AddAuthorModel> log;
        public AddAuthorModel(FinalProject.Data.ApplicationDbContext context, IUserService userService, ILogger<AddAuthorModel> log)
        {
            dbcontext = context;
            this.userService = userService;
            this.log = log;
        }

        public async Task OnGet()
        {
            Name = User.Identity.Name;
            var userInfo = await userService.GetUserAsync(User.Identity.Name);
            if (userInfo?.AvatarFileName != null)
            {
                AvatarPath = Path.Combine(AvatarFolder, userInfo.AvatarFileName);
            }

        }

        [BindProperty]
        public Author Author { get; set; }

        public string Name { get; set; }
        public string AvatarPath { get; set; }

        public IFormFile FormFile { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {

            Author.UserName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                await dbcontext.Author.AddAsync(Author);
                await dbcontext.SaveChangesAsync();
                await userService.SaveAvatarAsync(FormFile, User.Identity.Name);
                log.LogInformation("Author update/created: {Name}, Avatar file {advatar}", User.Identity.Name, Author.AvatarFileName);
                return Redirect("./Index");
            }
            return Page();
        }
    }
}
