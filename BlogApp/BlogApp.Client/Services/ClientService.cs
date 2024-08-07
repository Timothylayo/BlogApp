using BlogAppSharedProject.Models;

namespace BlogApp.Client.Services
{
    public class ClientService(HttpClient httpClient) : ICategoryService
	{
        private const string CategoryBaseUrl = "api/category";
        private const string CategoryBySlugBaseUrl = "api/category/bySlug";
        private const string AuthenticationBaseUrl = "api/account";

        public Action? CategoryAction { get ; set ; }
        public Category[] categories { get; set; }

        public async Task GetAllCategories()
        {
            if(categories == null)
            {
                var response = await httpClient.GetAsync(CategoryBaseUrl);
                
                CategoryAction?.Invoke();
            }
        }

        public Task GetCategoriesBySlug(string slug)
        {
            if(categories == null)
            {
                var response = httpClient.GetAsync(CategoryBySlugBaseUrl);

                CategoryAction?.Invoke();
            }
            return Task.FromResult(categories);
            
        }

        
	}
}
