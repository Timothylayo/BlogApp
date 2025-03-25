using BlogAppSharedProject.Models;

namespace BlogApp.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category[]> GetCategoryAsync();
        Task<Category?> GetCategoryBySlugAsync(string slug);
        Task<Category> SaveCategoryAsync(Category category);
    }
}