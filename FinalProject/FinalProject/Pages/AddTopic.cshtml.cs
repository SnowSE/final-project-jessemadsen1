using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FinalProject.Pages
{
    [Authorize]
    public class AddTopicModel : PageModel
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly IAuthorizationService authorizationService;
        private readonly ILogger<IndexModel> log;

        public AddTopicModel(ApplicationDbContext dbContext, ILogger<IndexModel> log, IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
            this._dbContext = dbContext;
            this.log = log;
        }

        public async Task OnGetAsync(Channel channel)
        {
            var authResult = await authorizationService.AuthorizeAsync(User, AuthPolicies.IsAdmin);
            IsAdmin = authResult.Succeeded;

            Channel = channel;
            MyGlobalVariables.LastRoute = Request.Headers["Referer"].ToString();
        }

        [BindProperty]
        public Topic NewTopic { get; set; }
        [BindProperty]
        public Channel Channel { get; set; }
        public bool IsAdmin { get; private set;}

        public async Task<IActionResult> OnPostAsync()
        {
            NewTopic.Slug = NewTopic.Title.GenerateSlug();
            NewTopic.ChannelId = Channel.ID;
            if (ModelState.IsValid)
            {
                await _dbContext.Topics.AddAsync(NewTopic);
                await _dbContext.SaveChangesAsync();
                log.LogInformation("New Topic Created by: {AdminName} To {Channel}", User.Identity.Name, Channel.Title);
                return Redirect(MyGlobalVariables.LastRoute);
            }
            return Page();
        }
    }
}
