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
    public class DeleteTopicModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<IndexModel> log;
        public DeleteTopicModel(ApplicationDbContext dbContext, ILogger<IndexModel> log)
        {
            this._dbContext = dbContext;
            this.log = log;
        }

        [BindProperty]
        public Topic Topic { get; set; }
        public Channel Channel { get; set; }
        public async Task<IActionResult> OnGetAsync(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            Topic = await _dbContext.Topics.FirstOrDefaultAsync(m => m.ID == ID);

            if (Topic == null)
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

            Topic = await _dbContext.Topics.FirstOrDefaultAsync(m => m.ID == ID);
            Channel = await _dbContext.Channels.FirstOrDefaultAsync(m => m.ID == Topic.ChannelId);
            if (Topic != null)
            {
                _dbContext.Topics.Remove(Topic);
                await _dbContext.SaveChangesAsync();
                log.LogInformation("{Topic} deleted by {AdminName}", Topic.Title, User.Identity.Name);
                return Redirect(MyGlobalVariables.LastRoute);
            }
            return Page();
        }

    }
}
