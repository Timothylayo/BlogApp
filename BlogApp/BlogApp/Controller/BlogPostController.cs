using BlogApp.Repositories;
using BlogAppSharedProject.DTOS;
using BlogAppSharedProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController(IBlogPostRepository blogPostRepository) : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository = blogPostRepository;

        [HttpGet("blogpostbyslug")]
        public async Task<ActionResult> GetBlogPostBySlugAsync(string slug)
        {
            return Ok(await blogPostRepository.GetBlogPostBySlugAsync(slug));
        }

        [HttpGet("featuredblogpost")]
        public async Task<ActionResult> GetFeaturedBlogPostAsync(int count)
        {
            return Ok(await blogPostRepository.GetFeaturedBlogPostAsync(count));
        }

        [HttpGet("popularblogpost")]
        public async Task<ActionResult> GetPopularBlogPostAsync(int count)
        {
            return Ok(await blogPostRepository.GetPopularBlogPostAsync(count));
        }

        [HttpGet("recentblogpost")]
        public async Task<ActionResult> GetRecentBlogPostAsync(int count)
        {
            return Ok(await blogPostRepository.GetRecentBlogPostAsync(count));
        }

        [HttpGet("getblogpost/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<PaginationResponse<BlogPostDto>>> GetBlogPGetBlogPostAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await blogPostRepository.GetBlogPostsAsync(pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (if logging is set up)
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



    }
}
