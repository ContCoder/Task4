using Microsoft.EntityFrameworkCore;
using Task4.AppDbContext;
using Task4.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
