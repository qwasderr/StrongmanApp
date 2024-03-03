//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
//using StrongmanApp.Context;
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
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, SportDbContext _context, IUserStore<User> _userStore)
        {
           
            string adminEmail = "admin@gmail.com";
            string password = "A123@a";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                var role = new IdentityRole<int>();
                role.Name = "admin";
                //await roleManager.CreateAsync(new IdentityRole("admin"));
                var res= await roleManager.CreateAsync(role);
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                //await roleManager.CreateAsync(new IdentityRole("user"));
                var role = new IdentityRole<int>();
                role.Name = "user";
                //await roleManager.CreateAsync(new IdentityRole("admin"));
                await roleManager.CreateAsync(role);
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                //var user = Activator.CreateInstance<IdentityUser<int>>();
                var user = Activator.CreateInstance<User>();
                var emailstore = (IUserEmailStore<User>)_userStore;
                await _userStore.SetUserNameAsync(user, adminEmail, CancellationToken.None);
                await emailstore.SetEmailAsync(user, adminEmail, CancellationToken.None);
                user.EmailConfirmed = true;
                //user.Email = adminEmail;
                //user.NormalizedEmail= adminEmail.ToUpper();
                //user.UserName = "admin";
                user.Name = "admin";
                //user.NormalizedUserName = "admin".ToUpper();
                var result = await userManager.CreateAsync(user, password);
                
                if (result.Succeeded)
                {
                    _context.SaveChanges();
                    await userManager.AddToRoleAsync(user, "admin");
                }
                //User admin = new User { Email = adminEmail, UserName = adminEmail };
                //IdentityResult result = await userManager.CreateAsync(user, password);
               
            }
        }

    }
}
