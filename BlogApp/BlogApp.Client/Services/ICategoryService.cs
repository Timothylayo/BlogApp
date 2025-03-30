using BlogAppSharedProject.Models;

namespace BlogApp.Client.Services
{
    public interface ICategoryService
    {
        Action? CategoryAction { get; set; }
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategoriesBySlug(string slug);
    }
}
