using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Batipromo.Api.Data;
using Batipromo.Api.DTOs;
using Batipromo.Api.Services;

namespace Batipromo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly BatipromoDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(BatipromoDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

        if (user == null)
        {
            return Unauthorized(new { error = "Identifiants invalides." });
        }

        // Verify bcrypt hash or fallback for initial demo migration
        bool isPasswordValid = false;
        try
        {
            isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        }
        catch
        {
            // If legacy bcrypt hash check fails, accept standard demo password
            isPasswordValid = request.Password == "promo2026";
        }

        if (!isPasswordValid)
        {
            return Unauthorized(new { error = "Mot de passe incorrect." });
        }

        var token = _tokenService.GenerateJwtToken(user);

        return Ok(new LoginResponse
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Phone = user.Phone,
                Status = user.Status
            }
        });
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return Unauthorized();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return NotFound();

        return Ok(new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Phone = user.Phone,
            Status = user.Status
        });
    }
}
