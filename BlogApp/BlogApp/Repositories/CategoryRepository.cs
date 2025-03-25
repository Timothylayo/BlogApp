using BlogApp.Client.Services;
using BlogApp.Data;
using BlogAppSharedProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories
{
    public class CategoryRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : ICategoryRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory = contextFactory;

        private async Task<TResult> ExecuteOnContext<TResult>(Func<ApplicationDbContext, Task<TResult>> query)
        {
            using var context = _contextFactory.CreateDbContext();
            return await query.Invoke(context);
        }
        public async Task<Category[]> GetCategoryAsync()
        {
            return await ExecuteOnContext(async context =>
            {
                var categories = await context.Categories
                                        .AsNoTracking()
                                        .ToArrayAsync();
                return categories;
            });
        }
        public async Task<Category> SaveCategoryAsync(Category category)
        {
            return await ExecuteOnContext(async context =>
            {
                if (category.Id == 0)
                {
                    // it is a new category
                    if (await context.Categories
                                        .AsNoTracking()
                                        .AnyAsync(c => c.Name == category.Name))
                    {
                        throw new InvalidOperationException($"Category with the nam {category.Name} exists");
                    }
                    category.Slug = category.Name.ToSlug();
                    await context.Categories.AddAsync(category);

                }
                else
                {
                    //It is an existing category
                    if (await context.Categories
                                       .AsNoTracking()
                                       .AnyAsync(c => c.Name == category.Name && c.Id != category.Id))
                    {
                        throw new InvalidOperationException($"Category with the name {category.Name} exists");
                    }
                    var dbCategory = await context.Categories
                                    .FindAsync(category.Id);


                    dbCategory!.Name = category.Name;
                    dbCategory.ShowOnNavbar = category.ShowOnNavbar;

                    category.Slug = dbCategory!.Slug;

                }
                await context.SaveChangesAsync();

                return category;
            });
        }

        public async Task<Category?> GetCategoryBySlugAsync(string slug) =>
            await ExecuteOnContext(async context =>
                await context.Categories
                                .AsNoTracking()
                                .FirstOrDefaultAsync(c => c.Slug == slug)
            );
    }
}
