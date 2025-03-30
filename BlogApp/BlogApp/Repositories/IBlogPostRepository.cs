using BlogAppSharedProject.DTOS;
using BlogAppSharedProject.Models;

namespace BlogApp.Repositories
{
    public interface IBlogPostRepository
    {
        Task<BlogPostDto> GetBlogPostBySlugAsync(string slug);
        Task<List<BlogPostDto>> GetFeaturedBlogPostAsync(int count);
        Task<List<BlogPostDto>> GetPopularBlogPostAsync(int count);
        Task<List<BlogPostDto>> GetRecentBlogPostAsync(int count);
        Task<PaginationResponse<BlogPostDto>> GetBlogPostsAsync(int pageNumber, int pageSize);
        Task<List<BlogPostDto>> GetBlogPostByCategoryAsync(string categorySlug);
    }
}
