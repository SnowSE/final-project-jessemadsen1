using System.IO;
using FinalProject.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using FinalProject.Pages;


namespace FinalProject.Services
{
    public class DefaultUserService : IUserService
    {
        private readonly ApplicationDbContext dbContext;

        public DefaultUserService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Author> GetUserAsync(string Name)
        {

            var userInfo = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == Name);
            var userInfo2 = new Author { UserName = Name };
            userInfo2.UserName = Name;
            if (userInfo == null)
            {
                dbContext.Author.Add(userInfo2);

            }

            return userInfo2;
        }

        public async Task SaveAvatarAsync(IFormFile formFile, string name)
        {
            var avatarFileName = name + Path.GetExtension(formFile.FileName);
            if (!Directory.Exists(AddAuthorModel.AvatarFolder))
            {
                Directory.CreateDirectory(AddAuthorModel.AvatarFolder);
            }

            string fullAvatarPath = Path.Combine("wwwroot", AddAuthorModel.AvatarFolder, avatarFileName);
            using var stream = System.IO.File.OpenWrite(fullAvatarPath);
            await formFile.CopyToAsync(stream);
            stream.Close();

            var userlnfo = await GetUserAsync(name);
            userlnfo.AvatarFileName = avatarFileName; 

            dbContext.Author.Add(userlnfo);
            await dbContext.SaveChangesAsync();
        }
    }
}
