using gurukul.data;
using gurukul.interfaces;
using gurukul.model;
using Oracle.ManagedDataAccess.Client;

namespace gurukul.dao;

public class StudentDao : IModelDao<Student>
{
    private List<Student> GetStudentList(string queryString)
    {
        List<Student> students = new List<Student>();
        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand(queryString, connection);
        command.Connection.Open();

        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Student student = new Student();
            student.StudentId = reader.GetString(0);
            student.StudentName = reader.GetString(1);
            student.CountryId = reader.GetString(2);
            student.StudentEmail = reader.GetString(3);
            student.StudentContact = reader.GetString(4);
            student.StudentDob = reader.GetDateTime(5);
            students.Add(student);
        }

        reader.Dispose();
        return students;
    }

    public List<Student> GetList()
    {
        List<Student> students = new List<Student>();

        try
        {
            students = GetStudentList("SELECT * FROM STUDENT");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return students;
    }

    public List<Student> GetList(string col, SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<Student> students = new List<Student>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            students = GetStudentList($"SELECT * FROM STUDENT ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return students;
    }

    public List<Student> GetList(string searchQuery, string searchCol)
    {
        List<Student> students = new List<Student>();

        try
        {
            students = GetStudentList($"SELECT * FROM STUDENT WHERE {searchCol} LIKE '%{searchQuery}%'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return students;
    }

    public List<Student> GetList(string searchQuery, string searchCol, string col,
        SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<Student> students = new List<Student>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            students = GetStudentList(
                $"SELECT * FROM STUDENT WHERE {searchCol} LIKE '%{searchQuery}%' ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return students;
    }

    public Student GetById(string id)
    {
        Student student = new Student();
        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand($"SELECT * FROM STUDENT WHERE STUDENT_ID = '{id}'", connection);
        command.Connection.Open();

        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            student.StudentId = reader.GetString(0);
            student.StudentName = reader.GetString(1);
            student.CountryId = reader.GetString(2);
            student.StudentEmail = reader.GetString(3);
            student.StudentContact = reader.GetString(4);
            student.StudentDob = reader.GetDateTime(5);
        }

        reader.Dispose();
        return student;
    }

    public void Create(Student data)
    {
        try
        {
            DbManager.Execute(
                $"INSERT INTO STUDENT (STUDENT_ID, STUDENT_NAME, STUDENT_EMAIL, COUNTRY_ID, STUDENT_CONTACT, STUDENT_DOB) VALUES ('{data.StudentId}', '{data.StudentName}', '{data.StudentEmail}', '{data.CountryId}', '{data.StudentContact}', '{data.StudentDob}')");
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
            DbManager.Execute($"DELETE FROM STUDENT WHERE STUDENT_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void UpdateById(string id, Student data)
    {
        try
        {
            DbManager.Execute(
                $"UPDATE STUDENT SET STUDENT_NAME = '{data.StudentName}', STUDENT_EMAIL = '{data.StudentEmail}', COUNTRY_ID = '{data.CountryId}', STUDENT_CONTACT = '{data.StudentContact}', STUDENT_DOB = '{data.StudentDob}' WHERE STUDENT_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}