using gurukul.data;
using gurukul.interfaces;
using gurukul.model;
using Oracle.ManagedDataAccess.Client;

namespace gurukul.dao;

public class CountryDao : IModelDao<Country>
{
    private List<Country> GetCountryList(string queryString)
    {
        List<Country> countries = new List<Country>();
        using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
        OracleCommand command = new OracleCommand(queryString, connection);
        command.Connection.Open();

        OracleDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Country country = new Country();
            country.CountryId = reader.GetString(0);
            country.CountryName = reader.GetString(1);
            country.CountryZipCode = reader.GetInt32(2);
            countries.Add(country);
        }

        reader.Dispose();
        return countries;
    }

    public List<Country> GetList()
    {
        List<Country> countries = new List<Country>();

        try
        {
            countries = GetCountryList("SELECT * FROM COUNTRY");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return countries;
    }

    public List<Country> GetList(string col, SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<Country> countries = new List<Country>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            countries = GetCountryList($"SELECT * FROM COUNTRY ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return countries;
    }

    public List<Country> GetList(string searchQuery, string searchCol)
    {
        List<Country> countries = new List<Country>();

        try
        {
            countries = GetCountryList($"SELECT * FROM COUNTRY WHERE {searchCol} LIKE '%{searchQuery}%'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return countries;
    }

    public List<Country> GetList(string searchQuery, string searchCol, string col,
        SortByEnum sortBy = SortByEnum.ASCENDING)
    {
        List<Country> countries = new List<Country>();

        string order = sortBy == SortByEnum.ASCENDING ? "ASC" : "DESC";

        try
        {
            countries = GetCountryList(
                $"SELECT * FROM COUNTRY WHERE {searchCol} LIKE '%{searchQuery}%' ORDER BY {col} {order}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return countries;
    }

    public Country GetById(string id)
    {
        Country country = new Country();

        try
        {
            using OracleConnection connection = new OracleConnection(DbManager.ConnectionString);
            OracleCommand command = new OracleCommand($"SELECT * FROM COUNTRY WHERE COUNTRY_ID = '{id}'", connection);
            command.Connection.Open();

            OracleDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                country.CountryId = reader.GetString(0);
                country.CountryName = reader.GetString(1);
                country.CountryZipCode = reader.GetInt32(2);
            }

            reader.Dispose();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return country;
    }

    public void Create(Country data)
    {
        try
        {
            DbManager.Execute(
                $"INSERT INTO COUNTRY (COUNTRY_ID, COUNTRY_NAME, COUNTRY_ZIPCODE) VALUES ('{Guid.NewGuid()}', '{data.CountryName}', '{data.CountryZipCode}')");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void DeleteById(string id)
    {
        try
        {
            DbManager.Execute($"DELETE FROM COUNTRY WHERE COUNTRY_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void UpdateById(string id, Country data)
    {
        try
        {
            DbManager.Execute(
                $"UPDATE COUNTRY SET COUNTRY_NAME = '{data.CountryName}', COUNTRY_ZIPCODE = '{data.CountryZipCode}' WHERE COUNTRY_ID = '{id}'");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}