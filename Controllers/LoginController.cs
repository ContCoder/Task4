

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task4.AppDbContext;
using Task4.Dtos;
using Task4.Models;

namespace Task4.Controllers;

public class LoginController : Controller
{
    private readonly Context _context;
    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger, Context context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(UserModel user)
    {

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> SingUp(SignUpDto dto)
    {
        try
        {
            await _context.Users.AddAsync(new UserModel
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                RoleId = 1
            });
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating user: {ex.Message}");
        }


        return Json(new
        {
            success = true,
            message = "User created successfully"
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}