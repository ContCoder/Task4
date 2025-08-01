using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Task4.AppDbContext;
using Task4.Middleware;
using Task4.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/Login/Index";
    });

builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews(config =>
{
    config.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build()));
});

var app = builder.Build();

// Dummy data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Context>();

    context.Database.EnsureCreated();

    if (!context.Rols.Any())
    {
        var datoDummy = new RoleModel
        {
            Role = "Admin"
        };

        context.Rols.Add(datoDummy);
        context.SaveChanges();

        Console.WriteLine("Admin role created");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseMiddleware<ActiveOnlyMiddleware>();
app.UseAuthorization();

//app.MapStaticAssets();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Login}/{action=Index}/{id?}")
//    .WithStaticAssets();
app.UseStaticFiles(); // Habilita archivos estáticos en wwwroot

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");


app.Run();
