namespace Project.Application.Dtos.Requests;

/// <summary>
/// Information of register request
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// Full name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Phone number
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Date of birth
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; } = string.Empty;
}