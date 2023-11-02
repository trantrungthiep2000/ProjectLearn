namespace Project.Application.Dtos.Requests;

/// <summary>
/// Information of user profile request
/// CreatedBy: ThiepTT(02/11/2023)
/// </summary>
public class UserProfileRequest
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
}