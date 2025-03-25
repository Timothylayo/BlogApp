using BlogAppSharedProject.DTOS;
using BlogAppSharedProject.Models;
using System.Net.Http.Json;

namespace BlogApp.Client.Services
{
    public class ClientService(HttpClient httpClient) : ICategoryService, IBlogPostService
    {
        private const string CategoryBaseUrl = "api/category";
        private const string CategoryBySlugBaseUrl = "api/category/bySlug";
        private const string BlogPostBaseUrl = "api/blogpost";



        public Action? CategoryAction { get; set; }
        public Category[] categories { get; set; }
        public List<BlogPostDto> blogPosts { get; set; }

        public async Task<Category[]> GetAllCategories()
        {
            var response = await httpClient.GetFromJsonAsync<Category[]>(CategoryBaseUrl);

            CategoryAction?.Invoke();
            return response!;
        }

        public async Task<List<BlogPostDto>> GetBlogPostAsync(int pageIndex, int pageSize)
        {
            var response = await httpClient.GetFromJsonAsync<List<BlogPostDto>>($"{BlogPostBaseUrl}/getblogpost");
            return response!;
        }

        public async Task<DetailPageModel> GetBlogPostBySlugAsync(string slug)
        {
            var response = await httpClient.GetFromJsonAsync<DetailPageModel>($"{BlogPostBaseUrl}/blogpostbyslug");
            return response!;
        }
        public async Task<List<BlogPostDto>> GetFeaturedBlogPostAsync(int count)
        {
            var response = await httpClient.GetFromJsonAsync<List<BlogPostDto>>($"{BlogPostBaseUrl}/featuredblogpost");
            return response!;
        }

        public async Task<List<BlogPostDto>> GetPopularBlogPostAsync(int count)
        {
            var response = await httpClient.GetFromJsonAsync<List<BlogPostDto>>($"{BlogPostBaseUrl}/popularblogpost");
            return response!;
        }

        public async Task<List<BlogPostDto>> GetRecentBlogPostAsync(int count)
        {
            var response = await httpClient.GetFromJsonAsync<List<BlogPostDto>>($"{BlogPostBaseUrl}/recentblogpost");
            return response!;
        }

        public async Task<PaginationResponse<BlogPostDto>> GetBlogPost(int pageNumber, int pageSize)
        {
            var response = await httpClient.GetFromJsonAsync<PaginationResponse<BlogPostDto>>($"{BlogPostBaseUrl}/getblogpost/{pageNumber}/{pageSize}");

            return response!;
        }

        public Task GetCategoriesBySlug(string slug)
        {
            if (categories == null)
            {
                var response = httpClient.GetAsync(CategoryBySlugBaseUrl);

                CategoryAction?.Invoke();
            }
            return Task.FromResult(categories);

        }


    }
}
