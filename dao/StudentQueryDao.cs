using gurukul.data;
using gurukul.interfaces;
using gurukul.model;
using Oracle.ManagedDataAccess.Client;

namespace gurukul.dao;

public class StudentQueryDao : IModelDao<StudentQuery>
{
    private List<StudentQuery> GetStudentQueries(string queryString)
    {
        List<StudentQuery> studentQueries = new List<StudentQuery>();
        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand(queryString, connection);
        command.Connection.Open();

        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            StudentQuery studentQuery = new StudentQuery();
            studentQuery.StudentQueryId = reader.GetString(0);
            studentQuery.EnrolmentId = reader.GetString(1);
            studentQuery.question = reader.GetString(2);
            studentQueries.Add(studentQuery);
        }

        reader.Dispose();
        return studentQueries;
    }

    public List<StudentQuery> GetList()
    {
        List<StudentQuery> studentQueries = new List<StudentQuery>();

        try
        {
            studentQueries = GetStudentQueries("SELECT * FROM STUDENTQUERY");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return studentQueries;
    }

    public List<StudentQuery> GetList(string col, SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<StudentQuery> studentQueries = new List<StudentQuery>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            studentQueries = GetStudentQueries($"SELECT * FROM STUDENTQUERY ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return studentQueries;
    }

    public List<StudentQuery> GetList(string searchQuery, string searchCol)
    {
        List<StudentQuery> studentQueries = new List<StudentQuery>();

        try
        {
            studentQueries = GetStudentQueries($"SELECT * FROM STUDENTQUERY WHERE {searchCol} = '{searchQuery}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return studentQueries;
    }

    public List<StudentQuery> GetList(string searchQuery, string searchCol, string col,
        SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<StudentQuery> studentQueries = new List<StudentQuery>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            studentQueries = GetStudentQueries(
                $"SELECT * FROM STUDENTQUERY WHERE {searchCol} = '{searchQuery}' ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return studentQueries;
    }

    public StudentQuery GetById(string id)
    {
        StudentQuery studentQuery = new StudentQuery();
        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand($"SELECT * FROM STUDENTQUERY WHERE STUDENT_QUERY_ID = '{id}'",
            connection);
        command.Connection.Open();

        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            studentQuery.StudentQueryId = reader.GetString(0);
            studentQuery.EnrolmentId = reader.GetString(1);
            studentQuery.question = reader.GetString(2);
        }

        reader.Dispose();
        return studentQuery;
    }

    public void Create(StudentQuery data)
    {
        try
        {
            DbManager.Execute(
                $"INSERT INTO STUDENTQUERY (STUDENT_QUERY_ID, ENROLMENT_ID, QUESTION) VALUES ('{data.StudentQueryId}', '{data.EnrolmentId}', '{data.question}')");
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
            DbManager.Execute($"DELETE FROM STUDENTQUERY WHERE STUDENT_QUERY_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void UpdateById(string id, StudentQuery data)
    {
        try
        {
            DbManager.Execute(
                $"UPDATE STUDENTQUERY SET ENROLMENT_ID = '{data.EnrolmentId}', QUESTION = '{data.question}' WHERE STUDENT_QUERY_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}