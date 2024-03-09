using gurukul.data;
using gurukul.interfaces;
using gurukul.model;
using Oracle.ManagedDataAccess.Client;

namespace gurukul.dao;

public class CourseDao : IModelDao<Course>
{
    private List<Course> GetCourseList(string queryString)
    {
        List<Course> courses = new List<Course>();

        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand(queryString, connection);
        command.Connection.Open();

        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Course course = new Course();
            course.CourseId = reader.GetString(0);
            course.CourseTitle = reader.GetString(1);
            course.CourseDescription = reader.GetString(2);
            courses.Add(course);
        }

        reader.Dispose();
        return courses;
    }

    public List<Course> GetList()
    {
        List<Course> courses = new List<Course>();

        try
        {
            courses = GetCourseList("SELECT * FROM COURSE");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return courses;
    }

    public List<Course> GetList(string col, SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<Course> courses = new List<Course>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            courses = GetCourseList($"SELECT * FROM COURSE ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return courses;
    }

    public List<Course> GetList(string searchQuery, string searchCol)
    {
        List<Course> courses = new List<Course>();

        try
        {
            courses = GetCourseList($"SELECT * FROM COURSE WHERE {searchCol} LIKE '%{searchQuery}%'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return courses;
    }

    public List<Course> GetList(string searchQuery, string searchCol, string col,
        SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<Course> courses = new List<Course>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            courses = GetCourseList(
                $"SELECT * FROM COURSE WHERE {searchCol} LIKE '%{searchQuery}%' ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return courses;
    }

    public Course GetById(string id)
    {
        Course course = new Course();

        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand($"SELECT * FROM COURSE WHERE COURSE_ID = '{id}'", connection);
        command.Connection.Open();

        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            course.CourseId = reader.GetString(0);
            course.CourseTitle = reader.GetString(1);
            course.CourseDescription = reader.GetString(2);
        }

        reader.Dispose();
        return course;
    }

    public void Create(Course data)
    {
        try
        {
            DbManager.Execute(
                $"INSERT INTO COURSE (COURSE_ID, COURSE_TITLE, COURSE_DESCRIPTION) VALUES ('{data.CourseId}', '{data.CourseTitle}', '{data.CourseDescription}')");
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
            DbManager.Execute($"DELETE FROM COURSE WHERE COURSE_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void UpdateById(string id, Course data)
    {
        try
        {
            DbManager.Execute(
                $"UPDATE COURSE SET COURSE_TITLE = '{data.CourseTitle}', COURSE_DESCRIPTION = '{data.CourseDescription}' WHERE COURSE_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}