using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Policy = "AdminOnly")]
    public IActionResult Add([FromBody] AddProductModel model){

        string? str_userId = User.FindFirst("Id")?.Value;
        if (str_userId is null) {
            Unauthorized();
        }
        _ = int.TryParse(str_userId,out int userId);

        AddProductCommand command = new(
            model.Name,
            model.Quantity,
            model.GTIN,
            model.Price,
            userId
        );
        CommandResult result = _ps.Execute(command);
        return Ok(result);
    }

    [HttpPatch]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult Update([FromBody] UpdateProductModel model){
        string? str_userId = User.FindFirst("Id")?.Value;
        if (str_userId is null) {
            Unauthorized();
        }
        _ = int.TryParse(str_userId,out int userId);

        UpdateProductCommand command = new(
            model.Id,
            model.Quantity,
            userId
        );
        CommandResult result = _ps.Execute(command);
        return Ok(result);
    }

    [HttpPatch]
    public IActionResult Consume([FromBody] UpdateProductModel model){
        string? str_userId = User.FindFirst("Id")?.Value;
        if (str_userId is null) {
            Unauthorized();
        }
        _ = int.TryParse(str_userId,out int userId);

        ConsumeProductCommand command = new(
            model.Id,
            model.Quantity,
            userId
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