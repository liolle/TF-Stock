using Microsoft.AspNetCore.Identity;
namespace stock.domain.services;


public class HashService(IPasswordHasher<string> passwordHasher) : IHashService
{
    private readonly IPasswordHasher<string> _passwordHasher = passwordHasher;

    public string HashPassword(string email, string password)
    {
        return _passwordHasher.HashPassword(email, password);
    }

    public bool VerifyPassword(string UserName, string hashedPassword, string password){
        var result = _passwordHasher.VerifyHashedPassword(UserName, hashedPassword, password);
        return result == PasswordVerificationResult.Success;
    }
}