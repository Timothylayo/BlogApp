using BlogApp.Data;
using BlogAppSharedProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories
{
    public class CategoryRepository(ApplicationDbContext dbContext) : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext = dbContext;

        public async Task<List<Category>> GetCategoryAsync()
        {
            var categories = await dbContext.Categories
                                        .AsNoTracking()
                                        .ToListAsync();

            return categories;
        }
        public async Task<Category> SaveCategoryAsync(Category category)
        {
            if (category.Id == 0)
            {
                // it is a new category
                if (await dbContext.Categories
                                    .AsNoTracking()
                                    .AnyAsync(c => c.Name == category.Name))
                {
                    throw new InvalidOperationException($"Category with the nam {category.Name} exists");
                }
                category.Slug = category.Name.ToSlug();
                await dbContext.Categories.AddAsync(category);

            }
            else
            {
                //It is an existing category
                if (await dbContext.Categories
                                    .AsNoTracking()
                                    .AnyAsync(c => c.Name == category.Name && c.Id != category.Id))
                {
                    throw new InvalidOperationException($"Category with the name {category.Name} exists");
                }
                var dbCategory = await dbContext.Categories
                                .FindAsync(category.Id);


                dbCategory!.Name = category.Name;
                dbCategory.ShowOnNavbar = category.ShowOnNavbar;

                category.Slug = dbCategory!.Slug;

            }
            await dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> GetCategoryBySlugAsync(string slug) =>
                await dbContext.Categories
                                .AsNoTracking()
                                .FirstOrDefaultAsync(c => c.Slug == slug);
    }
}
