using gurukul.interfaces;
using gurukul.Models;
using gurukul.Utils;
using Oracle.ManagedDataAccess.Client;

namespace gurukul.Dao;

public class LessonProgressDao : IModelDao<LessonProgress>
{
    private List<LessonProgress> GetLessonProgressList(string queryString)
    {
        List<LessonProgress> lessonProgresses = new List<LessonProgress>();
        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand(queryString, connection);
        command.Connection.Open();

        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            LessonProgress lessonProgress = new LessonProgress();
            lessonProgress.LessonProgressId = reader.GetString(0);
            lessonProgress.LessonId = reader.GetString(1);
            lessonProgress.EnrolmentId = reader.GetString(2);
            switch (reader.GetString(3))
            {
                case "NOT_STARTED":
                    lessonProgress.LessonStatus = LessonStatusEnum.NotStarted;
                    break;
                case "ON_PROGRESS":
                    lessonProgress.LessonStatus = LessonStatusEnum.InProgress;
                    break;
                case "COMPLETED":
                    lessonProgress.LessonStatus = LessonStatusEnum.Completed;
                    break;
                default:
                    lessonProgress.LessonStatus = LessonStatusEnum.NotStarted;
                    break;
            }

            lessonProgresses.Add(lessonProgress);
        }

        reader.Dispose();
        return lessonProgresses;
    }

    public List<LessonProgress> GetList()
    {
        List<LessonProgress> lessonProgresses = new List<LessonProgress>();

        try
        {
            lessonProgresses = GetLessonProgressList("SELECT * FROM LESSONPROGRESS");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return lessonProgresses;
    }

    public List<LessonProgress> GetList(string col, SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<LessonProgress> lessonProgresses = new List<LessonProgress>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            lessonProgresses = GetLessonProgressList($"SELECT * FROM LESSONPROGRESS ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return lessonProgresses;
    }

    public List<LessonProgress> GetList(string searchQuery, string searchCol)
    {
        List<LessonProgress> lessonProgresses = new List<LessonProgress>();

        try
        {
            lessonProgresses =
                GetLessonProgressList($"SELECT * FROM LESSONPROGRESS WHERE {searchCol} LIKE '%{searchQuery}%'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return lessonProgresses;
    }

    public List<LessonProgress> GetList(string searchQuery, string searchCol, string col,
        SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<LessonProgress> lessonProgresses = new List<LessonProgress>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            lessonProgresses =
                GetLessonProgressList(
                    $"SELECT * FROM LESSONPROGRESS WHERE {searchCol} LIKE '%{searchQuery}%' ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return lessonProgresses;
    }

    public LessonProgress GetById(string id)
    {
        LessonProgress lessonProgress = new LessonProgress();
        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command =
            new OracleCommand($"SELECT * FROM LESSONPROGRESS WHERE LESSON_PROGRESS_ID = '{id}'", connection);
        command.Connection.Open();

        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            lessonProgress.LessonProgressId = reader.GetString(0);
            lessonProgress.LessonId = reader.GetString(1);
            lessonProgress.EnrolmentId = reader.GetString(2);
            switch (reader.GetString(3))
            {
                case "NOT_STARTED":
                    lessonProgress.LessonStatus = LessonStatusEnum.NotStarted;
                    break;
                case "ON_PROGRESS":
                    lessonProgress.LessonStatus = LessonStatusEnum.InProgress;
                    break;
                case "COMPLETED":
                    lessonProgress.LessonStatus = LessonStatusEnum.Completed;
                    break;
                default:
                    lessonProgress.LessonStatus = LessonStatusEnum.NotStarted;
                    break;
            }
        }

        reader.Dispose();
        return lessonProgress;
    }

    public void Create(LessonProgress data)
    {
        string lessonStatus;

        if (data.LessonStatus == LessonStatusEnum.NotStarted)
        {
            lessonStatus = "NOT_STARTED";
        }
        else if (data.LessonStatus == LessonStatusEnum.InProgress)
        {
            lessonStatus = "ON_PROGRESS";
        }
        else
        {
            lessonStatus = "COMPLETED";
        }

        try
        {
            DbManager.Execute(
                $"INSERT INTO LESSONPROGRESS (LESSON_PROGRESS_ID, LESSON_ID, ENROLMENT_ID, LESSON_STATUS) VALUES ('{data.LessonProgressId}', '{data.LessonId}', '{data.EnrolmentId}', '{lessonStatus}')");
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
            DbManager.Execute($"DELETE FROM LESSONPROGRESS WHERE LESSON_PROGRESS_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void UpdateById(string id, LessonProgress data)
    {
        string lessonStatus;

        if (data.LessonStatus == LessonStatusEnum.NotStarted)
        {
            lessonStatus = "NOT_STARTED";
        }
        else if (data.LessonStatus == LessonStatusEnum.InProgress)
        {
            lessonStatus = "ON_PROGRESS";
        }
        else
        {
            lessonStatus = "COMPLETED";
        }


        try
        {
            DbManager.Execute(
                $"UPDATE LESSONPROGRESS SET LESSON_ID = '{data.LessonId}', ENROLMENT_ID = '{data.EnrolmentId}', LESSON_STATUS = '{lessonStatus}' WHERE LESSON_PROGRESS_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}