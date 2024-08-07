namespace BlogAppSharedProject.Models
{
    public record PagedResult<TResult>(TResult[] Results, int TotalCount);
}
