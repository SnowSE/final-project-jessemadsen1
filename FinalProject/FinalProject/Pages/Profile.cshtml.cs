using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FinalProject;
using FinalProject.Data;

namespace FinalProject.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly FinalProject.Data.ApplicationDbContext _context;

        public ProfileModel(FinalProject.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Author Author { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            if (User.Identity.Name == null)
            {
                return NotFound();
            }

            Author = await _context.Author.FirstOrDefaultAsync(m => m.UserName == User.Identity.Name);

            if (Author == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
