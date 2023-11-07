namespace Project.Application.Messages;

/// <summary>
/// Information of error message
/// CreatedBy: ThiepTT(07/11/2023)
/// </summary>
public static class ErrorMessage
{
    /// <summary>
    /// Information of identity
    /// CreatedBy: ThiepTT(07/11/2023)
    /// </summary>
    public class Identity
    {
        /// <summary>
        /// Identity user already exists
        /// </summary>
        public const string IdentityUserAlreadyExists = "This email has been registered";

        /// <summary>
        /// Identity user incorrect password
        /// </summary>
        public const string IdentityUserIncorrectPassword = "Email or password is incorrect";

        /// <summary>
        /// Identity user not exists
        /// </summary>
        public const string IdentityUserNotExists = "This email has not been registered";

        /// <summary>
        /// Internal server error
        /// </summary>
        public const string InternalServerError = "An error occurred and try again";
    }

    /// <summary>
    /// Information of user profile
    /// CreatedBy: ThiepTT(07/11/2023)
    /// </summary>
    public class UserProfile
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

    /// <summary>
    /// Information of product
    /// CreatedBy: ThiepTT(07/11/2023)
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Product not found
        /// </summary>
        public const string ProductNotFound = "No find Product with ID {0}";
    }
}