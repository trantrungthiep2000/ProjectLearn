namespace Project.Application.Users;

/// <summary>
/// Information of user profile error message
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public static class UserProfileErrorMessage
{
    /// <summary>
    /// User profile not found
    /// </summary>
    public const string UserProfileNotFound = "No find UserProfile with ID {0}";
        /// <summary>
        /// User profile by email not found
        /// </summary>
    public const string UserProfileByEmailNotFound = "No find UserProfile with email {0}";
}