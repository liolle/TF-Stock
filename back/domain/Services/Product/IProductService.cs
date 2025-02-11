using stock.CQS;
using stock.domain.Commands;
using stock.domain.entities;
using stock.domain.queries;

namespace stock.domain.services;

public interface IProductService : 
    ICommandHandler<AddProductCommand>,
    ICommandHandler<UpdateProductCommand>,
    ICommandHandler<ConsumeProductCommand>,

    IQueryHandler<ProductByGTINQuery,ProductEntity>,
    IQueryHandler<AllProductQuery,ICollection<ProductEntity>>
{
    
}