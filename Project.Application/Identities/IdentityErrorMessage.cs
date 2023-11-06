namespace Project.Application.Identities;

/// <summary>
/// Information of identity error message
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public static class IdentityErrorMessage
{
    public static string IdentityUserAlreadyExists = "This email has been registered";
    public static string IdentityUserIncorrectPassword = "Email or password is incorrect";
    public static string IdentityUserNotExists = "This email has not been registered";
    public static string InternalServerError = "An error occurred and try again";
}