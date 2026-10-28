
using StudentCourseManagement.Models;
using StudentCourseManagement.Repository;

namespace StudentCourseManagement.Services;

public class EnrollmentService
{
    private readonly EnrollmentRepository _repository;

    public EnrollmentService(EnrollmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> AddAsync(Enrollment enrollment)
    {
        if (enrollment.StudentId <= 0)
            throw new ArgumentException(
                "Student ID must be greater than zero.");

        if (enrollment.CourseId <= 0)
            throw new ArgumentException(
                "Course ID must be greater than zero.");

        // Match the database CHECK constraint.
        enrollment.Status = string.IsNullOrWhiteSpace(enrollment.Status)
            ? "Active"
            : enrollment.Status.Trim();

        ValidateStatus(enrollment.Status);

        return await _repository.AddAsync(enrollment);
    }

    public async Task<int> EnrollStudentAsync(int studentId, int courseId)
    {
        if (studentId <= 0)
            throw new ArgumentException(
                "Student ID must be greater than zero.");

        if (courseId <= 0)
            throw new ArgumentException(
                "Course ID must be greater than zero.");

        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            Status = "Active"
        };

        return await AddAsync(enrollment);
    }

    public Task<List<Enrollment>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public async Task<Enrollment?> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException(
                "Enrollment ID must be greater than zero.");

        return await _repository.GetByIdAsync(id);
    }

    public async Task<List<Enrollment>> GetByStudentIdAsync(int studentId)
    {
        if (studentId <= 0)
            throw new ArgumentException(
                "Student ID must be greater than zero.");

        return await _repository.GetByStudentIdAsync(studentId);
    }

    public async Task<bool> UpdateStatusAsync(
        int enrollmentId, string status)
    {
        if (enrollmentId <= 0)
            throw new ArgumentException(
                "Enrollment ID must be greater than zero.");

        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status is required.");

        status = status.Trim();
        ValidateStatus(status);

        return await _repository.UpdateStatusAsync(
            enrollmentId, status);
    }

    private static void ValidateStatus(string status)
    {
        if (status != "Active" &&
            status != "Completed" &&
            status != "Cancelled")
        {
            throw new ArgumentException(
                "Status must be Active, Completed, or Cancelled.");
        }
    }
}
