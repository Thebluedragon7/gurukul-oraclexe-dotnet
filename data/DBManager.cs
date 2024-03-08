using Oracle.ManagedDataAccess.Client;

namespace gurukul.data;

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

    public static void ExecuteGet(string queryString)
    {
        using OracleConnection connection = new OracleConnection(ConnectionString);
        OracleCommand command = new OracleCommand(queryString, connection);
        command.Connection.Open();

        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            reader.GetInt16(0);
            reader.GetString(1);
            reader.GetDateTime(2);
        }

        reader.Dispose();

        command.Connection.Close();
    }
}