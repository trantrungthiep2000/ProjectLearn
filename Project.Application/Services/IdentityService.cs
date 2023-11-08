using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Project.Application.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project.Application.Services;

/// <summary>
/// Information of identity service
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class IdentityService
{
    private readonly JwtSettings _jwtSettings;
    private readonly byte[] _key;
    public JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();


    public IdentityService(IOptions<JwtSettings> options)
    {
        _jwtSettings = options.Value;
        _key = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);
    }

    /// <summary>
    /// Create security token
    /// </summary>
    /// <param name="identity">ClaimsIdentity</param>
    /// <returns>SecurityToken</returns>
    /// CreatedBy: ThiepTT(31/10/2023)
    public SecurityToken CreateSecurityToken(ClaimsIdentity identity)
    {
        var tokenDescriptor = GetTokenDescriptor(identity);

        return TokenHandler.CreateToken(tokenDescriptor);
    }

    /// <summary>
    /// Write token
    /// </summary>
    /// <param name="token">SecurityToken</param>
    /// <returns>Token</returns>
    /// CreatedBy: ThiepTT(31/10/2023)
    public string WriteToken(SecurityToken token)
    {
        return TokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Get token descriptior
    /// </summary>
    /// <param name="identity">ClaimsIdentity</param>
    /// <returns>SecurityTokenDescriptor</returns>
    /// CreatedBy: ThiepTT(31/10/2023)
    private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity identity)
    {
        return new SecurityTokenDescriptor()
        {
            Subject = identity,
            Expires = DateTime.UtcNow.AddHours(2),
            Audience = _jwtSettings.Audiences[0],
            Issuer = _jwtSettings.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key),
                SecurityAlgorithms.HmacSha256Signature)
        };
    }
}