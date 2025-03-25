using BlogAppSharedProject.DTOS;

namespace BlogAppSharedProject.Models
{
    public record DetailPageModel(BlogPostDto BlogPost, List<BlogPostDto> RelatedPosts)
    {
        public static DetailPageModel Empty() => new(default!, []);
        public bool IsEmpty => BlogPost is null;
    }
}
