using stock.CQS;

namespace stock.domain.Commands;

public class AddProductCommand(string name, int quantity, string gTIN, int price, int userId) : ICommandDefinition
{
    public string Name { get; set; } = name;
    public int Quantity { get; set; } = quantity;
    public string GTIN { get; set; } = gTIN;
    public int Price { get; set; } = price;
    public int UserId {get;set;} = userId;
}