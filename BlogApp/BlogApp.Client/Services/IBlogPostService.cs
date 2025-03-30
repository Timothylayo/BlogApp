using BlogAppSharedProject.DTOS;
using BlogAppSharedProject.Models;

namespace BlogApp.Client.Services
{
    public interface IBlogPostService
    {
        public List<BlogPostDto> blogPosts { get; set; }
        Task<BlogPostDto> GetBlogPostBySlugAsync(string slug);
        Task<List<BlogPostDto>> GetFeaturedBlogPostAsync(int count);
        Task<List<BlogPostDto>> GetPopularBlogPostAsync(int count);
        Task<List<BlogPostDto>> GetRecentBlogPostAsync(int count);
        Task<PaginationResponse<BlogPostDto>> GetBlogPost(int pageNumber, int pageSize);
        Task<List<BlogPostDto>> GetBlogPostByCategoryAsync(string categorySlug);


    }
}
