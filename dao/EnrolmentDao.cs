using gurukul.data;
using gurukul.interfaces;
using gurukul.model;
using Oracle.ManagedDataAccess.Client;

namespace gurukul.dao;

public class EnrolmentDao : IModelDao<Enrolment>
{
    private List<Enrolment> GetEnrolmentList(string queryString)
    {
        List<Enrolment> enrolments = new List<Enrolment>();
        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand(queryString, connection);
        command.Connection.Open();

        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Enrolment enrolment = new Enrolment();
            enrolment.EnrolmentId = reader.GetString(0);
            enrolment.StudentId = reader.GetString(1);
            enrolment.CourseId = reader.GetString(2);
            enrolment.EnrolmentDate = reader.GetString(3);
            enrolments.Add(enrolment);
        }

        reader.Dispose();
        return enrolments;
    }

    public List<Enrolment> GetList()
    {
        List<Enrolment> enrolments = new List<Enrolment>();

        try
        {
            enrolments = GetEnrolmentList("SELECT * FROM ENROLMENT");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return enrolments;
    }

    public List<Enrolment> GetList(string col, SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<Enrolment> enrolments = new List<Enrolment>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            enrolments = GetEnrolmentList($"SELECT * FROM ENROLMENT ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return enrolments;
    }

    public List<Enrolment> GetList(string searchQuery, string searchCol)
    {
        List<Enrolment> enrolments = new List<Enrolment>();

        try
        {
            enrolments = GetEnrolmentList($"SELECT * FROM ENROLMENT WHERE {searchCol} LIKE '%{searchQuery}%'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return enrolments;
    }

    public List<Enrolment> GetList(string searchQuery, string searchCol, string col,
        SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<Enrolment> enrolments = new List<Enrolment>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            enrolments = GetEnrolmentList(
                $"SELECT * FROM ENROLMENT WHERE {searchCol} LIKE '%{searchQuery}%' ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return enrolments;
    }

    public Enrolment GetById(string id)
    {
        Enrolment enrolment = new Enrolment();
        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand($"SELECT * FROM ENROLMENT WHERE ENROLMENT_ID = '{id}'", connection);
        command.Connection.Open();

        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            enrolment.EnrolmentId = reader.GetString(0);
            enrolment.StudentId = reader.GetString(1);
            enrolment.CourseId = reader.GetString(2);
            enrolment.EnrolmentDate = reader.GetString(3);
        }

        reader.Dispose();
        return enrolment;
    }

    public void Create(Enrolment data)
    {
        try
        {
            DbManager.Execute($"INSERT INTO ENROLMENT (ENROLMENT_ID, STUDENT_ID, COURSE_ID, ENROLMENT_DATE) " +
                              $"VALUES ('{data.EnrolmentId}', '{data.StudentId}', '{data.CourseId}', '{data.EnrolmentDate}')");
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
            DbManager.Execute($"DELETE FROM ENROLMENT WHERE ENROLMENT_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void UpdateById(string id, Enrolment data)
    {
        try
        {
            DbManager.Execute($"UPDATE ENROLMENT SET STUDENT_ID = '{data.StudentId}', COURSE_ID = '{data.CourseId}', " +
                              $"ENROLMENT_DATE = '{data.EnrolmentDate}' WHERE ENROLMENT_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}