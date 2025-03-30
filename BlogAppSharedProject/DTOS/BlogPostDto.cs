using System.ComponentModel.DataAnnotations;

namespace BlogAppSharedProject.DTOS
{
    public class BlogPostDto
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(125)]
        public string Slug { get; set; }

        [MaxLength(100)]
        public string Image { get; set; }

        [Required, MaxLength(500, ErrorMessage = "The Max length is exceeded")]
        public string introduction { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Excerpt { get; set; } = string.Empty;
        public string CategoryName { get; set; }
        public string CategorySlug { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int ViewCount { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime? PublishedAt { get; set; }
    }
}
