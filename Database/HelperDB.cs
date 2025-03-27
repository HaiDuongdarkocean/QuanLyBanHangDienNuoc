using System;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

public static class HelperDB
{
    private static string _connectionString;

    static HelperDB()
    {
        try
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _connectionString = config.GetConnectionString("DefaultConnection");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi khi đọc cấu hình database: " + ex.Message);
            _connectionString = "";
        }
    }

    public static MySqlConnection GetConnection()
    {
        return new MySqlConnection(_connectionString);
    }

    public static bool RecordExists(string query, MySqlParameter[] parameters = null)
    {
        object result = ExecuteScalar(query, parameters);
        return result != null && Convert.ToInt32(result) > 0;
    }

    public static DataTable ExecuteQuery(string query, MySqlParameter[] parameters = null)
    {
        using (var conn = GetConnection())
        {
            conn.Open();
            using (var cmd = new MySqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }

    public static int ExecuteNonQuery(string query, MySqlParameter[] parameters = null)
    {
        using (var conn = GetConnection())
        {
            conn.Open();
            using (var cmd = new MySqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                return cmd.ExecuteNonQuery();
            }
        }
    }

    public static object ExecuteScalar(string query, MySqlParameter[] parameters = null)
    {
        using (var conn = GetConnection())
        {
            conn.Open();
            using (var cmd = new MySqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                return cmd.ExecuteScalar();
            }
        }
    }

    public static bool RecordExists(string tableName, string columnName, object value)
{
    string query = $"SELECT COUNT(1) FROM {tableName} WHERE {columnName} = @Value";
    MySqlParameter[] parameters = { new MySqlParameter("@Value", value) };
    object result = HelperDB.ExecuteScalar(query, parameters);
    return Convert.ToInt32(result) > 0;
}

}
