//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using StrongmanApp.Models;

namespace StrongmanApp
{
    public class RoleInit
    {
        /*private readonly SportDbContext _context;

        public RoleInit(SportDbContext context)
        {
            _context = context;
        }*/
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SportDbContext _context)
        {
           
            string adminEmail = "admin@gmail.com";
            string password = "A123@a";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var user = Activator.CreateInstance<IdentityUser>();
                user.EmailConfirmed = true;
                user.Email = adminEmail;
                user.UserName = "admin";
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    User user1 = new User();
                    user1.Name = "admin";
                    user1.Id = user.Id;
                    user1.Email = adminEmail;
                    _context.Add(user1);
                    _context.SaveChanges();
                    await userManager.AddToRoleAsync(user, "admin");
                }
                //User admin = new User { Email = adminEmail, UserName = adminEmail };
                //IdentityResult result = await userManager.CreateAsync(user, password);
               
            }
        }

    }
}
