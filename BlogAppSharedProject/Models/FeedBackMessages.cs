using System.ComponentModel.DataAnnotations;

namespace BlogAppSharedProject.Models
{
    public class FeedBackMessages
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required, EmailAddress]
        public string? EmailAddress { get; set; }

        [Required, MaxLength(500)]
        public string? Message { get; set; }


    }
}
