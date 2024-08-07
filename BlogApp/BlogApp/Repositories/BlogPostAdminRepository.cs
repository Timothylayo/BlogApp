using BlogApp.Data;
using BlogApp.Data.Entities;
using BlogAppSharedProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories
{
    public class BlogPostAdminRepository(ApplicationDbContext applicationDb) : IBlogPostAdminRepository
	{
		private readonly ApplicationDbContext _applicationDbContext = applicationDb;

		public async Task<PagedResult<BlogPost>> GetBlogPostAsync(int startIndex, int pageSize)
		{
			var query = _applicationDbContext.BlogPosts.AsNoTracking();

			var count = query.Count();

			var blogPosts = await query.Include(b => b.Category)
										.OrderByDescending(b => b.Id)
										.Skip(startIndex)
										.Take(pageSize)
										.ToArrayAsync();

			return new PagedResult<BlogPost>(blogPosts, count);
		}

		public async Task<BlogPost?> GetBlogPostByIdAsync(int id)
		{
			var blogPosts = await _applicationDbContext.BlogPosts.AsNoTracking()
														.Include(b => b.Category)
														.FirstOrDefaultAsync(b => b.Id == id);
			return blogPosts;
		}

		private async Task<string> GenerateSlugAsync(BlogPost blogPost)
		{

			string originalSlug = blogPost.Title.ToSlug();

			string slug = originalSlug;
			int count = 1;

			while (await _applicationDbContext.BlogPosts.AsNoTracking().AnyAsync(b => b.Slug == slug))
			{
				slug = $"{originalSlug} - {count++}";
			}
			return slug;

		}

		public async Task<BlogPost> SaveBlogPostAsync(BlogPost blogPost, string userId)
		{
			if (blogPost.Id == 0)
			{
				//New blogpost
				//Check if it is a duplicate title
				var isDuplicateTitle = await _applicationDbContext.BlogPosts.AsNoTracking().AnyAsync(b => b.Title == blogPost.Title);
				if (isDuplicateTitle)
				{
					throw new InvalidOperationException($"Blog post Title exists");
				}
				blogPost.Slug = await GenerateSlugAsync(blogPost);
				blogPost.CreatedAt = DateTime.UtcNow;
				blogPost.UserId = userId;

				if (blogPost.IsPublished)
				{
					blogPost.PublishedAt = DateTime.UtcNow;
				}
				await _applicationDbContext.AddAsync(blogPost);
			}
			else
			{
				// for existing blogpost
				var isDuplicateTitle = await _applicationDbContext.BlogPosts.AsNoTracking()
					.AnyAsync(b => b.Title == blogPost.Title && b.Id != blogPost.Id);

				if (isDuplicateTitle)
				{
					throw new InvalidOperationException($"Blog post Title exists");
				}

				var findBlog = await _applicationDbContext.BlogPosts.FindAsync(blogPost.Id);

				findBlog!.Title = blogPost.Title;
				findBlog.Image = blogPost.Image;
				findBlog.introduction = blogPost.introduction;
				findBlog.Content = blogPost.Content;
				findBlog.CategoryId = blogPost.CategoryId;
				findBlog.PublishedAt = blogPost.PublishedAt;
				findBlog.IsPublished = blogPost.IsPublished;
				findBlog.IsFeatured = blogPost.IsFeatured;

				if (blogPost.IsPublished)
				{
					if (!findBlog.IsPublished)
					{
						blogPost.PublishedAt = DateTime.UtcNow;
					}
				}
				else
				{
					blogPost.PublishedAt = null;
				}

			}
			await _applicationDbContext.SaveChangesAsync();
			return blogPost;
		}

	}
}
