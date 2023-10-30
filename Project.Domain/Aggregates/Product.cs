namespace Project.Domain.Aggregates;

/// <summary>
/// Information of product
/// CreatedBy: ThiepTT(30/10/2023)
/// </summary>
public class Product : BaseEntity
{
    public Product()
    { }

    /// <summary>
    /// Id of product
    /// </summary>
    public Guid ProductId { get; private set; }

    /// <summary>
    /// Name of product
    /// </summary>
    public string ProductName { get; private set; } = string.Empty;

    /// <summary>
    /// Price
    /// </summary>
    public double Price { get; private set; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Create product
    /// </summary>
    /// <param name="productName">Name of product</param>
    /// <param name="price">Price</param>
    /// <param name="description">Description</param>
    /// <param name="createdBy">Created by</param>
    /// <returns>Product</returns>
    /// CreatedBy: ThiepTT(30/10/2023)
    public static Product CreateProduct(string productName, double price, string description, string createdBy)
    {
        Product product = new Product()
        {
            ProductName = productName,
            Price = price,
            Description = description,
        };
        product.CreateEntity(createdBy);

        return product;
    }

    /// <summary>
    /// Update product
    /// </summary>
    /// <param name="productName">Name of product</param>
    /// <param name="price">Price</param>
    /// <param name="description">Description</param>
    /// <param name="updatedBy">Updated by</param>
    /// CreatedBy: ThiepTT(30/10/2023)
    public void UpdateProduct(string productName, double price, string description, string updatedBy)
    {
        ProductName = productName;
        Price = price;
        Description = description;
        UpdateEntity(updatedBy);
    }
}