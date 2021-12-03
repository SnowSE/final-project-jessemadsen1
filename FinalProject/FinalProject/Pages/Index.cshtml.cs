using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly FinalProject.Data.ApplicationDbContext _dbContext;
        private readonly IAuthorizationService authorizationService;
        private readonly ILogger<IndexModel> log;

        public IndexModel(FinalProject.Data.ApplicationDbContext dbContext, IAuthorizationService authorizationService)
        {
            _dbContext = dbContext;
            this.authorizationService = authorizationService;
        }
        public IList<Channel> Channels { get; set; }
        public bool CanEdit { get; private set; }
        public bool IsAdmin { get; private set; }

        [BindProperty]
        public string ParentName { get; set; }

        public async Task OnGetAsync()
        {
            Channels = await _dbContext.Channels.ToListAsync();

            var authResult = await authorizationService.AuthorizeAsync(User, AuthPolicies.IsAdmin);
            IsAdmin = authResult.Succeeded;
        }
        public IActionResult OnPostAddParent()
        {
            //log.LogInformation("Adding new parent: {parent}", ParentName);
            //itemManager.TopLevelItems.Add(new ParentItem { Name = ParentName });
            return RedirectToPage();
        }

    }
}
