using stock.CQS;
namespace stock.domain.Queries;

public class LoginQuery(string userName, string password) : IQueryDefinition<string>
{
    public string UserName { get; } = userName;
    public string Password { get; } = password;
}