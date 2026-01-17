using CitiesManager.DTO;
using CitiesManager.Models.IdentityEntities;
using CitiesManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CitiesManager.Controllers.v1;

[AllowAnonymous]
public class AccountController : CustomControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly JwtService _jwtService;
    public AccountController(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, JwtService jwtService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> PostRegister(Register register)
    {
        User user = new User()
        {
            Name = register.Name,
            Email = register.Email,
            UserName = register.Email
        };

        IdentityResult? identityResult = await _userManager.CreateAsync(user, register.Password);
        if (!identityResult.Succeeded) return Problem(string.Join(" | ", identityResult.Errors.Select(error => error.Description)));

        UserResponse userResponse = _jwtService.GenerateToken(user);
        user.RefreshToken = userResponse.RefreshToken;
        user.RefreshTokenExpiration = userResponse.RefreshTokenExpiration;
        await _userManager.UpdateAsync(user);

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userResponse);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetUser(Guid id)
    {
        User? user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null) return NotFound();

        UserResponse userResponse = _jwtService.GenerateToken(user);
        return Ok(userResponse);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> PostLogin(Login login)
    {
        var signInResult = await _signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent: false, lockoutOnFailure: false);

        if (!signInResult.Succeeded) return Problem();

        User? user = await _userManager.FindByNameAsync(login.Email);
        if (user is null) return NotFound();

        UserResponse userResponse = _jwtService.GenerateToken(user);
        user.RefreshToken = userResponse.RefreshToken;
        user.RefreshTokenExpiration = userResponse.RefreshTokenExpiration;
        await _userManager.UpdateAsync(user);

        return Ok(userResponse);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<UserResponse>> RefreshToken(AuthenticationToken authToken)
    {
        ClaimsPrincipal principal = _jwtService.ValidateToken(authToken.Token);
        
        User? user = await _userManager.FindByIdAsync(principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
        if (user is null) return NotFound("Can't find user");

        if (user.RefreshToken.Equals(authToken.RefreshToken) || user.RefreshTokenExpiration < DateTime.UtcNow)
        {
            return Unauthorized();
        }

        UserResponse userResponse = _jwtService.GenerateToken(user);

        user.RefreshToken = userResponse.RefreshToken;
        user.RefreshTokenExpiration = userResponse.RefreshTokenExpiration;
        await _userManager.UpdateAsync(user);

        return userResponse;
    }
}
