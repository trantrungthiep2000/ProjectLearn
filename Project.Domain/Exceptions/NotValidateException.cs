namespace Project.Domain.Exceptions;

/// <summary>
/// Information of not validation exception
/// CreatedBy: ThiepTT(30/10/2023)
/// </summary>
public class NotValidateException : Exception
{
    internal NotValidateException()
    {
        ValidationErrors = new List<string>();
    }

    internal NotValidateException(string message) : base(message)
    {
        ValidationErrors = new List<string>();
    }    

    internal NotValidateException(string message, Exception exception) : base(message, exception)
    {
        ValidationErrors = new List<string>();
    }

    /// <summary>
    /// Validation errors
    /// </summary>
    public List<string> ValidationErrors { get; }
}