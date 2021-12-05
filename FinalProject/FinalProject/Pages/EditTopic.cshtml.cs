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
    public class EditTopicModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<EditTopicModel> log;
        public EditTopicModel(ApplicationDbContext dbContext, ILogger<EditTopicModel> log)
        {
            this.dbContext = dbContext;
            this.log = log;
        }

        [BindProperty]
        public Topic Topic { get; set; }


        public async Task<IActionResult> OnGetAsync(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            Topic = await dbContext.Topics.FirstOrDefaultAsync(m => m.ID == ID);
            MyGlobalVariables.LastRoute = Request.Headers["Referer"].ToString();
            if (Topic == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Topic.Slug = Topic.Title.GenerateSlug();
            if (ModelState.IsValid)
            {
                dbContext.Attach(Topic).State = EntityState.Modified;
                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
                log.LogInformation("Edited Topic by {AdminName}", User.Identity.Name);
                return Redirect(MyGlobalVariables.LastRoute);
            }
            return Page();
        }
    }
}
