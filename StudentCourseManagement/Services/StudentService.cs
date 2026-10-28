
using StudentCourseManagement.Models;
using StudentCourseManagement.Repository;

namespace StudentCourseManagement.Services;

public class StudentService
{
    private readonly StudentRepository _repository;

    public StudentService(StudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> AddStudentAsync(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.Name))
            throw new ArgumentException("Student name is required.");

        if (string.IsNullOrWhiteSpace(student.Email))
            throw new ArgumentException("Student email is required.");

        if (student.Age is < 17 or > 60)
            throw new ArgumentOutOfRangeException(
                nameof(student.Age),
                "Age must be between 17 and 60.");

        if (student.DepartmentId <= 0)
            throw new ArgumentException(
                "A valid department ID is required.");

        return await _repository.AddAsync(student);
    }

    public Task<int> RegisterAsync(Student student)
    {
        return AddStudentAsync(student);
    }

    public Task<List<Student>> GetAllStudentsAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<Student?> GetStudentByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Student ID must be positive.");

        return _repository.GetByIdAsync(id);
    }
}
