
using StudentCourseManagement.Models;
using StudentCourseManagement.Repository;

namespace StudentCourseManagement.Services;

public class DepartmentService
{
    private readonly DepartmentRepository _repository;

    public DepartmentService(DepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> AddAsync(Department department)
    {
        if (string.IsNullOrWhiteSpace(department.DepartmentName))
        {
            throw new ArgumentException("Department name is required.");
        }

        department.DepartmentName = department.DepartmentName.Trim();

        if (department.Location != null)
        {
            department.Location = department.Location.Trim();

            if (department.Location == "")
            {
                department.Location = null;
            }
        }

        return await _repository.AddAsync(department);
    }

    public Task<List<Department>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "Department ID must be greater than zero.");
        }

        return await _repository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(Department department)
    {
        if (department.DepartmentId <= 0)
        {
            throw new ArgumentException(
                "Department ID must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(department.DepartmentName))
        {
            throw new ArgumentException("Department name is required.");
        }

        department.DepartmentName = department.DepartmentName.Trim();

        if (department.Location != null)
        {
            department.Location = department.Location.Trim();

            if (department.Location == "")
            {
                department.Location = null;
            }
        }

        return await _repository.UpdateAsync(department);
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "Department ID must be greater than zero.");
        }

        return await _repository.DeactivateAsync(id);
    }
}
