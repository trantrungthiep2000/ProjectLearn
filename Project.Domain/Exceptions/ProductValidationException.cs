namespace Project.Domain.Exceptions;

/// <summary>
/// Information of product validation exception
/// CreatedBy: ThiepTT(30/10/2023)
/// </summary>
public class ProductValidationException : NotValidateException
{
    internal ProductValidationException() { }

    internal ProductValidationException(string message) : base(message) { }

    internal ProductValidationException(string message, Exception exception) : base(message, exception) { }
}