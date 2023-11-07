namespace Project.Application.Messages;

/// <summary>
/// Information of response message
/// CreatedBy: ThiepTT(07/11/2023)
/// </summary>
public class ResponseMessage
{
    /// <summary>
    /// Information of identity
    /// CreatedBy: ThiepTT(07/11/2023)
    /// </summary>
    public class Identity
    {
        /// <summary>
        /// Register success
        /// </summary>
        public const string RegisterSuccess = "Register user success";
    }

    /// <summary>
    /// Information of user profile
    /// CreatedBy: ThiepTT(07/11/2023)
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// Remove account success
        /// </summary>
        public const string RemoveAccountSuccess = "Remove account success";

        /// <summary>
        /// Update account success
        /// </summary>
        public const string UpdateAccountSuccess = "Update account success";
    }
}