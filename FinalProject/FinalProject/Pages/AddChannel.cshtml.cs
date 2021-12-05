using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FinalProject.Pages.Shared
{
    [Authorize]
    public class AddChannelModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAuthorizationService authorizationService;
        private readonly ILogger<AddChannelModel> log;
        public AddChannelModel(ApplicationDbContext dbContext, ILogger<AddChannelModel> log, IAuthorizationService authorizationService)
        {
            this._dbContext = dbContext;
            this.authorizationService = authorizationService;
            this.log = log;
        }
        public async Task OnGetAsync()
        {
            var authResult = await authorizationService.AuthorizeAsync(User, AuthPolicies.IsAdmin);
            IsAdmin = authResult.Succeeded;
        }

        [BindProperty]
        public Channel NewChannel { get; set; }
        public bool IsAdmin { get; private set; }
        public async Task<IActionResult> OnPostAsync()
        {
            NewChannel.Slug = NewChannel.Title.GenerateSlug();
            if (ModelState.IsValid)
            {
                await _dbContext.Channels.AddAsync(NewChannel);
                await _dbContext.SaveChangesAsync();
                log.LogInformation("New Channel Created by: {AdminName}", User.Identity.Name);
                return RedirectToPage("./Index");
            }
            return Page();
        }

    }
}
