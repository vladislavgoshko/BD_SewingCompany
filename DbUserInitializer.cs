using Microsoft.AspNetCore.Identity;

namespace SewingCompany
{
    //Инициализация базы данных первой учетной записью и двумя ролями admin и user
    public static class DbUserInitializer
    {
        public static async Task Initialize(HttpContext context)
        {
            RoleManager<IdentityRole> roleManager = context.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();


            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

        }
    }
}


