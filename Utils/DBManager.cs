using Oracle.ManagedDataAccess.Client;

namespace gurukul.Utils;

public static class DbManager
{
    private const string Host = "localhost";
    private const string Port = "1521";
    private const string ServiceName = "XE";
    private const string Username = "gurukul";
    private const string Password = "gurukul";

    public const string ConnectionString =
        $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Host})(PORT={Port}))(CONNECT_DATA=(SERVICE_NAME={ServiceName})));User Id={Username};Password={Password};";


    public static void Execute(string queryString)
    {
        try
        {
            using OracleConnection connection = new OracleConnection(ConnectionString);
            OracleCommand command = new OracleCommand(queryString, connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}