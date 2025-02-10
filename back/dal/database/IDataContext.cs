using Microsoft.Data.SqlClient;
namespace stock.dal.database;

public interface IDataContext {
    SqlConnection CreateConnection();
}