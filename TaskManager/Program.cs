using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;

var builder = WebApplication.CreateBuilder(args);

// 1?? Подключаем БД
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2?? Регистрация Identity с ролями
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

// 3?? Контроллеры с Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 4?? Создаём роли при старте
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

// 5?? Middleware
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 6?? Маршрутизация
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tasks}/{action=Index}/{id?}");

app.Run();
