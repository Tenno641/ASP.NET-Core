using CitiesManager.DTO;
using CitiesManager.Models.IdentityEntities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CitiesManager.Services;

public class JwtService
{
    private readonly IConfiguration _configuration;
    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public UserResponse GenerateToken(User user)
    {
        DateTime expiration = DateTime.UtcNow.AddSeconds(Convert.ToDouble(_configuration["Jwt:ExpirationTimeInSeconds"]));

        Claim[] claims =
        [
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.Jti, Guid.CreateVersion7().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            new (ClaimTypes.NameIdentifier, user.Email),
            new (ClaimTypes.Email, user.Email),
            new (ClaimTypes.Name, user.Name),
        ];

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("Please Provide Security Key")));

        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken securityToken = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: signingCredentials);
        
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        string token = tokenHandler.WriteToken(securityToken);

        UserResponse userResponse = new UserResponse()
        {
            Id = user.Id,
            Email = user.Email,
            Expiration = expiration,
            Name = user.Name,
            Token = token
        };
        return userResponse;
    }
}
