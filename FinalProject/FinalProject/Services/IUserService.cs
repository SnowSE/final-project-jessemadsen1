using FinalProject.Data;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    public interface IUserService
    {
        Task<Author> GetUserAsync(string Name);
        Task SaveAvatarAsync(IFormFile formFile, string name);
    }
}