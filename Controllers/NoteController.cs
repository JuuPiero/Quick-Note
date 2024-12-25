using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Note.Attributes;
using Note.Data;
using Note.Models;

namespace Note.Controllers;

public class NoteController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    public NoteController(ApplicationDbContext dbContext)
    {   
        _dbContext = dbContext;
    }

    [Auth]
    public IActionResult Create()
    {
        return View();
    }
    

    [HttpPost]
    public async Task<IActionResult> Store(Note.Models.Note note)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == HttpContext.Session.GetString("Username"));
        if(note != null && user != null) {
            note.UserId = user.Id;
            note.CreatedAt = DateTime.Now;
            note.UpdatedAt = DateTime.Now;
            await _dbContext.Notes.AddAsync(note);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        else {
            System.Console.WriteLine(HttpContext.Session.GetString("Username"));
        }

        return RedirectToAction("Create");
    }


    [Auth]
    [HttpPost]
    public async Task<IActionResult> Update(int id, Note.Models.Note note)
    {
        var noteExist = await _dbContext.Notes.FirstOrDefaultAsync(n => n.Id == id);
        noteExist.Title = note.Title;
        noteExist.Content = note.Content;
        noteExist.UpdatedAt = DateTime.Now;
        await _dbContext.SaveChangesAsync();

        return RedirectToAction("Detail", new { id });
    }

    [Auth]
    public async Task<IActionResult> Detail(int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == HttpContext.Session.GetString("Username"));
        ViewBag.User = user;

        var note = await _dbContext.Notes.FirstOrDefaultAsync(n => n.Id == id);
        return View(note);
    }

}
