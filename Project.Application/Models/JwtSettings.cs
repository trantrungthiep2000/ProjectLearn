namespace Project.Application.Models;

/// <summary>
/// Information of json web token settings
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Signing key
    /// </summary>
    public string SigningKey { get; set; } = string.Empty;

    /// <summary>
    /// Issuer
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Audiences
    /// </summary>
    public string[] Audiences { get; set; } = default!;
}