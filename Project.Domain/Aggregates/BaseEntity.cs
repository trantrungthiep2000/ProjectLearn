namespace Project.Domain.Aggregates;

/// <summary>
/// Information of base entity
/// CreatedBy: ThiepTT(30/10/2023)
/// </summary>
public class BaseEntity
{
    protected BaseEntity() { }

    /// <summary>
    /// Created by
    /// </summary>
    public string CreatedBy { get; private set; } = string.Empty;

    /// <summary>
    /// Created date
    /// </summary>
    public DateTime CreatedDate { get; private set; }

    /// <summary>
    /// Updated by
    /// </summary>
    public string UpdatedBy { get; private set;} = string.Empty;

    /// <summary>
    /// Updated date
    /// </summary>
    public DateTime UpdatedDate { get; private set; }

    /// <summary>
    /// Create entity
    /// </summary>
    /// <param name="createdBy">Created by</param>
    /// CreatedBy: ThiepTT(30/10/2023)
    protected void CreateEntity(string createdBy)
    {
        CreatedBy = createdBy;
        CreatedDate = DateTime.UtcNow;
        UpdatedBy = string.Empty;
        UpdatedDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="updatedBy">Updated by</param>
    /// CreatedBy: ThiepTT(30/10/2023)
    protected void UpdateEntity(string updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }
}