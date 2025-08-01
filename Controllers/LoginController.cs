using System.Diagnostics;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Task4.AppDbContext;
using Task4.DTOs;
using Task4.Models;
using Task4.Migrations;

namespace Task4.Controllers;

[AllowAnonymous]
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
        return View("Index");
    }

    public IActionResult SignUp()
    {
        return View("SignUp");
    }

    [HttpPost]
    public IActionResult Login(UserModel user)
    {

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInDto dto)
    { 
        var email = dto.Email;
        var password = dto.Password;
        var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Email == email && u.Password == password && u.isActive);

        if (user == null)
        {
            ViewBag.ErrorMessage = "Invalid email or password";
            return View("Index");
        }

        user.LastSeen = DateTime.Now;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim("Role", user.Role.Role),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("UserId", user.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home");
    }   

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpDto dto)
    {
        try
        {
            await _context.Users.AddAsync(new UserModel
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                RoleId = 1,
                isActive = true
            });
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlEx)
            {
                switch (sqlEx.Number)
                {
                    case 2601:
                        ViewBag.ErrorMessage = "Email already registred";
                        break;
                    case 515:
                        ViewBag.ErrorMessage = "Missing requiered fields: Name, Email or Password ";
                        break;
                    case 547:
                        ViewBag.ErrorMessage = "Password Empty";
                        break;
                    default:
                        ViewBag.ErrorMessage = $"Code: {sqlEx.Number}, Message: {sqlEx.Message}";
                        break;

                }
                return View("SignUp");
            }
        }

        TempData["Message"] = "Registered Successfully";
        return RedirectToAction("Index", "Login");
    }

}