using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;
using System.Data;

namespace StudentCourseManagement.Repository;

public class AuditLogRepository
{
    public async Task<long> AddAsync(AuditLog auditLog)
    {
        const string sql = """
            INSERT INTO AuditLogs (EntityName, EntityId, [Action], Details)
            VALUES (@EntityName, @EntityId, @Action, @Details);

            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);
            """;

        await using SqlConnection connection = DatabaseHelper.CreateConnection();
        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@EntityName", SqlDbType.NVarChar, 100)
            .Value = auditLog.EntityName;

        command.Parameters.Add("@EntityId", SqlDbType.Int)
            .Value = auditLog.EntityId;

        command.Parameters.Add("@Action", SqlDbType.NVarChar, 50)
            .Value = auditLog.Action;

        command.Parameters.Add("@Details", SqlDbType.NVarChar, -1)
            .Value = (object?)auditLog.Details ?? DBNull.Value;

        await connection.OpenAsync();

        object? result = await command.ExecuteScalarAsync();

        return Convert.ToInt64(result);
    }

    public async Task<List<AuditLog>> GetAllAsync()
    {
        const string sql = """
            SELECT AuditId, EntityName, EntityId, [Action], Details, CreatedAt
            FROM AuditLogs
            ORDER BY CreatedAt DESC;
            """;

        List<AuditLog> auditLogs = new List<AuditLog>();

        await using SqlConnection connection = DatabaseHelper.CreateConnection();
        await using SqlCommand command = new(sql, connection);

        await connection.OpenAsync();

        await using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            AuditLog auditLog = new AuditLog
            {
                AuditId = reader.GetInt64(0),
                EntityName = reader.GetString(1),
                EntityId = reader.GetInt32(2),
                Action = reader.GetString(3),
                Details = reader.IsDBNull(4) ? null : reader.GetString(4),
                CreatedAt = reader.GetDateTime(5)
            };

            auditLogs.Add(auditLog);
        }

        return auditLogs;
    }

    public async Task<List<AuditLog>> GetByEntityAsync(
        string entityName,
        int entityId)
    {
        const string sql = """
            SELECT AuditId, EntityName, EntityId, [Action], Details, CreatedAt
            FROM AuditLogs
            WHERE EntityName = @EntityName
              AND EntityId = @EntityId
            ORDER BY CreatedAt DESC;
            """;

        List<AuditLog> auditLogs = new List<AuditLog>();

        await using SqlConnection connection = DatabaseHelper.CreateConnection();
        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@EntityName", SqlDbType.NVarChar, 100)
            .Value = entityName;

        command.Parameters.Add("@EntityId", SqlDbType.Int)
            .Value = entityId;

        await connection.OpenAsync();

        await using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            auditLogs.Add(new AuditLog
            {
                AuditId = reader.GetInt64(0),
                EntityName = reader.GetString(1),
                EntityId = reader.GetInt32(2),
                Action = reader.GetString(3),
                Details = reader.IsDBNull(4) ? null : reader.GetString(4),
                CreatedAt = reader.GetDateTime(5)
            });
        }

        return auditLogs;
    }
}