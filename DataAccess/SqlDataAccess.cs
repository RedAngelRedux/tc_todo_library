using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace TodoLibrary.DataAccess;

public class SqlDataAccess(IConfiguration config) : ISqlDataAccess
{
    private readonly IConfiguration _config = config;

    public async Task<List<T>> LoadData<T, U>(
        string storedProcedure,
        U parameters,
        string connectionStringName)
    {
        string connectionString = GetValidConnectionString(connectionStringName)!;

        using IDbConnection connection = new SqlConnection(connectionString);

        var rows = await connection.QueryAsync<T>(
            storedProcedure,
            parameters,
            commandType: CommandType.StoredProcedure);

        return rows.ToList();
    }

    public async Task SaveData<T>(
        string storedProcedure,
        T parameters,
        string connectionStringName)
    {
        string connectionString = GetValidConnectionString(connectionStringName)!;

        using IDbConnection connection = new SqlConnection(connectionString);

        // Since we don't have to do anything after this, we don't need to await
        // We call ExecuteAsync() because it might take time, but we'll let the consumer await
        await connection.ExecuteAsync(
            storedProcedure,
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    private string? GetValidConnectionString(string? connectionStringName)
    {
        return (connectionStringName is null)
            ? ";" : _config.GetConnectionString(connectionStringName);
    }
}
