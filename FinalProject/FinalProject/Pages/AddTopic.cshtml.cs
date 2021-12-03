using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject.Pages
{
    public class AddTopicModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;


        public AddTopicModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult OnGet(Channel channel)
        {
            Channel = channel;
            ViewData["ChannelID"] = new SelectList(dbContext.Channels, "ID", "Title");
            MyGlobalVariables.LastRoute = Request.Headers["Referer"].ToString();
            return Page();
        }

        [BindProperty]
        public Topic NewTopic { get; set; }
        [BindProperty]
        public Channel Channel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            NewTopic.Slug = NewTopic.Title.GenerateSlug();
            NewTopic.ChannelId = Channel.ID;
            if (ModelState.IsValid)
            {
                await dbContext.Topics.AddAsync(NewTopic);
                await dbContext.SaveChangesAsync();
                return Redirect(MyGlobalVariables.LastRoute);
            }
            return Page();
        }
    }
}
