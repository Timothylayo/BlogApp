using BlogApp.Data.Entities;
using BlogAppSharedProject.DTOS;
using BlogAppSharedProject.Models;

namespace BlogApp.Repositories
{
    public interface IBlogPostRepository
    {
        Task<DetailPageModel> GetBlogPostBySlugAsync(string slug);
        Task<List<BlogPostDto>> GetFeaturedBlogPostAsync(int count);
        Task<List<BlogPostDto>> GetPopularBlogPostAsync(int count);
        Task<List<BlogPostDto>> GetRecentBlogPostAsync(int count);
        Task<PaginationResponse<BlogPostDto>> GetBlogPostsAsync(int pageNumber, int pageSize);
    }
}
