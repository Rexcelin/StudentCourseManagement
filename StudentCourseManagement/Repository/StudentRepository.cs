
using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repository;

public class StudentRepository
{
    public async Task<int> AddAsync(Student student)
    {
        const string sql = """
            INSERT INTO Students
                (Name, Email, Age, DepartmentId)
            VALUES
                (@Name, @Email, @Age, @DepartmentId);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@Name", student.Name);
        command.Parameters.AddWithValue("@Email", student.Email);
        command.Parameters.AddWithValue("@Age", student.Age);
        command.Parameters.AddWithValue("@DepartmentId", student.DepartmentId);

        await connection.OpenAsync();

        object? result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<List<Student>> GetAllAsync()
    {
        const string sql = """
            SELECT StudentId, Name, Email, Age, DepartmentId,
                   AdmissionDate, IsActive
            FROM Students
            ORDER BY StudentId;
            """;

        var students = new List<Student>();

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            students.Add(new Student
            {
                StudentId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Age = reader.GetInt32(3),
                DepartmentId = reader.GetInt32(4),
                AdmissionDate = reader.GetDateTime(5),
                IsActive = reader.GetBoolean(6)
            });
        }

        return students;
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT StudentId, Name, Email, Age, DepartmentId,
                   AdmissionDate, IsActive
            FROM Students
            WHERE StudentId = @StudentId;
            """;

        await using var connection = DatabaseHelper.CreateConnection();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@StudentId", id);

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new Student
        {
            StudentId = reader.GetInt32(0),
            Name = reader.GetString(1),
            Email = reader.GetString(2),
            Age = reader.GetInt32(3),
            DepartmentId = reader.GetInt32(4),
            AdmissionDate = reader.GetDateTime(5),
            IsActive = reader.GetBoolean(6)
        };
    }
}
