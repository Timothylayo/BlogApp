using BlogApp.Data.Entities;
using BlogAppSharedProject.Models;

namespace BlogApp.Repositories
{
    public interface IBlogPostAdminRepository
	{
		Task<PagedResult<BlogPost>> GetBlogPostAsync(int startIndex, int pageSize);
		Task<BlogPost?> GetBlogPostByIdAsync(int id);
		Task<BlogPost> SaveBlogPostAsync(BlogPost blogPost, string userId);
	}
}