using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using stock.domain.entities;

namespace stock.domain.services;

public class JWTService  : IJWTService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;

    public JWTService(IConfiguration configuration)
    {
        string? jwt_key = configuration["JWT_KEY"] ?? throw new Exception("Missing JWT_KEY configuration");
        string? jwt_issuer = configuration["JWT_ISSUER"] ?? throw new Exception("Missing JWT_ISSUER configuration");
        string? jwt_audience = configuration["JWT_AUDIENCE"] ?? throw new Exception("Missing JWT_AUDIENCE configuration");

        _secretKey = jwt_key;
        _issuer = jwt_issuer;
        _audience = jwt_audience;
    }
    public string generate(User user)
    {

        var claims = new List<Claim>
        {
            new(nameof(User.Id), user.Id.ToString()),    
            new(nameof(User.FirstName), user.FirstName),             
            new(nameof(User.LastName), user.LastName),                
            new(nameof(User.Email), user.Email), 
            new(nameof(User.Role), user.Role),                     
        };

        // Standardized User info
        ClaimsIdentity identity = new(claims,CookieAuthenticationDefaults.AuthenticationScheme);
        ClaimsPrincipal principal = new(identity);




        // Token signing info
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_secretKey));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken token = new(
            issuer: _issuer,
            audience: _audience,
            claims: principal.Claims, 
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token); 
    }
}