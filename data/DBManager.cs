using Oracle.ManagedDataAccess.Client;

namespace gurukul.data;

public class DbManager
{
    private const string Host = "localhost";
    private const string Port = "1521";
    private const string ServiceName = "XE";
    private const string Username = "gurukul";
    private const string Password = "gurukul";

    private const string ConnectionString =
        $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Host})(PORT={Port}))(CONNECT_DATA=(SERVICE_NAME={ServiceName})));User Id={Username};Password={Password};";
    

    public void Execute(string queryString)
    {
        Console.WriteLine("Before Attempting");
        Console.WriteLine(ConnectionString);

        try
        {
            // using (OracleConnection connection = new OracleConnection(_connectionString))
            using (OracleConnection _connection = new OracleConnection(ConnectionString))
            {
                Console.WriteLine("Inside Connection");
                Console.WriteLine(queryString);
                OracleCommand command = new OracleCommand(queryString, _connection);
                Console.WriteLine("Command is set");
                command.Connection.Open();
                Console.WriteLine("Connection is Open");

                command.ExecuteNonQuery();
                Console.WriteLine("Query Executed");
                command.Connection.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("\n\n\n\n");
            Console.WriteLine("Error Occured");
            Console.WriteLine(e.Message);
            Console.WriteLine("\n\n\n\n");
        }
    }

    public void ExecuteGet(string queryString)
    {
        // using (OracleConnection connection = new OracleConnection(_connectionString))
        // using (_connection)
        // {
        //     OracleCommand command = new OracleCommand(queryString, _connection);
        //     command.Connection.Open();
        //
        //     OracleDataReader reader = command.ExecuteReader();
        //     while (reader.Read())
        //     {
        //         reader.GetInt16(0);
        //     }
        //     
        //     reader.Dispose();
        //
        //     command.Connection.Close();
        // }
    }
}