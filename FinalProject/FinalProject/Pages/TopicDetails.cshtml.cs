using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Pages
{
    public class TopicDetailsModel : PageModel
    {
        private readonly FinalProject.Data.ApplicationDbContext _dbContext;
        private readonly IAuthorizationService authorizationService;

        public TopicDetailsModel(FinalProject.Data.ApplicationDbContext dbContext, IAuthorizationService authorizationService)
        {
            _dbContext = dbContext;
            this.authorizationService = authorizationService;
        }
        public IList<Topic> Topics { get; set; }
        public Channel Channel { get; set; }
        public bool CanEdit { get; private set; }
        public bool IsAdmin { get; private set; }

        public async Task<IActionResult> OnGetAsync(string slug)
        {
            var authResult = await authorizationService.AuthorizeAsync(User, AuthPolicies.IsAdmin);
            IsAdmin = authResult.Succeeded;

            if (slug == null)
            {
                return NotFound();
            }

            Channel = await _dbContext.Channels
                .Include(p => p.Topics)
                .FirstOrDefaultAsync(m => m.Slug.ToLower() == slug.ToLower());

            ViewData["BlogPostId"] = new SelectList(_dbContext.Channels, "ID", "Title");

            if (Channel == null)
            {
                return NotFound();
            }
            return Page();
        }



        //public async Task OnGetAsync()
        //{
        //    Topics = await _dbContext.Topics.ToListAsync();

        //    var authResult = await authorizationService.AuthorizeAsync(User, AuthPolicies.IsAdmin);
        //    IsAdmin = authResult.Succeeded;
        //}
    }
}
