using stock.CQS;
using stock.domain.entities;

namespace stock.domain.queries;

public class ProductByGTINQuery(string gTIN) : IQueryDefinition<ProductEntity>
{
    public string GTIN { get;} = gTIN;
}