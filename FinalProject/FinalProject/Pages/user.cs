using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace FinalProject.Pages
{
    [Route("api/[controller]")]
    [ApiController]
    public class User : ControllerBase
    {

        private readonly FinalProject.Data.ApplicationDbContext _dbcontext;
        public Author author { get; set; }
        public User(FinalProject.Data.ApplicationDbContext context)
        {
            _dbcontext = context;

        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<string> GetAsync(int id)
        {
            author = await _dbcontext.Author.FirstOrDefaultAsync(m => m.ID == id);

            return JsonSerializer.Serialize(author);

        }
    }
}