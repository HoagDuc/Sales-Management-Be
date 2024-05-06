using System.Data;
using Dapper;
using Npgsql;

namespace ptdn_net.Utils;

public static class DapperUtil
{
    public static async Task<IEnumerable<TK>> QueryAsync<TK>(
        string query,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = CommandType.Text
    )
    {
        await using var connection = new NpgsqlConnection(ConfigUtil.ConnectionStr);
        await connection.OpenAsync();
        return await connection.QueryAsync<TK>(query, param, transaction, commandTimeout, commandType);
    }

    public static async Task<TK> QueryFirstAsync<TK>(
        string query,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = CommandType.Text
    )
    {
        await using var connection = new NpgsqlConnection(ConfigUtil.ConnectionStr);
        await connection.OpenAsync();
        return await connection.QueryFirstAsync<TK>(query, param, transaction, commandTimeout, commandType);
    }

    public static async Task<int> ExecuteAsync(
        string query,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = CommandType.Text
    )
    {
        await using var connection = new NpgsqlConnection(ConfigUtil.ConnectionStr);
        await connection.OpenAsync();
        return await connection.ExecuteAsync(query, param, transaction, commandTimeout, commandType);
    }
}