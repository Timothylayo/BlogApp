using BlogAppSharedProject.Models;

namespace BlogApp.Client.Services
{
    public interface ICategoryService
    {
        Action? CategoryAction { get; set; }
        

        Task<Category[]> GetAllCategories();

        Task GetCategoriesBySlug(string slug);

        Category[] categories { get; set; }
    }
}
