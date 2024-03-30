using MySqlConnector;
using System;
using System.Data;

public class MySqlService
{
    private static readonly Lazy<MySqlService> _instance = new Lazy<MySqlService>(() => new MySqlService());

    public static MySqlService Instance => _instance.Value;

    private readonly string _connectionString;

    private MySqlService()
    {
        _connectionString = "Server=localhost;Database=voremoi;User ID=vore;Password=N!ghtdr3am13;";
    }

    public DataTable ExecuteQuery(string query)
    {
        using MySqlConnection connection = new MySqlConnection(_connectionString);
        using MySqlCommand command = new MySqlCommand(query, connection);

        DataTable dataTable = new DataTable();

        try
        {
            connection.Open();

            using MySqlDataReader reader = command.ExecuteReader();
            dataTable.Load(reader);

            connection.Close();
        }
        catch (Exception ex)
        {
            // Gérer les erreurs de manière appropriée
            Console.WriteLine(ex.ToString());
        }

        return dataTable;
    }
}

