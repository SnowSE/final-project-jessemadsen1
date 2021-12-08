using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using FinalProject;

namespace FinalProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Topic> Topics { get; set;}
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set;}
        public DbSet<SubComment> SubComments { get; set; }
        public DbSet<Author> Author { get; set; }

    }
}
