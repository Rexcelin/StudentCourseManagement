
using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;
using StudentCourseManagement.Repository;
using StudentCourseManagement.Services;

const string connectionString =
    @"Server=.\SQLEXPRESS;Database=StudentCourseDB;Trusted_Connection=True;TrustServerCertificate=True;";

var studentRepository = new StudentRepository();
var courseRepository = new CourseRepository(connectionString);
var departmentRepository = new DepartmentRepository();
var enrollmentRepository = new EnrollmentRepository();
var auditLogRepository = new AuditLogRepository();

var studentService = new StudentService(studentRepository);
var courseService = new CourseService(courseRepository);
var departmentService = new DepartmentService(departmentRepository);
var enrollmentService = new EnrollmentService(enrollmentRepository);
var auditLogService = new AuditLogService(auditLogRepository);

while (true)
{
    Console.WriteLine("\n==================================");
    Console.WriteLine("   STUDENT COURSE MANAGEMENT");
    Console.WriteLine("==================================");
    Console.WriteLine("1. Add Student");
    Console.WriteLine("2. View All Students");
    Console.WriteLine("3. Search Student By ID");
    Console.WriteLine("4. Add Course");
    Console.WriteLine("5. View All Courses");
    Console.WriteLine("6. Search Course By ID");
    Console.WriteLine("7. Enroll Student in Course");
    Console.WriteLine("8. Exit");
    Console.Write("Enter your choice: ");

    string? choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                {
                    Console.Write("Student name: ");
                    string name = Console.ReadLine() ?? "";

                    Console.Write("Email: ");
                    string email = Console.ReadLine() ?? "";

                    Console.Write("Age: ");
                    if (!int.TryParse(Console.ReadLine(), out int age))
                    {
                        Console.WriteLine("Invalid age.");
                        break;
                    }

                    Console.Write("Department ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int departmentId))
                    {
                        Console.WriteLine("Invalid department ID.");
                        break;
                    }

                    var student = new Student
                    {
                        Name = name,
                        Email = email,
                        Age = age,
                        DepartmentId = departmentId
                    };

                    int studentId = await studentService.AddStudentAsync(student);

                    Console.WriteLine(
                        $"Student added successfully. Student ID: {studentId}");
                    break;
                }

            case "2":
                {
                    var students = await studentService.GetAllStudentsAsync();

                    if (students.Count == 0)
                    {
                        Console.WriteLine("No students found.");
                        break;
                    }

                    Console.WriteLine("\n--- STUDENT LIST ---");

                    foreach (var student in students)
                    {
                        Console.WriteLine(
                            $"{student.StudentId} | {student.Name} | " +
                            $"{student.Email} | Age: {student.Age} | " +
                            $"Department: {student.DepartmentId}");
                    }

                    break;
                }

            case "3":
                {
                    Console.Write("Student ID: ");

                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("Invalid student ID.");
                        break;
                    }

                    var student = await studentService.GetStudentByIdAsync(id);

                    if (student is null)
                    {
                        Console.WriteLine("Student not found.");
                    }
                    else
                    {
                        Console.WriteLine(
                            $"{student.StudentId} | {student.Name} | " +
                            $"{student.Email} | Age: {student.Age} | " +
                            $"Department: {student.DepartmentId}");
                    }

                    break;
                }

            case "4":
                {
                    Console.Write("Course name: ");
                    string courseName = Console.ReadLine() ?? "";

                    Console.Write("Duration in months: ");

                    if (!int.TryParse(Console.ReadLine(), out int duration))
                    {
                        Console.WriteLine("Invalid duration.");
                        break;
                    }

                    Console.Write("Fee: ");

                    if (!decimal.TryParse(Console.ReadLine(), out decimal fee))
                    {
                        Console.WriteLine("Invalid fee.");
                        break;
                    }

                    Console.Write("Department ID: ");

                    if (!int.TryParse(Console.ReadLine(), out int departmentId))
                    {
                        Console.WriteLine("Invalid department ID.");
                        break;
                    }

                    var course = new Course
                    {
                        CourseName = courseName,
                        DurationMonths = duration,
                        Fee = fee,
                        DepartmentId = departmentId
                    };

                    await courseService.AddCourseAsync(course);

                    Console.WriteLine("Course added successfully.");
                    break;
                }

            case "5":
                {
                    var courses = await courseService.GetAllCoursesAsync();

                    if (courses.Count == 0)
                    {
                        Console.WriteLine("No courses found.");
                        break;
                    }

                    Console.WriteLine("\n--- COURSE LIST ---");

                    foreach (var course in courses)
                    {
                        Console.WriteLine(
                            $"{course.CourseId} | {course.CourseName} | " +
                            $"{course.DurationMonths} months | " +
                            $"Fee: {course.Fee:N2} | " +
                            $"Department: {course.DepartmentId}");
                    }

                    break;
                }

            case "6":
                {
                    Console.Write("Course ID: ");

                    if (!int.TryParse(Console.ReadLine(), out int courseId))
                    {
                        Console.WriteLine("Invalid course ID.");
                        break;
                    }

                    var course = await courseService.GetCourseByIdAsync(courseId);

                    if (course is null)
                    {
                        Console.WriteLine("Course not found.");
                    }
                    else
                    {
                        Console.WriteLine(
                            $"{course.CourseId} | {course.CourseName} | " +
                            $"{course.DurationMonths} months | " +
                            $"Fee: {course.Fee:N2} | " +
                            $"Department: {course.DepartmentId}");
                    }

                    break;
                }

            case "7":
                {
                    Console.Write("Enter Student ID: ");

                    if (!int.TryParse(Console.ReadLine(), out int studentId))
                    {
                        Console.WriteLine("Invalid student ID.");
                        break;
                    }

                    Console.Write("Enter Course ID: ");

                    if (!int.TryParse(Console.ReadLine(), out int courseId))
                    {
                        Console.WriteLine("Invalid course ID.");
                        break;
                    }

                    // Uses the enrollment service, not StudentRepository.
                    await enrollmentService.EnrollStudentAsync(studentId, courseId);

                    Console.WriteLine("Student enrolled successfully.");
                    break;
                }

            case "8":
                Console.WriteLine("Application closed.");
                return;

            default:
                Console.WriteLine("Invalid choice. Please select 1-8.");
                break;
        }
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Validation error: {ex.Message}");
    }
    catch (SqlException ex)
    {
        Console.WriteLine($"Database error: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
