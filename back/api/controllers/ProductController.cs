using Microsoft.AspNetCore.Mvc;
using stock.api.models;
using stock.domain.Commands;
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
        return Ok(_ps.Execute(command));
    }
}