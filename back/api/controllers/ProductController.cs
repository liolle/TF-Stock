using Microsoft.AspNetCore.Mvc;
using stock.api.models;
using stock.CQS;
using stock.domain.Commands;
using stock.domain.entities;
using stock.domain.queries;
using stock.domain.services;

public class ProductController(IProductService _ps) : ControllerBase
{
    [HttpPost]
    public IActionResult Add([FromBody] AddProductModel model){
        AddProductCommand command = new(
            model.Name,
            model.Quantity,
            model.GTIN,
            model.Price
        );
        CommandResult result = _ps.Execute(command);
        return Ok(result);
    }

    [HttpPatch]
    public IActionResult Update([FromBody] UpdateProductModel model){
        UpdateProductCommand command = new(
            model.Id,
            model.Quantity
        );
        CommandResult result = _ps.Execute(command);
        return Ok(result);
    }

    [HttpPatch]
    public IActionResult Consume([FromBody] UpdateProductModel model){
        ConsumeProductCommand command = new(
            model.Id,
            model.Quantity
        );
        CommandResult result = _ps.Execute(command);
        return Ok(result);
    }

    [HttpGet]
    public IActionResult All(){
        AllProductQuery query = new();
        QueryResult<ICollection<ProductEntity>> result = _ps.Execute(query);
        return Ok(result);
    }
}