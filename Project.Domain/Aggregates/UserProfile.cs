namespace Project.Domain.Aggregates;

/// <summary>
/// Information of user profile
/// CreatedBy: ThiepTT(30/10/2023)
/// </summary>
public class UserProfile 
{
    public UserProfile() { }

    /// <summary>
    /// Id of user profile
    /// </summary>
    public Guid UserProfileId { get; private set; }

    /// <summary>
    /// Full name
    /// </summary>
    public string FullName { get; private set; } = string.Empty;

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>
    /// Phone number
    /// </summary>
    public string PhoneNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Date of birth
    /// </summary>
    public DateTime DateOfBirth { get; private set; }

    /// <summary>
    /// Create user profile
    /// </summary>
    /// <param name="fullName">Full name</param>
    /// <param name="email">Email</param>
    /// <param name="phoneNumber">Phone number</param>
    /// <param name="dateOfBirth">Date of birth</param>
    /// <returns>UserProfile</returns>
    /// CreatedBy: ThiepTT(30/10/2023)
    public static UserProfile CreateUserProfile(string fullName, string email, string phoneNumber, DateTime dateOfBirth)
    {
        UserProfile userProfile = new UserProfile()
        {
            FullName = fullName,
            Email = email,
            PhoneNumber = phoneNumber,
            DateOfBirth = dateOfBirth,
        };

        return userProfile;
    }

    /// <summary>
    /// Update user profile
    /// </summary>
    /// <param name="fullName">Full name</param>
    /// <param name="phoneNumber">Phone number</param>
    /// <param name="dateOfBirth">Date of birth</param>
    /// CreatedBy: ThiepTT(30/10/2023)
    public void UpdateUserProfile(string fullName, string phoneNumber, DateTime dateOfBirth)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
        DateOfBirth = dateOfBirth;
    }
}