namespace Project.Application.Models;

/// <summary>
/// Information of error
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class Error
{
    /// <summary>
    /// Code
    /// </summary>
    public ErrorCode Code { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    public string Message { get; set; } = string.Empty;
}