
using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;
using System.Data;

namespace StudentCourseManagement.Repository;

public class EnrollmentRepository
{
    public async Task<int> AddAsync(Enrollment enrollment)
    {
        const string sql = """
            INSERT INTO Enrollments (StudentId, CourseId, Status)
            VALUES (@StudentId, @CourseId, @Status);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@StudentId", SqlDbType.Int).Value =
            enrollment.StudentId;

        command.Parameters.Add("@CourseId", SqlDbType.Int).Value =
            enrollment.CourseId;

        command.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value =
            enrollment.Status;

        await connection.OpenAsync();

        object? result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
    }

    public async Task<List<Enrollment>> GetAllAsync()
    {
        const string sql = """
            SELECT EnrollmentId, StudentId, CourseId,
                   EnrollmentDate, Status
            FROM Enrollments
            ORDER BY EnrollmentId;
            """;

        var enrollments = new List<Enrollment>();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            enrollments.Add(MapEnrollment(reader));
        }

        return enrollments;
    }

    public async Task<Enrollment?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT EnrollmentId, StudentId, CourseId,
                   EnrollmentDate, Status
            FROM Enrollments
            WHERE EnrollmentId = @EnrollmentId;
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@EnrollmentId", SqlDbType.Int).Value = id;

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
            return MapEnrollment(reader);

        return null;
    }

    public async Task<List<Enrollment>> GetByStudentIdAsync(int studentId)
    {
        const string sql = """
            SELECT EnrollmentId, StudentId, CourseId,
                   EnrollmentDate, Status
            FROM Enrollments
            WHERE StudentId = @StudentId
            ORDER BY EnrollmentId;
            """;

        var enrollments = new List<Enrollment>();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@StudentId", SqlDbType.Int).Value =
            studentId;

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            enrollments.Add(MapEnrollment(reader));
        }

        return enrollments;
    }

    public async Task<bool> UpdateStatusAsync(
        int enrollmentId, string status)
    {
        const string sql = """
            UPDATE Enrollments
            SET Status = @Status
            WHERE EnrollmentId = @EnrollmentId;
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@EnrollmentId", SqlDbType.Int).Value =
            enrollmentId;

        command.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value =
            status;

        await connection.OpenAsync();

        int rowsAffected = await command.ExecuteNonQueryAsync();

        return rowsAffected > 0;
    }

    private static Enrollment MapEnrollment(SqlDataReader reader)
    {
        return new Enrollment
        {
            EnrollmentId = reader.GetInt32(0),
            StudentId = reader.GetInt32(1),
            CourseId = reader.GetInt32(2),
            EnrolledAt = reader.GetDateTime(3),
            Status = reader.GetString(4)
        };
    }
}
