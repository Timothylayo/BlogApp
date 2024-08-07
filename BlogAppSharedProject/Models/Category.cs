using System.ComponentModel.DataAnnotations;

namespace BlogAppSharedProject.Models
{
    public class Category
    {
        public short Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(75)]
        public string Slug { get; set; }

        public bool ShowOnNavbar { get; set; }

        // this helps to create a copy of the current object
        public Category Clone() => (MemberwiseClone() as Category)!;
        public static Category[] GetSeedCategories()
        {
            return
            [
                new Category { Name = "Real Madrid", Slug = "real-madrid", ShowOnNavbar = true },
                new Category { Name = "Manchester-United", Slug = "manchester-united", ShowOnNavbar = true },
                new Category { Name = "La Liga", Slug = "la-liga", ShowOnNavbar = true },
                new Category { Name = "Premier League", Slug = "premier-league", ShowOnNavbar = false },
            ];
        }
    }
}
