using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Note.Attributes;
using Note.Data;
using Note.Models;

namespace Note.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public HomeController(ApplicationDbContext dbContext)
    {   
        _dbContext = dbContext;
    }
    [Auth]
    public async Task<IActionResult> Index()
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == HttpContext.Session.GetString("Username"));
        var notes = await _dbContext.Notes.Where(n => n.UserId == user.Id).ToListAsync();
        ViewBag.Notes = notes;
        return View();
    }
  
}
