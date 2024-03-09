using gurukul.data;
using gurukul.interfaces;
using gurukul.model;
using Oracle.ManagedDataAccess.Client;

namespace gurukul.dao;

public class LessonDao : IModelDao<Lesson>
{
    private List<Lesson> GetLessonList(string queryString)
    {
        List<Lesson> lessons = new List<Lesson>();
        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand(queryString, connection);
        command.Connection.Open();
        
        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Lesson lesson = new Lesson();
            lesson.LessonId = reader.GetString(0);
            lesson.LessonNumber = reader.GetInt16(1);
            lesson.CourseId = reader.GetString(2);
            lesson.LessonContent = reader.GetString(3);
            lessons.Add(lesson);
        }
        
        reader.Dispose();
        return lessons;
    }
    
    public List<Lesson> GetList()
    {
        List<Lesson> lessons = new List<Lesson>();

        try
        {
            lessons = GetLessonList("SELECT * FROM LESSON");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return lessons;
    }

    public List<Lesson> GetList(string col, SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<Lesson> lessons = new List<Lesson>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            lessons = GetLessonList($"SELECT * FROM LESSON ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return lessons;
    }

    public List<Lesson> GetList(string searchQuery, string searchCol)
    {
        List<Lesson> lessons = new List<Lesson>();

        try
        {
            lessons = GetLessonList($"SELECT * FROM LESSON WHERE {searchCol} = '{searchQuery}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return lessons;
    }

    public List<Lesson> GetList(string searchQuery, string searchCol, string col, SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<Lesson> lessons = new List<Lesson>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            lessons = GetLessonList($"SELECT * FROM LESSON WHERE {searchCol} = '{searchQuery}' ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return lessons;
    }

    public Lesson GetById(string id)
    {
        Lesson lesson = new Lesson();
        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand($"SELECT * FROM LESSON WHERE LESSON_ID = '{id}'", connection);
        command.Connection.Open();
        
        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            lesson.LessonId = reader.GetString(0);
            lesson.LessonNumber = reader.GetInt16(1);
            lesson.CourseId = reader.GetString(2);
            lesson.LessonContent = reader.GetString(3);
        }
        
        reader.Dispose();
        return lesson;
    }

    public void Create(Lesson data)
    {
        try
        {
            DbManager.Execute($"INSERT INTO LESSON (LESSON_ID, LESSON_NUMBER, COURSE_ID, LESSON_CONTENT) VALUES ('{data.LessonId}', {data.LessonNumber}, '{data.CourseId}', '{data.LessonContent}')");
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
            DbManager.Execute($"DELETE FROM LESSON WHERE LESSON_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void UpdateById(string id, Lesson data)
    {
        try
        {
            DbManager.Execute($"UPDATE LESSON SET LESSON_NUMBER = {data.LessonNumber}, COURSE_ID = '{data.CourseId}', LESSON_CONTENT = '{data.LessonContent}' WHERE LESSON_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}