using CleaningService.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Runtime.ExceptionServices;
using System.Text.Json.Nodes;
using System.Reflection.Metadata;
using System.Xml.Linq;
using System.ComponentModel.Design;
using CleaningService.Web.Models;
using System.Runtime.CompilerServices;
using System.Formats.Tar;
using Microsoft.AspNetCore.Http; // make sure this exists at top


namespace CleaningService.Web.Controllers
{
    


[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;


    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("users")]
public IActionResult Read()
{
    var users = _context.Users
        .Select(u => new { u.Id, u.Name, u.Email, u.Role })
        .ToList();

    return Ok(users);
}

[HttpPost("registered")]
public IActionResult Create([FromForm] User newUser)
{
    _context.Add(newUser);
    _context.SaveChanges();

    return Redirect("/login");
}


[HttpPost("login")]
public IActionResult Login([FromForm] User req)
{
    var user = _context.Users
        .FirstOrDefault(u => u.Email == req.Email && u.Password == req.Password);

    if (user == null)
        return Redirect("/login?error=1");

    HttpContext.Session.SetInt32("UserId", user.Id);
    HttpContext.Session.SetString("Role", user.Role);
    HttpContext.Session.SetString("Name", user.Name);

    return user.Role == "Admin"
    ? Redirect("/Admin")
    : Redirect("/Booking");  
}

    [HttpPut("users/{id:int}")]
public IActionResult Update(int id, [FromForm] User updated)
{
    var user = _context.Users.Find(id);
    if (user == null) return NotFound();

    user.Name = updated.Name;
    user.Email = updated.Email;
    user.Role = updated.Role;

    _context.SaveChanges();

    return Ok(new { message = "updated" });
    // or: return NoContent();
}

[HttpDelete("users/{id:int}")]
public IActionResult Delete(int id)
{
    var existingUser = _context.Users.Find(id);
    if (existingUser == null) return NotFound();

    _context.Users.Remove(existingUser);
    _context.SaveChanges();

    return Ok(new { message = "deleted" });
    // or: return NoContent();
}
}


    
}