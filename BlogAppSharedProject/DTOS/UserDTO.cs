using System.ComponentModel.DataAnnotations;

namespace BlogAppSharedProject.DTOS
{
    public class UserDTO
    {
        public string Id { get; set; }
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Image { get; set; }



    }
}
