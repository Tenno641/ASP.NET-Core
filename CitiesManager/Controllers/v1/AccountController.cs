using CitiesManager.DTO;
using CitiesManager.Models.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.Controllers.v1;

public class AccountController : CustomControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly SignInManager<User> _signInManager;
    public AccountController(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
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

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new { user.Id, user.Name, user.Email });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        User? user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null) return NotFound();
        return Ok(new { user.Id, user.Name, user.Email });
    }

    [HttpPost]
    public async Task<ActionResult<User>> PostLogin(Login login)
    {
        var signInResult = await _signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent: false, lockoutOnFailure: false);

        if (!signInResult.Succeeded) return Problem();

        User? user = await _userManager.FindByNameAsync(login.Email);
        if (user is null) return NotFound();

        return Ok(new { user.Id, user.Email, user.Name });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return NoContent();
    }

}
