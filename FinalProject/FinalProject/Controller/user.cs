using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class Users : ControllerBase
    {

        private readonly FinalProject.Data.ApplicationDbContext _dbcontext;

        public Users(FinalProject.Data.ApplicationDbContext context)
        {
            _dbcontext = context;

        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<Author> GetAsync(int id)
        {
            var Author = await _dbcontext.Author.FirstOrDefaultAsync(m => m.ID == id);
            return Author;
        }
    }
}
