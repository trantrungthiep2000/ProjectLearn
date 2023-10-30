namespace Project.Domain.Exceptions;

/// <summary>
/// Information of user profile validation exception
/// CreatedBy: ThiepTT(30/10/2023)
/// </summary>
public class UserProfileValidationException : NotValidateException
{
    internal UserProfileValidationException() { }

    internal UserProfileValidationException(string message) : base(message) { }

    internal UserProfileValidationException(string message, Exception exception) : base(message, exception) { } 
}