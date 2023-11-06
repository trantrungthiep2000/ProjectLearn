namespace Project.API.Options;

/// <summary>
/// Information of api routes
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public static class ApiRoutes
{
    /// <summary>
    /// Base route
    /// </summary>
    public const string BaseRoute = "api/v1/[controller]";

    /// <summary>
    /// Api
    /// </summary>
    public const string Api = "api/v1";

    /// <summary>
    /// Time to live
    /// </summary>
    public const int TimeToLive = 3600;

    /// <summary>
    /// Information of authentication
    /// CreatedBy: ThiepTT(31/10/2023)
    /// </summary>
    public class Authentication
    {
        /// <summary>
        /// Register
        /// </summary>
        public const string Register = "Register";

        /// <summary>
        /// Login
        /// </summary>
        public const string Login = "Login";
    }

    /// <summary>
    /// Information of user profile
    /// CreatedBy: ThiepTT(02/11/2023)
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// Get all user profiles
        /// </summary>
        public const string GetAllUserProfiles = "GetAllUserProfiles";

        /// <summary>
        /// Get all user profiles entity framework
        /// </summary>
        public const string GetAllUserProfilesEF = "GetAllUserProfilesEF";

        /// <summary>
        /// Get all user profiles entity framework as no tracking
        /// </summary>
        public const string GetAllUserProfilesEFAsNoTracking = "GetAllUserProfilesEFAsNoTracking";

        /// <summary>
        /// Get user profile by id
        /// </summary>
        public const string GetUserProfileById = "GetUserProfileById";

        /// <summary>
        /// Update user profile by id
        /// </summary>
        public const string UpdateUserProfileById = "UpdateUserProfileById";

        /// <summary>
        /// Remove user profile by id
        /// </summary>
        public const string RemoveUserProfileById = "RemoveUserProfileById";

        /// <summary>
        /// Get information of user profile
        /// </summary>
        public const string GetInformationOfUserProfile = "GetInformationOfUserProfile";

        /// <summary>
        /// Update information of user profile
        /// </summary>
        public const string UpdateInformationOfUserProfile = "UpdateInformationOfUserProfile";

        /// <summary>
        /// Remove account
        /// </summary>
        public const string RemoveAccount = "RemoveAcount";
    }

    /// <summary>
    /// Information of role
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Admin
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// User
        /// </summary>
        public const string User = "User";
    }
}