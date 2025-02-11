namespace stock.domain.entities;

public class ProductEntity(int id, int quantity, string gTIN, int price, string name, DateTime lastModified, int transactions)
{
    public int Id { get; } = id;

    public int Quantity { get; set; } = quantity;

    public string GTIN { get; set; } = gTIN;

    public int Price { get; set; } = price;

    public string Name { get; set; } = name;

    public DateTime LastModified { get; set; } = lastModified;

    public int Transactions { get; set; } = transactions;
}