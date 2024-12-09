using CaloriesCalculator.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация на връзка с базата данни
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Регистрация на Identity с настройки за пароли
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole>() // Добавяне на роли
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();  // Ако използваш Razor Pages

var app = builder.Build();

// Създаване на роля "Admin" и администраторски потребител (ако не съществуват)
await CreateAdminRoleAndUser(app);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Регистриране на маршрут за Admin Area
app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",   // Името на Admin Area
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

// Създаване на роля "Admin" и потребител с роля "Admin", ако не съществуват
async Task CreateAdminRoleAndUser(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        // Проверка дали ролята "Admin" съществува
        var roleExists = await roleManager.RoleExistsAsync("Admin");
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // Проверка дали администраторският потребител съществува
        var user = await userManager.FindByEmailAsync("admin@domain.com");
        if (user == null)
        {
            // Създаване на нов потребител
            user = new IdentityUser
            {
                UserName = "admin@domain.com",
                Email = "admin@domain.com"
            };

            // Създаване на потребителя с парола
            var result = await userManager.CreateAsync(user, "AdminPassword123!");
            if (result.Succeeded)
            {
                // Добавяне на роля "Admin" към потребителя
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
