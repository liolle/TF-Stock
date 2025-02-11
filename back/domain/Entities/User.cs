namespace stock.domain.entities;

public class User 
{
    public int Id {get;}
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string Email {get;set;}
    public string Password {get;set;}
    public string Role {get;set;}
    public string UserName {get;set;}

    // Meant to be used only when an user is created.
    internal User(int id, string firstName, string lastName, string email,string password,string role,string userName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Role = role;
        UserName = userName;
    }
}