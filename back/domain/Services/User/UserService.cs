using stock.domain.Commands;
using stock.domain.Queries;
using Microsoft.Data.SqlClient;
using stock.CQS;
using stock.dal.database;
using stock.domain.entities;
using stock.domain.services;

namespace stock.domain.services;

public class UserService(IDataContext context, IHashService hashService, IJWTService jwt) : IUserService
{
    public CommandResult Execute(RegisterCommand command)
    {
        try
        {
            using SqlConnection conn = context.CreateConnection();
            string hashedPassword = hashService.HashPassword(command.Email,command.Password);

            string query = @"
                INSERT INTO Users (FirstName, LastName, Email, UserName, Password) 
                VALUES (@FirstName, @LastName, @Email, @UserName, @Password);";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FirstName", command.FirstName);
            cmd.Parameters.AddWithValue("@LastName", command.LastName);
            cmd.Parameters.AddWithValue("@Email", command.Email);
            cmd.Parameters.AddWithValue("@UserName", command.UserName); 
            cmd.Parameters.AddWithValue("@Password", hashedPassword); 

            conn.Open();
            int result = cmd.ExecuteNonQuery(); 
            if (result != 1) {
                return ICommandResult.Failure("User insertion failed.");
            }
            return ICommandResult.Success();
        }
        catch (Exception e)
        {
            return ICommandResult.Failure(e.Message);
        }
    }

    public QueryResult<string> Execute(LoginQuery query)
    {
        try
        {
            QueryResult<User?> qr = Execute(new UserFromUserNameQuery(query.UserName));
            if (qr.IsFailure || qr.Result is null ){
                return IQueryResult<string>.Failure(qr.ErrorMessage!,qr.Exception);
            }

            if (!hashService.VerifyPassword(query.UserName,qr.Result.Password,query.Password)){
                return IQueryResult<string>.Failure("Invalid credential combination");
            }

            return IQueryResult<string>.Success(jwt.generate(qr.Result));
        }
        catch (Exception e)
        {
            return IQueryResult<string>.Failure(e.Message);
        }
    }

    public QueryResult<User?>  Execute(UserFromUserNameQuery query)
    {
        try
        {
            using SqlConnection conn = context.CreateConnection();
            string sql_query = "SELECT * FROM Users WHERE UserName = @UserName";
            using SqlCommand cmd = new(sql_query, conn);
            cmd.Parameters.AddWithValue("@UserName", query.UserName);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                User u = new(
                    (int)reader[nameof(User.Id)],
                    (string)reader[nameof(User.FirstName)],
                    (string)reader[nameof(User.LastName)],
                    (string)reader[nameof(User.Email)],
                    (string)reader[nameof(User.UserName)],
                    (string)reader[nameof(User.Role)],
                    (string)reader[nameof(User.Password)]
                );
                return IQueryResult<User?>.Success(u);
            }
            return IQueryResult<User?>.Failure($"Could not find UserName {query.UserName}");
        }
        catch (Exception e)
        {
            return IQueryResult<User?>.Failure(e.Message);
        }
    }
}