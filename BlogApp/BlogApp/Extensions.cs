using BlogAppSharedProject;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace BlogApp
{
    public static partial class Extensions
    {
        public static string GetUserName(this ClaimsPrincipal principal) =>
            principal.FindFirstValue(AppConstants.ClaimsNames.FullName)!;
        public static string GetUserId(this ClaimsPrincipal principal) =>
            principal.FindFirstValue(ClaimTypes.NameIdentifier)!;


        public static string ToSlug(this string text)
        { 
            //Blazor (WASM) -> blazor--wasm-   
            text = SlugRegex().Replace(text.ToLowerInvariant(), "-");

            return text.Replace("--", "-") //blazor-wasm-
                     .Trim('-'); ; // blazor-wasm 
        }

        [GeneratedRegex(@"[^0-9a-z_]")]
        private static partial Regex SlugRegex();

        public static string ToDisplay(this DateTime? dataTime) => dataTime?.ToString("MMM dd") ?? string.Empty;
    }
}
