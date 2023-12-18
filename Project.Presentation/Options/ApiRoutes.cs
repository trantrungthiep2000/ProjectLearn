namespace Project.Presentation.Options;

/// <summary>
/// Information of api routes
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public static class ApiRoutes
{
    /// <summary>
    /// Base router
    /// </summary>
    public const string BaseRouter = "api/v{version:apiversion}/[controller]";

    /// <summary>
    /// Api
    /// </summary>
    public const string Api = "api/v1";

    /// <summary>
    /// Time to live
    /// </summary>
    public const int TimeToLive = 3600;

    /// <summary>
    /// Information of version
    /// CreatedBy: ThiepTT(07/11/2023)
    /// </summary>
    public class Version
    {
        /// <summary>
        /// V1
        /// </summary>
        public const string V1 = "1.0";

        /// <summary>
        /// V2
        /// </summary>
        public const string V2 = "2.0";
    }

    /// <summary>
    /// Information of role
    /// CreatedBy: ThiepTT(02/11/2023)
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

    /// <summary>
    /// Information of authentications
    /// CreatedBy: ThiepTT(31/10/2023)
    /// </summary>
    public class Authentications
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
    /// Information of user profiles
    /// CreatedBy: ThiepTT(02/11/2023)
    /// </summary>
    public class UserProfiles
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
        public const string GetUserProfileById = "GetUserProfileById/{userProfileId}";

        /// <summary>
        /// Update user profile by id
        /// </summary>
        public const string UpdateUserProfileById = "UpdateUserProfileById/{userProfileId}";

        /// <summary>
        /// Remove user profile by id
        /// </summary>
        public const string RemoveUserProfileById = "RemoveUserProfileById/{userProfileId}";

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
    /// Information of test
    /// CreatedBy: ThiepTT(07/11/2023)
    /// </summary>
    public class Test
    {
        /// <summary>
        /// Test v2
        /// </summary>
        public const string TestV2 = "TestV2";
    }

    /// <summary>
    /// Information of products
    /// CreatedBy: ThiepTT(07/11/2023)
    /// </summary>
    public class Products
    {
        /// <summary>
        /// Get all products
        /// </summary>
        public const string GetAllProducts = "GetAllProducts";

        /// <summary>
        /// Get product by id
        /// </summary>
        public const string GetProductById = "GetProductById/{productId}";

        /// <summary>
        /// Create bulk product
        /// </summary>
        public const string CreateBulkProduct = "CreateBulkProduct";

        /// <summary>
        /// Create product
        /// </summary>
        public const string CreateProduct = "CreateProduct";

        /// <summary>
        /// Update product
        /// </summary>
        public const string UpdateProduct = "UpdateProduct/{productId}";

        /// <summary>
        /// Delete bulk product
        /// </summary>
        public const string DeleteBulkProduct = "DeleteBulkProduct";

        /// <summary>
        /// Delete product
        /// </summary>
        public const string DeleteProduct = "DeleteProduct/{productId}";
    }
}