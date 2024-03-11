using gurukul.interfaces;
using gurukul.Models;
using gurukul.Utils;
using Oracle.ManagedDataAccess.Client;

namespace gurukul.Dao;

public class FeedbackDao : IModelDao<Feedback>
{
    private List<Feedback> GetFeedbackList(string queryString)
    {
        List<Feedback> feedbacks = new List<Feedback>();
        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand(queryString, connection);
        command.Connection.Open();

        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Feedback feedback = new Feedback();
            feedback.FeedbackId = reader.GetString(0);
            feedback.CourseInstructorId = reader.GetString(1);
            feedback.StudentQueryId = reader.GetString(2);
            feedback.FeedbackText = reader.GetString(3);
            feedbacks.Add(feedback);
        }

        reader.Dispose();
        return feedbacks;
    }

    public List<Feedback> GetList()
    {
        List<Feedback> feedbacks = new List<Feedback>();

        try
        {
            feedbacks = GetFeedbackList("SELECT * FROM FEEDBACK");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return feedbacks;
    }

    public List<Feedback> GetList(string col, SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<Feedback> feedbacks = new List<Feedback>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            feedbacks = GetFeedbackList($"SELECT * FROM FEEDBACK ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return feedbacks;
    }

    public List<Feedback> GetList(string searchQuery, string searchCol)
    {
        List<Feedback> feedbacks = new List<Feedback>();

        try
        {
            feedbacks = GetFeedbackList($"SELECT * FROM FEEDBACK WHERE {searchCol} = '{searchQuery}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return feedbacks;
    }

    public List<Feedback> GetList(string searchQuery, string searchCol, string col,
        SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<Feedback> feedbacks = new List<Feedback>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            feedbacks = GetFeedbackList(
                $"SELECT * FROM FEEDBACK WHERE {searchCol} = '{searchQuery}' ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return feedbacks;
    }

    public Feedback GetById(string id)
    {
        Feedback feedback = new Feedback();
        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand($"SELECT * FROM FEEDBACK WHERE FEEDBACK_ID = '{id}'", connection);
        command.Connection.Open();

        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            feedback.FeedbackId = reader.GetString(0);
            feedback.CourseInstructorId = reader.GetString(1);
            feedback.StudentQueryId = reader.GetString(2);
            feedback.FeedbackText = reader.GetString(3);
        }

        reader.Dispose();
        return feedback;
    }

    public void Create(Feedback data)
    {
        try
        {
            DbManager.Execute(
                $"INSERT INTO FEEDBACK (FEEDBACK_ID, FEEDBACK, COURSE_INSTRUCTOR_ID, STUDENT_QUERY_ID) VALUES ('{data.FeedbackId}', '{data.FeedbackText}', '{data.CourseInstructorId}', '{data.StudentQueryId}')");
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
            DbManager.Execute($"DELETE FROM FEEDBACK WHERE FEEDBACK_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void UpdateById(string id, Feedback data)
    {
        try
        {
            DbManager.Execute(
                $"UPDATE FEEDBACK SET FEEDBACK = '{data.FeedbackText}', COURSE_INSTRUCTOR_ID = '{data.CourseInstructorId}', STUDENT_QUERY_ID = '{data.StudentQueryId}' WHERE FEEDBACK_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}