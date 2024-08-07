using BlogApp.Data;
using BlogAppSharedProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories
{
    public class CategoryRepository(ApplicationDbContext applicationDb) : ICategoryRepository
	{
		private readonly ApplicationDbContext _applicationDb = applicationDb;

		public async Task<Category[]> GetCategoryAsync()
        {
            var category = await _applicationDb.Categories.AsNoTracking().ToArrayAsync();
			return category;
        }

		public async Task<Category> SaveCategoryAsync(Category category)
		{
			if (category.Id == 0)
			{
				//New Category
				if (await _applicationDb.Categories.AsNoTracking()
                                                        .AnyAsync(b => b.Name == b.Name))
				{
					throw new InvalidOperationException("Category Exists");
				}
				category.Slug = category.Name.ToSlug();
				await _applicationDb.Categories.AddAsync(category);
			}
			else
			{
				//New Category
				if (await _applicationDb.Categories.AsNoTracking()
                                                        .AnyAsync(b => b.Name == category.Name && b.Id != category.Id))
				{
					throw new InvalidOperationException($"Category with name {category.Name} Exists");
				}
				var findCategory = await _applicationDb.Categories.FindAsync(category.Id);

				findCategory!.Name = category.Name.ToSlug();
				findCategory.ShowOnNavbar = category.ShowOnNavbar;
				category.Slug = findCategory.Slug;

			}
			await _applicationDb.SaveChangesAsync();
			return category;
		}

		public async Task<Category?> GetCategoryBySlugAsync(string slug) => await _applicationDb.Categories.AsNoTracking()
																		.FirstOrDefaultAsync(c => c.Slug == slug);
	}
}
