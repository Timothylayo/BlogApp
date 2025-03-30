using BlogApp.Data;
using BlogAppSharedProject.DTOS;
using BlogAppSharedProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories
{
    public class BlogPostRepository(ApplicationDbContext context) : IBlogPostRepository
    {
        private readonly ApplicationDbContext context = context;

        public async Task<List<BlogPostDto>> GetBlogPostAsync()
        {
            List<BlogPostDto> posts = [];
            var blogPosts = await context.BlogPosts.AsNoTracking()
                                             .Include(c => c.Category)
                                             .Include(c => c.User)
                                             .Where(c => c.IsPublished)
                                             .ToListAsync();

            foreach (var blogPost in blogPosts)
            {
                BlogPostDto blogPostDto = new()
                {
                    Title = blogPost.Title,
                    CategoryName = blogPost.Category.Name,
                    Content = blogPost.Content,
                    Image = blogPost.Image,
                    Slug = blogPost.Slug,
                    UserId = blogPost.UserId,
                    ViewCount = blogPost.ViewCount,
                    PublishedAt = blogPost.CreatedAt,
                    introduction = blogPost.introduction,
                    IsFeatured = blogPost.IsFeatured,
                };
                posts.Add(blogPostDto);
            }

            return posts;
        }

        public async Task<PaginationResponse<BlogPostDto>> GetBlogPostsAsync(int pageNumber, int pageSize)
        {
            var totalRecords = await context.BlogPosts.CountAsync(); // Get total records count

            var blogposts = await context.BlogPosts
                .Include(p => p.Category)
                .Include(p => p.User)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = blogposts.Select(blogPost => new BlogPostDto
            {
                Title = blogPost!.Title,
                CategoryName = blogPost.Category.Name,
                Content = blogPost.Content,
                Excerpt = Truncate(blogPost.Content, 180),
                Slug = blogPost.Slug,
                Image = blogPost.Image,
                introduction = blogPost.introduction,
                IsFeatured = blogPost.IsFeatured,
                PublishedAt = blogPost.PublishedAt,
                UserId = blogPost.UserId,
                ViewCount = blogPost.ViewCount
            });

            return new PaginationResponse<BlogPostDto>
            {
                TotalRecords = totalRecords,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Data = [.. result]
            };
        }


        public async Task<List<BlogPostDto>> GetBlogPostByCategoryAsync(string categorySlug)
        {
            List<BlogPostDto> blogs = [];
            var blogPosts = await context.BlogPosts
                                          .AsNoTracking()
                                          .Include(p => p.Category)
                                          .Include(p => p.User)
                                          .Where(b => b.Category.Slug == categorySlug && b.IsPublished)
                                          .ToListAsync();

            if (blogPosts is null)
            {
                return null;
            }

            foreach (var blogPost in blogPosts)
            {
                var post = new BlogPostDto
                {
                    Title = blogPost!.Title,
                    CategoryName = blogPost.Category.Name,
                    Content = blogPost.Content,
                    Slug = blogPost.Slug,
                    Image = blogPost.Image,
                    introduction = blogPost.introduction,
                    IsFeatured = blogPost.IsFeatured,
                    PublishedAt = blogPost.PublishedAt,
                    UserId = blogPost.UserId,
                    ViewCount = blogPost.ViewCount,
                };
                blogs.Add(post);
            }
            return blogs;
        }
        public async Task<BlogPostDto> GetBlogPostBySlugAsync(string slug)
        {
            var blogPost = await context.BlogPosts
                                          .AsNoTracking()
                                          .Include(p => p.Category)
                                          .Include(p => p.User)
                                          .FirstOrDefaultAsync(b => b.Slug == slug && b.IsPublished);

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == blogPost!.UserId);

            if (blogPost is null && user == null)
            {
                return null;
            }
            var blog = new BlogPostDto
            {
                Title = blogPost.Title,
                CategoryName = blogPost.Category.Name,
                Content = blogPost.Content,
                Slug = blogPost.Slug,
                Image = blogPost.Image,
                introduction = blogPost.introduction,
                IsFeatured = blogPost.IsFeatured,
                PublishedAt = blogPost.PublishedAt,
                UserId = blogPost.UserId,
                ViewCount = blogPost.ViewCount,
                UserName = user!.Name!

            };
            return blog;
        }

        public async Task<List<BlogPostDto>> GetFeaturedBlogPostAsync(int count)
        {
            List<BlogPostDto> featuredPosts = [];
            var query = context.BlogPosts
                            .AsNoTracking()
                            .Include(p => p.Category)
                            .Include(p => p.User)
                            .Where(p => p.IsPublished);

            var records = await query.Where(p => p.IsFeatured)
                        .OrderBy(_ => Guid.NewGuid())
                        .Take(count)
                        .ToListAsync();

            if (count > records.Count)
            {
                var additionalRecords = await query.Where(p => !p.IsFeatured)
                       .OrderBy(_ => Guid.NewGuid())
                       .Take(count - records.Count)
                       .ToListAsync();
                records = [.. records, .. additionalRecords];
            }
            foreach (var post in records)
            {
                BlogPostDto featuredPost = new()
                {
                    Title = post.Title,
                    CategoryName = post.Category.Name,
                    Content = post.Content,
                    Slug = post.Slug,
                    Image = post.Image,
                    introduction = post.introduction,
                    IsFeatured = post.IsFeatured,
                    PublishedAt = post.PublishedAt,
                    UserId = post.UserId,
                    ViewCount = post.ViewCount
                };
                featuredPosts.Add(featuredPost);
            }
            return featuredPosts;
        }

        public async Task<List<BlogPostDto>> GetPopularBlogPostAsync(int count)
        {
            List<BlogPostDto> popularPosts = [];
            var query = context.BlogPosts
                            .AsNoTracking()
                            .Include(p => p.Category)
                            .Include(p => p.User)
                            .Where(p => p.IsPublished);

            await query.OrderByDescending(b => b.ViewCount)
                       .Take(count)
                       .ToListAsync();

            foreach (var post in query)
            {
                BlogPostDto popularPost = new()
                {
                    Title = post.Title,
                    CategoryName = post.Category.Name,
                    Content = post.Content,
                    Slug = post.Slug,
                    Image = post.Image,
                    introduction = post.introduction,
                    IsFeatured = post.IsFeatured,
                    PublishedAt = post.PublishedAt,
                    UserId = post.UserId,
                    ViewCount = post.ViewCount
                };
                popularPosts.Add(popularPost);
            }
            return popularPosts;
        }

        public async Task<List<BlogPostDto>> GetRecentBlogPostAsync(int count) =>
            await GetPostsAsync(0, count, 0);


        private async Task<List<BlogPostDto>> GetPostsAsync(int skip, int take, int categoryId)
        {
            List<BlogPostDto> blogPosts = [];
            var query = context.BlogPosts
                        .AsNoTracking()
                        .Include(p => p.Category)
                        .Include(p => p.User)
                        .Where(p => p.IsPublished);
            if (categoryId > 0)
            {
                query = query.Where(b => b.CategoryId == categoryId);
            }

            await query.OrderByDescending(b => b.PublishedAt)
                        .Skip(skip)
                        .Take(take)
                        .ToListAsync();
            foreach (var post in query)
            {
                BlogPostDto blogPost = new()
                {
                    Title = post.Title,
                    CategoryName = post.Category.Name,
                    Content = post.Content,
                    Slug = post.Slug,
                    Image = post.Image,
                    introduction = post.introduction,
                    IsFeatured = post.IsFeatured,
                    PublishedAt = post.PublishedAt,
                    UserId = post.UserId,
                    ViewCount = post.ViewCount
                };

                blogPosts.Add(blogPost);
            }


            return blogPosts;

        }

        private static string Truncate(string content, int maxLength)
        {
            if (string.IsNullOrEmpty(content)) return content;

            if (content.Length <= maxLength) return content;

            return content.Substring(0, maxLength) + "...";
        }
    }
}
