using AuthApi.Modals;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthApi.Controllers;

/// <summary>
/// Controller for handling user authentication operations such as registration and login.
/// </summary>
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="userService">Service for user data operations.</param>
    /// <param name="configuration">Application configuration, including JWT settings.</param>
    public AuthController(UserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    /// <summary>
    /// Registers a new user if the username is not already taken.
    /// </summary>
    /// <param name="user">The username and password provided by the user.</param>
    /// <returns>An HTTP response indicating success or failure.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto user)
    {
        var existingUser = await _userService.GetByUsername(user.Username);
        if (existingUser != null)
            return BadRequest("User already exists.");

        await _userService.CreateUser(user.Username, user.Password);
        return Ok("User registered.");
    }
    /// <summary>
    /// Authenticates a user and returns a JWT if credentials are valid.
    /// </summary>
    /// <param name="user">The username and password provided by the user.</param>
    /// <returns>A JWT token if authentication is successful; otherwise, an error.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto user)
    {
        var dbUser = await _userService.GetByUsername(user.Username);
        if (dbUser == null)
        {
            dbUser = await _userService.GetByEmail(user.Username);
        }
        if (dbUser == null)
        {
            dbUser = await _userService.GetByPhone(user.Username);
        }
        if (dbUser == null || !_userService.VerifyPassword(user.Password, dbUser.PasswordHash))
            return Unauthorized("Invalid credentials.");

        var token = GenerateJwtToken(dbUser);
        return Ok(new { Token = token });
    }
    /// <summary>
    /// regenerate token using prevoious token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("Regenrate")]
    public async Task<IActionResult> RegenerateToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token);
        if (jsonToken == null)
            return Unauthorized("Invalid token.");
        var jwtdata = CustomJwtParser.Parse(jsonToken.ToString());
        if (jwtdata.Expiration < DateTime.UtcNow) return Unauthorized("token expire.");
        var user = jwtdata.User;
        var dbUser = await _userService.GetByUsername(user);
        if (user == null)
            return Unauthorized("Invalid credentials.");
        return Ok(new { Token = GenerateJwtToken(dbUser) });
    }
    /// <summary>
    /// Generates a JWT token for a validated user.
    /// </summary>
    /// <param name="user">The authenticated user.</param>
    /// <returns>A signed JWT token.</returns>
    private string GenerateJwtToken(User user)
    {
        var claims = new[]{new Claim(ClaimTypes.Name, user.Username)};

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.WriteToken(token);  // ✅ This will now succeed

        return jsonToken;
    }

}
public class UserLoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}
/// <summary>
/// DTO for user credentials used in registration and login.
/// </summary>
public class UserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNo { get; set; }
    /// <summary>
    /// The username of the user.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// The plain-text password of the user.
    /// </summary>
    public string Password { get; set; }
}