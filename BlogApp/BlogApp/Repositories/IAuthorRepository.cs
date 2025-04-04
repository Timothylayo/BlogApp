
namespace BlogApp.Repositories
{
    public interface IAuthorRepository
    {
        Task GetAuthorDetailsAsync(string userId);
    }
}