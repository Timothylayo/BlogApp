using BlogApp.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }

        public string? Image { get; set; }

        public List<BlogPost> BlogPosts { get; set; }
    }

}
