using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Note.Attributes;
using Note.Data;
using Note.Models;

namespace Note.Controllers;

public class GroupController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    public GroupController(ApplicationDbContext dbContext)
    {   
        _dbContext = dbContext;
    }

    [Auth]
    public async Task<IActionResult> Index()
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == HttpContext.Session.GetString("Username"));
        ViewBag.User = user;
        var groups = await _dbContext.Groups
        .Where(g => g.UserId == user.Id) // Các nhóm mà user làm chủ
        .Union(
            _dbContext.Members.Where(m => m.UserId == user.Id).Select(m => m.Group) // Chuyển từ Member sang Group
        )
        .ToListAsync();
        ViewBag.Groups = groups;
        return View();
    }

    [Auth]
    public IActionResult Create()
    {
        return View();
    }
    

    [HttpPost]
    public async Task<IActionResult> Store(Group group)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == HttpContext.Session.GetString("Username"));
        if(group != null && user != null) {
            group.UserId = user.Id;
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Group");
        }
        

        return RedirectToAction("Create");
    }

    [Auth]
    public async Task<IActionResult> Detail(int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == HttpContext.Session.GetString("Username"));
        ViewBag.User = user;

        var usersWithGroupStatus = await _dbContext.Users.Where(u => u.Id != user.Id)
        .Select(u => new {
            User = u,
            IsInGroup = _dbContext.Members.Any(m => m.UserId == u.Id && m.GroupId == id )
        }).ToListAsync();
        ViewBag.Users = usersWithGroupStatus;

        var notes = await _dbContext.Notes.Where(note => note.UserId == user.Id) 
        .Select(note => new
        {
            Note = note,
            IsShared = _dbContext.Shares.Any(share => share.NoteId == note.Id && share.GroupId == id)
        })
        .ToListAsync();
        ViewBag.Notes = notes;

        var group = await _dbContext.Groups.FirstOrDefaultAsync(g => g.Id == id);

        var noteInGroups = await _dbContext.Notes.Where(note => _dbContext.Shares.Any(share => share.NoteId == note.Id && share.GroupId == id))
        .ToListAsync();
        ViewBag.NoteInGroups = noteInGroups;

        return View(group);
    }

    [HttpPost]
    public async Task<IActionResult> AddMembers(int id, List<int> userIds)
    {
        var members = await _dbContext.Members.Where(member => member.GroupId == id).ToListAsync();
        foreach (var member in members)
        {
            _dbContext.Members.Remove(member);
        }
        await _dbContext.SaveChangesAsync();


        foreach (var userId in userIds)
        {
            await _dbContext.Members.AddAsync(new Member { GroupId = id, UserId = userId});
            await _dbContext.SaveChangesAsync();
        }
        

        return RedirectToAction("Detail", new {id = id});
    }



    [HttpPost]
    public async Task<IActionResult> ShareNotes(int id, List<int> noteIds)
    {
        var shares = await _dbContext.Shares.Where(share => share.GroupId == id).ToListAsync();
        foreach (var share in shares)
        {
            _dbContext.Shares.Remove(share);
        }
        await _dbContext.SaveChangesAsync();


        foreach (var noteId in noteIds)
        {
            await _dbContext.Shares.AddAsync(new Share { GroupId = id, NoteId = noteId});
            await _dbContext.SaveChangesAsync();
        }

        return RedirectToAction("Detail", new {id = id});
    }


}
