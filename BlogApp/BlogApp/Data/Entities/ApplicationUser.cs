using Microsoft.AspNetCore.Identity;

namespace BlogApp.Data.Entities
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }

        public string? Image { get; set; }
        public string? Description { get; set; }

        public List<BlogPost> BlogPosts { get; set; }
    }

}
