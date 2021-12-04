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
    public class DeleteChannelModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<IndexModel> log;
        public DeleteChannelModel(ApplicationDbContext dbContext, ILogger<IndexModel> log)
        {
            this._dbContext = dbContext;
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

            Channel = await _dbContext.Channels.FirstOrDefaultAsync(m => m.ID == ID);

            if (Channel == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? ID)
        {

            if (ID == null)
            {
                return NotFound();
            }

            Channel = await _dbContext.Channels.FirstOrDefaultAsync(m => m.ID == ID);

            if (Channel != null)
            {
                _dbContext.Channels.Remove(Channel);
                await _dbContext.SaveChangesAsync();
                log.LogInformation("Channel deleted by {AdminName}", User.Identity.Name);
                return RedirectToPage("./Index");
            }
            return Page();
        }

    }
}
