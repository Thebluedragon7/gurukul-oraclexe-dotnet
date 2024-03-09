using gurukul.data;
using gurukul.interfaces;
using gurukul.model;
using Oracle.ManagedDataAccess.Client;

namespace gurukul.dao;

public class InstructorDao : IModelDao<Instructor>
{
    private List<Instructor> GetInstructorList(string queryString)
    {
        List<Instructor> instructors = new List<Instructor>();
        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand(queryString, connection);
        command.Connection.Open();
        
        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Instructor instructor = new Instructor();
            instructor.InstructorId = reader.GetString(0);
            instructor.InstructorName = reader.GetString(1);
            instructor.InstructorEmail = reader.GetString(2);
            instructors.Add(instructor);
        }
        
        reader.Dispose();
        return instructors;
    }
    
    public List<Instructor> GetList()
    {
        List<Instructor> instructors = new List<Instructor>();
        
        try
        {
            instructors = GetInstructorList("SELECT * FROM INSTRUCTOR");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        return instructors;
    }

    public List<Instructor> GetList(string col, SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<Instructor> instructors = new List<Instructor>();
        
        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";
        
        try
        {
            instructors = GetInstructorList($"SELECT * FROM INSTRUCTOR ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        return instructors;
    }

    public List<Instructor> GetList(string searchQuery, string searchCol)
    {
        List<Instructor> instructors = new List<Instructor>();
        
        try
        {
            instructors = GetInstructorList($"SELECT * FROM INSTRUCTOR WHERE {searchCol} = '{searchQuery}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        return instructors;
    }

    public List<Instructor> GetList(string searchQuery, string searchCol, string col, SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<Instructor> instructors = new List<Instructor>();
        
        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";
        
        try
        {
            instructors = GetInstructorList($"SELECT * FROM INSTRUCTOR WHERE {searchCol} = '{searchQuery}' ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        return instructors;
    }

    public Instructor GetById(string id)
    {
        Instructor instructor = new Instructor();
        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand($"SELECT * FROM INSTRUCTOR WHERE INSTRUCTOR_ID = '{id}'", connection);
        command.Connection.Open();
        
        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            instructor.InstructorId = reader.GetString(0);
            instructor.InstructorName = reader.GetString(1);
            instructor.InstructorEmail = reader.GetString(2);
        }
        
        reader.Dispose();
        return instructor;
    }

    public void Create(Instructor data)
    {
        try
        {
            DbManager.Execute(
                $"INSERT INTO INSTRUCTOR (INSTRUCTOR_ID, INSTRUCTOR_NAME, INSTRUCTOR_EMAIL) VALUES ('{data.InstructorId}', '{data.InstructorName}', '{data.InstructorEmail}')");
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
            DbManager.Execute($"DELETE FROM INSTRUCTOR WHERE INSTRUCTOR_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void UpdateById(string id, Instructor data)
    {
        try
        {
            DbManager.Execute(
                $"UPDATE INSTRUCTOR SET INSTRUCTOR_NAME = '{data.InstructorName}', INSTRUCTOR_EMAIL = '{data.InstructorEmail}' WHERE INSTRUCTOR_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}