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

namespace FinalProject.Pages
{
    [Authorize]
    public class AddAuthorModel : PageModel
    {
        private readonly FinalProject.Data.ApplicationDbContext _context;
        private readonly FinalProject.Services.IUserService userService;
        public const string AvatarFolder = "images/avatars";
        public AddAuthorModel(FinalProject.Data.ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            this.userService = userService;
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

            await userService.SaveAvatarAsync(FormFile, User.Identity.Name);

            //_context.Author.Add(Author);
            //await _context.SaveChangesAsync();

            return RedirectToPage("./Setup");
        }
    }
}
