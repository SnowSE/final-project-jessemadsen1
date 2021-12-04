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
    public class EditChannelModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<IndexModel> log;
        public EditChannelModel(ApplicationDbContext dbContext, ILogger<IndexModel> log)
        {
            this.dbContext = dbContext;
            this.log = log;
        }

        [BindProperty]
        public Channel Channel { get; set; }
        public async Task<IActionResult> OnGetAsync(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            Channel = await dbContext.Channels.FirstOrDefaultAsync(m => m.ID == ID);

            if (Channel == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Channel.Slug = Channel.Title.GenerateSlug();
            if (ModelState.IsValid)
            {
                dbContext.Attach(Channel).State = EntityState.Modified;
                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
                log.LogInformation("Edited Channel by {AdminName}", User.Identity.Name);
                return RedirectToPage("./Index");
            }
            return Page();
        }
    }
}
