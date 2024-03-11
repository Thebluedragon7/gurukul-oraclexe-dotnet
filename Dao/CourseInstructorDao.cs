using gurukul.interfaces;
using gurukul.Models;
using gurukul.Utils;
using Oracle.ManagedDataAccess.Client;

namespace gurukul.Dao;

public class CourseInstructorDao : IModelDao<CourseInstructor>
{
    private List<CourseInstructor> GetCourseInstructorList(string queryString)
    {
        List<CourseInstructor> courseInstructors = new List<CourseInstructor>();

        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand(queryString, connection);
        command.Connection.Open();

        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            CourseInstructor courseInstructor = new CourseInstructor();
            courseInstructor.CourseInstructorId = reader.GetString(0);
            courseInstructor.InstructorId = reader.GetString(1);
            courseInstructor.CourseId = reader.GetString(2);
            courseInstructors.Add(courseInstructor);
        }

        reader.Dispose();
        return courseInstructors;
    }

    public List<CourseInstructor> GetList()
    {
        List<CourseInstructor> courseInstructors = new List<CourseInstructor>();

        try
        {
            courseInstructors = GetCourseInstructorList("SELECT * FROM COURSEINSTRUCTOR");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return courseInstructors;
    }

    public List<CourseInstructor> GetList(string col, SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<CourseInstructor> courseInstructors = new List<CourseInstructor>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            courseInstructors = GetCourseInstructorList($"SELECT * FROM COURSEINSTRUCTOR ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return courseInstructors;
    }

    public List<CourseInstructor> GetList(string searchQuery, string searchCol)
    {
        List<CourseInstructor> courseInstructors = new List<CourseInstructor>();

        try
        {
            courseInstructors =
                GetCourseInstructorList($"SELECT * FROM COURSEINSTRUCTOR WHERE {searchCol} = '{searchQuery}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return courseInstructors;
    }

    public List<CourseInstructor> GetList(string searchQuery, string searchCol, string col,
        SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<CourseInstructor> courseInstructors = new List<CourseInstructor>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            courseInstructors =
                GetCourseInstructorList(
                    $"SELECT * FROM COURSEINSTRUCTOR WHERE {searchCol} = '{searchQuery}' ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return courseInstructors;
    }

    public CourseInstructor GetById(string id)
    {
        CourseInstructor instructor = new CourseInstructor();

        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand($"SELECT * FROM COURSEINSTRUCTOR WHERE COURSE_INSTRUCTOR_ID = '{id}'",
            connection);
        command.Connection.Open();

        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            instructor.CourseInstructorId = reader.GetString(0);
            instructor.InstructorId = reader.GetString(1);
            instructor.CourseId = reader.GetString(2);
        }

        reader.Dispose();
        return instructor;
    }

    public void Create(CourseInstructor data)
    {
        try
        {
            DbManager.Execute(
                $"INSERT INTO COURSEINSTRUCTOR (COURSE_INSTRUCTOR_ID, INSTRUCTOR_ID, COURSE_ID) VALUES ('{data.CourseInstructorId}', '{data.InstructorId}', '{data.CourseId}')");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void DeleteById(string id)
    {
        try
        {
            DbManager.Execute($"DELETE FROM COURSEINSTRUCTOR WHERE COURSE_INSTRUCTOR_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void UpdateById(string id, CourseInstructor data)
    {
        try
        {
            DbManager.Execute(
                $"UPDATE COURSEINSTRUCTOR SET INSTRUCTOR_ID = '{data.InstructorId}', COURSE_ID = '{data.CourseId}' WHERE COURSE_INSTRUCTOR_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}