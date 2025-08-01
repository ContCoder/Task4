using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Task4.Models;
using Task4.AppDbContext;
using Task4.DTOs;

namespace Task4.Controllers;

public class HomeController : Controller
{
    private readonly Context _context;
    public HomeController(Context context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var users = GetUsers();
        return View(users);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateActiveUsers([FromBody]UpActiveUsersDto data)
    {
        try
        {
            var users = _context.Users.Where(u => data.Ids.Contains(u.Id)).ToList();
            foreach (var user in users)
            {
                user.isActive = data.IsActive;
                _context.Update(user);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest();

        }

    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUsers([FromBody] int[] ids)
    {
        try
        {
            var users = _context.Users.Where(u => ids.Contains(u.Id)).ToList();
            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {

            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties
        {
            ExpiresUtc = DateTime.UtcNow,
            IsPersistent = false,
            AllowRefresh = false
        });

        return RedirectToAction("Index", "Home");
    }

    private List<UserModelView> GetUsers()
    {
        return _context.Users.Select(u =>
            new UserModelView
            {
                Email = u.Email,
                Id = u.Id,
                Name = u.Name,
                LastSeen = u.LastSeen,
                isActive = u.isActive
            }).ToList();
    }
}
