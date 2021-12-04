using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalProject.Pages.Shared
{
    [Authorize]
    public class AddChannelModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        public AddChannelModel(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public void OnGet()
        {
        }

        [BindProperty]
        public Channel NewChannel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            NewChannel.Slug = NewChannel.Title.GenerateSlug();
            if (ModelState.IsValid)
            {
                await _dbContext.Channels.AddAsync(NewChannel);
                await _dbContext.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            return Page();
        }

    }
}
