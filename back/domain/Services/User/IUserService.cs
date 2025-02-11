using stock.domain.Commands;
using stock.domain.Queries;
using stock.CQS;
using stock.domain.entities;

namespace stock.domain.services;

public interface IUserService : 
    ICommandHandler<RegisterCommand>,
    IQueryHandler<UserFromUserNameQuery,User?>,
    IQueryHandler<LoginQuery,string>
{
}