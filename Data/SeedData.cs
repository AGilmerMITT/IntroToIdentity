using IntroToIdentity.Models;
using Microsoft.AspNetCore.Identity;

namespace IntroToIdentity.Data
{
    public class SeedData
    {
        public static async Task Initialize
            (IServiceProvider serviceProvider, string? seedPw)
        {
            if (seedPw == null)
            {
                throw new Exception("No seed password!");
            }
            string adminId = await SeedUser
                (serviceProvider, "your.user@random.com", seedPw);

            await SeedRole(serviceProvider, adminId, Constants.AdminRole);
            await AddRoles(serviceProvider, Constants.LibrarianRole);
        }

        public static async Task<string> SeedUser
            (IServiceProvider serviceProvider, string userName,
            string seedPw)
        {
            var userManager = serviceProvider
                .GetService<UserManager<ApplicationUser>>()!;

            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = userName,
                    EmailConfirmed = true
                };
                
                var result = await userManager.CreateAsync(user, seedPw);

                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create seed user");
                }
            }

            return user.Id;
        }

        public static async Task<IdentityResult> SeedRole
            (IServiceProvider serviceProvider, string userId, string role)
        {
            var roleManager = serviceProvider
                .GetService<RoleManager<IdentityRole>>()!;

            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider
                .GetService<UserManager<ApplicationUser>>()!;

            var user = await userManager.FindByIdAsync(userId)
                ?? throw new Exception("Seed user not found");

            IdentityResult result = await userManager
                .AddToRoleAsync(user, role);

            return result;
        }

        public static async Task AddRoles(IServiceProvider serviceProvider, params string[] roles)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            foreach(string role in roles)
            {
                if (!await roleManager!.RoleExistsAsync(role))
                    await roleManager!.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
