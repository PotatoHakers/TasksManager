using Microsoft.EntityFrameworkCore;
using TaskManager.Controllers;
using TaskManager.Data;
using TaskManager.Services;

var builder = WebApplication.CreateBuilder(args);
// Подключаем БД
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Подключаем MVC
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<TaskService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


