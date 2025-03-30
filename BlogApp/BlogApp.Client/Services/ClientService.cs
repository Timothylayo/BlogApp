using BlogAppSharedProject.DTOS;
using BlogAppSharedProject.Models;
using System.Net.Http.Json;

namespace BlogApp.Client.Services
{
    public class ClientService(HttpClient httpClient) : ICategoryService, IBlogPostService
    {
        private const string CategoryBaseUrl = "api/Category";
        private const string BlogPostBaseUrl = "api/blogpost";



        public Action? CategoryAction { get; set; }
        public List<BlogPostDto> blogPosts { get; set; }

        public async Task<List<Category>> GetAllCategories()
        {
            var response = await httpClient.GetFromJsonAsync<List<Category>>(CategoryBaseUrl);

            CategoryAction?.Invoke();
            return response!;
        }

        public async Task<List<BlogPostDto>> GetBlogPostAsync(int pageIndex, int pageSize)
        {
            var response = await httpClient.GetFromJsonAsync<List<BlogPostDto>>($"{BlogPostBaseUrl}/getblogpost");
            return response!;
        }

        public async Task<BlogPostDto> GetBlogPostBySlugAsync(string slug)
        {
            var response = await httpClient.GetAsync($"{BlogPostBaseUrl}/blogpostbyslug/{slug}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var apiResponse = await response.Content.ReadFromJsonAsync<BlogPostDto>();

            return apiResponse!;
        }
        public async Task<List<BlogPostDto>> GetBlogPostByCategoryAsync(string categorySlug)
        {
            var response = await httpClient.GetAsync($"{BlogPostBaseUrl}/blogpostbycategory/{categorySlug}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var apiResponse = await response.Content.ReadFromJsonAsync<List<BlogPostDto>>();

            return apiResponse!;
        }
        public async Task<List<BlogPostDto>> GetFeaturedBlogPostAsync(int count)
        {
            var response = await httpClient.GetAsync($"{BlogPostBaseUrl}/featuredblogpost/{count}");
            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var apiResponse = await response.Content.ReadFromJsonAsync<List<BlogPostDto>>();

            return apiResponse!;
        }

        public async Task<List<BlogPostDto>> GetPopularBlogPostAsync(int count)
        {
            var response = await httpClient.GetFromJsonAsync<List<BlogPostDto>>($"{BlogPostBaseUrl}/popularblogpost/{count}");
            return response!;
        }

        public async Task<List<BlogPostDto>> GetRecentBlogPostAsync(int count)
        {
            var response = await httpClient.GetFromJsonAsync<List<BlogPostDto>>($"{BlogPostBaseUrl}/recentblogpost/{count}");
            return response!;
        }

        public async Task<PaginationResponse<BlogPostDto>> GetBlogPost(int pageNumber, int pageSize)
        {
            var response = await httpClient.GetFromJsonAsync<PaginationResponse<BlogPostDto>>($"{BlogPostBaseUrl}/getblogpost/{pageNumber}/{pageSize}");

            return response!;
        }

        public async Task<Category> GetCategoriesBySlug(string slug)
        {
            var response = await httpClient.GetAsync($"{CategoryBaseUrl}/bySlug/{slug}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var apiResponse = await response.Content.ReadFromJsonAsync<Category>();
            if (apiResponse == null)
            {
                throw new Exception("Failed to parse the response or the response is null.");
            }

            return apiResponse!;
        }

    }
}
