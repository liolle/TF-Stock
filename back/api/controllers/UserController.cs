using stock.domain.Commands;
using stock.domain.Queries;
using stock.domain.services;
using Microsoft.AspNetCore.Mvc;
using stock.CQS;

public class UserController(IUserService userService,IConfiguration configuration) : ControllerBase
{
    [HttpPost]
    public IActionResult Register([FromBody] RegisterCommand command){
        return Ok(userService.Execute(command));
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginQuery query){
        
        string? token_name = configuration["AUTH_TOKEN_NAME"] ?? throw new Exception("Missing AUTH_TOKEN_NAME Configuration");
        QueryResult<string> result = userService.Execute(query);
        if (result.IsFailure){return BadRequest(result);}

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, 
            Secure = true,  
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(1) 
        };

        Response.Cookies.Append(token_name, result.Result, cookieOptions);
        return Ok(IQueryResult<string>.Success(result.Result));
    }

    [HttpGet]
    public IActionResult Logout(){
        string? token_name = configuration["AUTH_TOKEN_NAME"] ?? throw new Exception("Missing AUTH_TOKEN_NAME Configuration");
        Response.Cookies.Delete(token_name);
        return Ok(ICommandResult.Success());
    }
}