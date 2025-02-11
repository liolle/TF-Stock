namespace stock.domain.entities;

public class TransactionEntity(int id, string type, int quantity, DateTime date, int productId, int userId)
{
    public int Id { get; } = id;
    public string Type { get; } = type;
    public int Quantity { get; } = quantity;
    public DateTime Date { get; } = date;
    public int ProductId { get; } = productId;
    public int UserId { get; } = userId;
}