using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalProject.Pages
{
    public class AddTopicModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        public AddTopicModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
        }

        [BindProperty]
        public Topic NewTopic { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            NewTopic.Slug = NewTopic.Title.GenerateSlug();
            if (ModelState.IsValid)
            {
                await dbContext.Topics.AddAsync(NewTopic);
                await dbContext.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            return Page();
        }
    }
}
