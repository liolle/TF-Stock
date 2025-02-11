
using stock.CQS;

namespace stock.domain.Commands;

public class RegisterCommand(string firstName, string lastName, string email, string username, string password) : ICommandDefinition
{
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
    public string Email { get; } = email;
    public string UserName { get; } = username;
    public string Password { get; } = password;
}