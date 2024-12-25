using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Note.Attributes;
using Note.Data;
using Note.Models;

namespace Note.Controllers;

public class UserController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    public UserController(ApplicationDbContext dbContext)
    {   
        _dbContext = dbContext;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> PostLogin(User user)
    {
        var exists = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == user.Username && u.Password == user.Password);
        if (user != null)
        {
            HttpContext.Session.SetString("Username", user.Username);
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Invalid username or password");
        return RedirectToAction("Login");
    }


    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> PostRegister(User user)
    {
        if (ModelState.IsValid)
        {
            if (_dbContext.Users.Any(u => u.Username == user.Username))
            {
                ModelState.AddModelError("", "Username already exists");
                return View();
            }

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Login");
        }

        return RedirectToAction("Register", "User"); 
    }
    [Auth]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }


}
