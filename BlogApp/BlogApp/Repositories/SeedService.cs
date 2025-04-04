using BlogApp.Data;
using BlogApp.Data.Entities;
using BlogAppSharedProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories
{
    internal static class AdminAccount
    {
        public const string Name = "Timothy Omolayole";
        public const string Email = "timothylayo27@gmail.com";
        public const string Role = "Admin";
        public const string Password = "DGc_Z8f7$J9zV9@";

    }
    public class SeedService(ApplicationDbContext applicationDb, IUserStore<ApplicationUser> userStore,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : ISeedService
    {
        private readonly ApplicationDbContext _applicationDbContext = applicationDb;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IUserStore<ApplicationUser> userStore = userStore;

        public async Task SeedDataAsync()
        {
            //seed admin role
            if (await _roleManager.FindByNameAsync(AdminAccount.Role) is null)
            {
                var adminRole = new IdentityRole(AdminAccount.Role);

                var result = await _roleManager.CreateAsync(adminRole);
                if (!result.Succeeded)
                {
                    var errorsString = result.Errors.Select(e => e.Description);
                    throw new Exception($"Error in creating Admin Role {Environment.NewLine} {string.Join(Environment.NewLine, errorsString)}");
                }

            }

            //seed Admin User

            var adminUser = await _userManager.FindByEmailAsync(AdminAccount.Email);

            if (adminUser == null)
            {
                //create the user since it does not exist in the database
                adminUser = new ApplicationUser();

                adminUser.Name = AdminAccount.Name;

                await userStore.SetUserNameAsync(adminUser, AdminAccount.Email, CancellationToken.None);
                var emailStore = (IUserEmailStore<ApplicationUser>)userStore;
                await emailStore.SetEmailAsync(adminUser, AdminAccount.Email, CancellationToken.None);


                var result = await _userManager.CreateAsync(adminUser, AdminAccount.Password);
                var getUserRoles = await _userManager.GetRolesAsync(adminUser);
                var userRole = await _userManager.AddToRoleAsync(adminUser!, AdminAccount.Role);


                if (!result.Succeeded)
                {
                    var errorsString = result.Errors.Select(e => e.Description);
                    throw new Exception($"Error in creating Admin User {Environment.NewLine} {string.Join(Environment.NewLine, errorsString)}");

                }
            }

            //Seed Categories
            if (!await _applicationDbContext.Categories.AsNoTracking().AnyAsync())
            {
                await _applicationDbContext.Categories.AddRangeAsync(Category.GetSeedCategories());
                await _applicationDbContext.SaveChangesAsync();
            }
        }

    }


}
