using FutureXcelRoleBasedAccess.Data;
using FutureXcelRoleBasedAccess.Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("users")]
    public async Task<ActionResult<List<UserDto>>> GetAllUsers()
    {
        var users = await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                CreatedAt = u.CreatedAt,
                IsActive = u.IsActive
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpDelete("user/{id}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound("User not found");

        if (user.Role == "Admin")
            return BadRequest("Cannot delete admin users");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok("User deleted successfully");
    }

    [HttpPut("user/{id}/toggle-status")]
    public async Task<ActionResult> ToggleUserStatus(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound("User not found");

        user.IsActive = !user.IsActive;
        await _context.SaveChangesAsync();

        return Ok($"User {(user.IsActive ? "activated" : "deactivated")} successfully");
    }
}