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
}