using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project.Application.Models;

/// <summary>
/// Information of error response
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Status code
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Status phrase
    /// </summary>
    public string StatusPhrase { get; set; } = string.Empty;

    /// <summary>
    /// Errors
    /// </summary>
    public List<string> Errors { get; } = new List<string>();

    /// <summary>
    /// Time stamp
    /// </summary>
    public DateTime TimeStamp { get; set; }
}