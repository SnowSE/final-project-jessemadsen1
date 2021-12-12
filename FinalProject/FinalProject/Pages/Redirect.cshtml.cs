using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FinalProject;
using FinalProject.Data;
using FinalProject.Pages.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace FinalProject.Pages
{
    public class RedirectModel : PageModel
    {
        private readonly FinalProject.Data.ApplicationDbContext _dbcontext;
        private readonly ILogger<RedirectModel> log;
        public RedirectModel(FinalProject.Data.ApplicationDbContext context, IAuthorizationService authorizationService, ILogger<RedirectModel> log)
        {
            _dbcontext = context;
            this.log = log;
        }
        public Post Post { get; set; }
        public Topic Topic { get; set; }
        public Channel Channel { get; set; }
        public async Task<IActionResult> OnGetAsync(string child)
        {
            Post = await _dbcontext.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(m => m.Slug.ToLower() == child.ToLower());
            Topic = await _dbcontext.Topics
                    .FirstOrDefaultAsync(m => m.ID == Post.TopicId);
            Channel = await _dbcontext.Channels
                    .FirstOrDefaultAsync(m => m.ID == Topic.ChannelId);
            Response.Redirect(string.Format("./Details", Channel.Slug, Topic.Slug, Post.Slug));

            return RedirectToPage("./TopicDetails", Channel.Slug );
        }
    }
}
