using stock.CQS;
using stock.domain.entities;

namespace stock.domain.Queries;

public class UserFromUserNameQuery(string userName) : IQueryDefinition<User?>
{
    public string UserName { get; } = userName;

}