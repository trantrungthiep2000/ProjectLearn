namespace Project.Application.Models;

/// <summary>
/// Information of operation result
/// </summary>
/// <typeparam name="T">TEntity</typeparam>
/// CreatedBy: ThiepTT(31/10/2023)
public class OperationResult<T>
{
    public T Data { get; set; } = default!;

    public bool IsError { get; private set; } = false;
    public List<Error> Errors { get; } = new List<Error>();

    /// <summary>
    /// Add error
    /// </summary>
    /// <param name="errorCode">Error code</param>
    /// <param name="message">Message</param>
    /// CreatedBy: ThiepTT(31/10/2023)
    public void AddError(ErrorCode errorCode, string message)
    {
        IsError = true;
        Errors.Add(new Error { Code = errorCode, Message = $"{message}" });
    }
}