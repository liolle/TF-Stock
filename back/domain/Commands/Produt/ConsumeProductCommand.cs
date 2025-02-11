using stock.CQS;

namespace stock.domain.Commands;

public class ConsumeProductCommand(int id, int quantity) : ICommandDefinition
{
    public int Id { get; set; } = id;
    public int Quantity { get; set; } = quantity;
}