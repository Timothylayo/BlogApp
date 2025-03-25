using BlogApp.Data;
using BlogApp.Data.Entities;
using BlogAppSharedProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories
{
    public class BlogPostAdminRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IBlogPostAdminRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory = contextFactory;

        private async Task<TResult> ExecuteOnContext<TResult>(Func<ApplicationDbContext, Task<TResult>> query)
        {
            using var context = _contextFactory.CreateDbContext();
            return await query.Invoke(context);
        }


        public async Task<PagedResult<BlogPost>> GetBlogPostAsync(int startIndex, int pageSize)
        {
            return await ExecuteOnContext(async context =>
            {
                var query = context.BlogPosts
                                    .AsNoTracking();

                var count = await query.CountAsync();

                var records = await query.Include(b => b.Category)
                                        .OrderByDescending(b => b.Id)
                                        .Skip(startIndex)
                                        .Take(pageSize)
                                        .ToArrayAsync();

                return new PagedResult<BlogPost>(records, count);
            });
        }

        public async Task<BlogPost?> GetBlogPostByIdAsync(int id) =>
            await ExecuteOnContext(async contextInfo =>
                await contextInfo.BlogPosts
                                .AsNoTracking()
                                .Include(b => b.Category)
                                .FirstOrDefaultAsync(b => b.Id == id)
            );

        /*
         example of how we can have duplicate slug
        * blog1 -> How to do this in Blazor (WASM) ->
        * blog2 -> how to do this in blasor wasm
        * both will end up being the same
         */

        private async Task<string> GenerateSlugAsync(BlogPost blogPost)
        {
            return await ExecuteOnContext(async context =>
            {
                string originalSlug = blogPost.Title.ToSlug();

                string slug = originalSlug;
                int count = 1;

                while (await context.BlogPosts.AsNoTracking().AnyAsync(b => b.Slug == slug))
                {
                    slug = $"{originalSlug} - {count++}";
                }
                return slug;
            });

        }

        public async Task<BlogPost> SaveBlogPostAsync(BlogPost blogPost, string userId)
        {
            return await ExecuteOnContext(async context =>
            {
                if (blogPost.Id == 0)
                {
                    //New blog Post
                    var isDuplicatedTitle = await context.BlogPosts
                                                    .AsNoTracking()
                                                    .AnyAsync(b => b.Title == blogPost.Title);
                    if (isDuplicatedTitle)
                    {
                        throw new InvalidOperationException($"Blog poat with this same title exists");
                    }

                    blogPost.Slug = await GenerateSlugAsync(blogPost);

                    blogPost.CreatedAt = DateTime.UtcNow;
                    blogPost.UserId = userId;

                    if (blogPost.IsPublished)
                    {
                        blogPost.PublishedAt = DateTime.UtcNow;
                    }
                    await context.BlogPosts.AddAsync(blogPost);

                }
                else
                {
                    //existing blog post
                    var isDuplicatedTitle = await context.BlogPosts
                                                    .AsNoTracking()
                                                    .AnyAsync(b => b.Title == blogPost.Title && b.Id != blogPost.Id);
                    if (isDuplicatedTitle)
                    {
                        throw new InvalidOperationException($"Blog post with this same title exists");
                    }

                    var dbBlog = await context.BlogPosts.FindAsync(blogPost.Id);

                    dbBlog!.Title = blogPost.Title;
                    dbBlog.Image = blogPost.Image;

                    dbBlog.introduction = blogPost.introduction;
                    dbBlog.Content = blogPost.Content;
                    dbBlog.CategoryId = blogPost.CategoryId;
                    dbBlog.PublishedAt = blogPost.PublishedAt;
                    dbBlog.IsPublished = blogPost.IsPublished;
                    dbBlog.IsFeatured = blogPost.IsFeatured;

                    if (blogPost.IsPublished)
                    {
                        if (!dbBlog.IsPublished)
                        {
                            blogPost.PublishedAt = DateTime.UtcNow;
                        }
                    }
                    else
                    {
                        blogPost.PublishedAt = null;
                    }
                }
                await context.SaveChangesAsync();
                return blogPost;
            });
        }
    }
}

