using stock.CQS;
using stock.domain.entities;

namespace stock.domain.queries;

public class TransactionByProductIdQuery(int id) : IQueryDefinition<ICollection<TransactionEntity>>
{
    public int Id { get;} = id;
}