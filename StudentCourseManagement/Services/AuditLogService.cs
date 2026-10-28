using StudentCourseManagement.Models;
using StudentCourseManagement.Repository;

namespace StudentCourseManagement.Services;

public class AuditLogService
{
    private readonly AuditLogRepository _repository;

    public AuditLogService(AuditLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<long> AddAsync(AuditLog auditLog)
    {
        if (string.IsNullOrWhiteSpace(auditLog.EntityName))
            throw new ArgumentException("Entity name is required.");

        if (auditLog.EntityId <= 0)
            throw new ArgumentException("Entity ID must be greater than zero.");

        if (string.IsNullOrWhiteSpace(auditLog.Action))
            throw new ArgumentException("Action is required.");

        auditLog.EntityName = auditLog.EntityName.Trim();
        auditLog.Action = auditLog.Action.Trim();

        if (auditLog.Details != null)
        {
            auditLog.Details = auditLog.Details.Trim();

            if (auditLog.Details == "")
                auditLog.Details = null;
        }

        return await _repository.AddAsync(auditLog);
    }

    public Task<List<AuditLog>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public async Task<List<AuditLog>> GetByEntityAsync(
        string entityName,
        int entityId)
    {
        if (string.IsNullOrWhiteSpace(entityName))
            throw new ArgumentException("Entity name is required.");

        if (entityId <= 0)
            throw new ArgumentException("Entity ID must be greater than zero.");

        return await _repository.GetByEntityAsync(
            entityName.Trim(), entityId);
    }
}