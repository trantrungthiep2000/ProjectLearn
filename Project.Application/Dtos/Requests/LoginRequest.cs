namespace Project.Application.Dtos.Requests;

/// <summary>
/// Information of login request
/// CreatedBy: ThiepTT(01/11/2023)
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; } = string.Empty;
}