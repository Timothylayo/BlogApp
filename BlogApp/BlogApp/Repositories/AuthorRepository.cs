using BlogApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories
{
    public class AuthorRepository(ApplicationDbContext dbContext) : IAuthorRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task GetAuthorDetailsAsync(string userId)
        {
            var user = await dbContext.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
        }
    }
}
