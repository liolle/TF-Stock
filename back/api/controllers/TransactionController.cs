using Microsoft.AspNetCore.Mvc;
using stock.api.models;
using stock.CQS;
using stock.domain.entities;
using stock.domain.queries;
using stock.domain.services;

public class TransactionController(IProductService _ps) : ControllerBase
{

    [HttpGet]
    public IActionResult ByProduct([FromQuery] GetTransactionByProductIdQuery model){
        TransactionByProductIdQuery query = new(model.Id);
        QueryResult<ICollection<TransactionEntity>> result = _ps.Execute(query);
        return Ok(result);
    }
}