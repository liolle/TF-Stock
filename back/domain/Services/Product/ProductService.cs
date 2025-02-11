using Microsoft.Data.SqlClient;
using stock.CQS;
using stock.dal.database;
using stock.domain.Commands;
using stock.domain.entities;
using stock.domain.queries;

namespace stock.domain.services;

public class ProductService(IDataContext context) : IProductService
{
    public CommandResult Execute(AddProductCommand command)
    {
        try
        {
            ProductByGTINQuery productQuery = new(
                command.GTIN
            );
            var result = Execute(productQuery);
            if (result.IsFailure && result.ErrorMessage != "Could not corresponding product") {
                return ICommandResult.Failure(result.ErrorMessage!,result.Exception);
            }

            using SqlConnection conn = context.CreateConnection();
            conn.Open();
            if (result.IsFailure ) {
                // create new element 
                using SqlCommand cmd = new SqlCommand("InsertProductAndTransaction", conn){
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Quantity", command.Quantity);
                cmd.Parameters.AddWithValue("@Price", command.Price);
                cmd.Parameters.AddWithValue("@Name", command.Name);
                cmd.Parameters.AddWithValue("@GTIN", command.GTIN);
                cmd.Parameters.AddWithValue("@UserId", 1);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected <1) {return ICommandResult.Failure("Failed to add product");}
            }else
            {
                // update existing element
                using SqlCommand cmd = new SqlCommand("AddProductAndTransaction", conn){
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Quantity", command.Quantity);
                cmd.Parameters.AddWithValue("@ProductId", result.Result.Id);
                cmd.Parameters.AddWithValue("@UserId", 1);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected <1) {return ICommandResult.Failure("Failed to add product");}
            }
            
            return ICommandResult.Success();
        }
        catch (Exception e)
        {
            return ICommandResult.Failure(e.Message);
        }
    }

    public QueryResult<ProductEntity> Execute(ProductByGTINQuery query)
    {
        try
        {
            using SqlConnection conn = context.CreateConnection();
            string sql_query = "SELECT Id, Quantity, GTIN, Price, Name, LastModified, Transactions FROM Products WHERE GTIN = @GTIN";
            using SqlCommand cmd = new SqlCommand(sql_query, conn);
            cmd.Parameters.AddWithValue("@GTIN", query.GTIN);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                ProductEntity product = new(
                    (int)reader[nameof(ProductEntity.Id)],
                    (int)reader[nameof(ProductEntity.Quantity)],
                    (string)reader[nameof(ProductEntity.GTIN)],
                    (int)reader[nameof(ProductEntity.Price)],
                    (string)reader[nameof(ProductEntity.Name)],
                    (DateTime)reader[nameof(ProductEntity.LastModified)],
                    (int)reader[nameof(ProductEntity.Transactions)]
                );
                return IQueryResult<ProductEntity>.Success(product);
            }

            return IQueryResult<ProductEntity>.Failure("Could not corresponding product");
            
        }
        catch (Exception e)
        {
           return IQueryResult<ProductEntity>.Failure(e.Message);
        }
    }

    public CommandResult Execute(UpdateProductCommand command)
    {
        try
        {
            using SqlConnection conn = context.CreateConnection();
            conn.Open();
            using SqlCommand cmd = new SqlCommand("ChangeProductAndTransaction", conn){
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Quantity", command.Quantity);
            cmd.Parameters.AddWithValue("@ProductId", command.Id);
            cmd.Parameters.AddWithValue("@UserId", 1);
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected <1) {return ICommandResult.Failure("Failed to Update product");}
            
            return ICommandResult.Success();
        }
        catch (Exception e)
        {
            return ICommandResult.Failure(e.Message);
        }
    }

    public CommandResult Execute(ConsumeProductCommand command)
    {
        try
        {
            using SqlConnection conn = context.CreateConnection();
            conn.Open();
            using SqlCommand cmd = new SqlCommand("OutputProductAndTransaction", conn){
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Quantity", command.Quantity);
            cmd.Parameters.AddWithValue("@ProductId", command.Id);
            cmd.Parameters.AddWithValue("@UserId", 1);
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected <1) {return ICommandResult.Failure($"Could not consume {command.Quantity} product");}
            
            return ICommandResult.Success();
        }
        catch (Exception e)
        {
            return ICommandResult.Failure(e.Message);
        }
    }

    public QueryResult<ICollection<ProductEntity>> Execute(AllProductQuery query)
    {
        List<ProductEntity> products = [];

        try
        {
            using SqlConnection conn = context.CreateConnection();
            string sql_query = "SELECT Id, Quantity, GTIN, Price, Name, LastModified, Transactions FROM Products";
            using SqlCommand cmd = new SqlCommand(sql_query, conn);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                products.Add(new ProductEntity(
                    (int)reader[nameof(ProductEntity.Id)],
                    (int)reader[nameof(ProductEntity.Quantity)],
                    (string)reader[nameof(ProductEntity.GTIN)],
                    (int)reader[nameof(ProductEntity.Price)],
                    (string)reader[nameof(ProductEntity.Name)],
                    (DateTime)reader[nameof(ProductEntity.LastModified)],
                    (int)reader[nameof(ProductEntity.Transactions)]
                ));

            }
        }
        catch (Exception e)
        {
            return IQueryResult<ICollection<ProductEntity>>.Failure(e.Message,e);
        }

        return IQueryResult<ICollection<ProductEntity>>.Success(products);
    }

    public QueryResult<ICollection<TransactionEntity>> Execute(TransactionByProductIdQuery query)
    {
        List<TransactionEntity> transactions = [];

        try
        {
            using SqlConnection conn = context.CreateConnection();
            string sql_query = "SELECT Id, Type, Quantity, Date, ProductId, UserId FROM Transactions WHERE ProductId = @ProductId";
            using SqlCommand cmd = new SqlCommand(sql_query, conn);
            cmd.Parameters.AddWithValue("@ProductId",query.Id);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                TransactionEntity product = new(
                    (int)reader[nameof(TransactionEntity.Id)],
                    (string)reader[nameof(TransactionEntity.Type)],
                    (int)reader[nameof(TransactionEntity.Quantity)],
                    (DateTime)reader[nameof(TransactionEntity.Date)],
                    (int)reader[nameof(TransactionEntity.ProductId)],
                    (int)reader[nameof(TransactionEntity.UserId)]
                );
                transactions.Add(product);
            }
        }
        catch (Exception e)
        {
            return IQueryResult<ICollection<TransactionEntity>>.Failure(e.Message,e);
        }

        return IQueryResult<ICollection<TransactionEntity>>.Success(transactions);
    }
}