namespace TimeWebApi.Auth;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TimeWebApi.Resources;

public sealed class JwtTokenGenerator
{
    private IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Generate(string userName, IEnumerable<string> userRoles)
    {
        var jwtIssuer = _configuration.GetSection(StaticData.ConfigurationOptions.JwtIssuer).Get<string>()!;
        var jwtKey = _configuration.GetSection(StaticData.ConfigurationOptions.JwtKey).Get<string>()!;

        var claims = new ClaimsIdentity();
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenHandler = new JwtSecurityTokenHandler();

        claims.AddClaim(new Claim(ClaimTypes.Name, userName));
        claims.AddClaims(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = jwtIssuer,
            Issuer = jwtIssuer,
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials,
            Subject = claims
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
}
