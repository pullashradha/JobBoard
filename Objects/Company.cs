using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Linq;

namespace JobBoard
{
  public class Company
  {
    private string _name;
    private int _id;

    public Company(string name, int id=0)
    {
      _name = name;
      _id = id;
    }
    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public override bool Equals(System.Object otherCompany)
    {
      if (!(otherCompany is Company))
      {
        return false;
      }
      else
      {
        Company newCompany = (Company) otherCompany;
        bool idEquality = this.GetId() == newCompany.GetId();
        bool nameEquality = this.GetName() == newCompany.GetName();
        return (idEquality && nameEquality);
      }
    }
    public static List<Company> GetAll()
    {
      List<Company> allCompanys = new List<Company>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM companies;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int CompanyId = rdr.GetInt32(0);
        string CompanyName = rdr.GetString(1);
        Company newCompany = new Company(CompanyName, CompanyId);
        allCompanys.Add(newCompany);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allCompanys;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO companies (name) OUTPUT INSERTED.id VALUES (@CompanyName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@CompanyName";
      nameParameter.Value = this.GetName();

      cmd.Parameters.Add(nameParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public static Company Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand ("SELECT * FROM companies WHERE id = @CompanyId;", conn);

      SqlParameter CompanyIdParameter = new SqlParameter();
      CompanyIdParameter.ParameterName = "@CompanyId";
      CompanyIdParameter.Value = id;
      cmd.Parameters.Add(CompanyIdParameter);

      rdr = cmd.ExecuteReader();

      int foundCompanyId = 0;
      string foundCompanyName = null;

      while(rdr.Read())
      {
        foundCompanyId = rdr.GetInt32(0);
        foundCompanyName = rdr.GetString(1);
      }
      Company foundCompany = new Company(foundCompanyName, foundCompanyId);

      if(rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCompany;
    }
    public void Update(string newName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      this.SetName(newName);

      SqlCommand cmd = new SqlCommand("UPDATE companies SET name = @NewName WHERE id = @CompanyId;", conn);

      SqlParameter newNameParameter = new SqlParameter();
      newNameParameter.ParameterName = "@NewName";
      newNameParameter.Value = newName;
      cmd.Parameters.Add(newNameParameter);

      SqlParameter CompanyIdParameter = new SqlParameter();
      CompanyIdParameter.ParameterName = "@CompanyId";
      CompanyIdParameter.Value = this.GetId();
      cmd.Parameters.Add(CompanyIdParameter);


      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public List<Job> GetJobs ()
    {
      List<Job> foundJobs = new List<Job> {};
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand ("SELECT * FROM jobs WHERE company_id = @CompanyId;", conn);

      SqlParameter companyIdParameter = new SqlParameter();
      companyIdParameter.ParameterName = "@CompanyId";
      companyIdParameter.Value = this.GetId();

      cmd.Parameters.Add(companyIdParameter);

      rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        int foundJobId = rdr.GetInt32(0);
        string foundJobName = rdr.GetString(1);
        string foundJobDescription = rdr.GetString(2);
        int foundJobSalary = rdr.GetInt32(3);
        int foundCompanyId = rdr.GetInt32(4);
        int foundCategoryId = rdr.GetInt32(5);

        Job foundJob = new Job (foundJobName, foundJobDescription, foundJobSalary, foundCompanyId, foundCategoryId, foundJobId);
        foundJobs.Add(foundJob);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundJobs;
    }
    public List<Job> FindJobs (string searchKeyword)
    {
      List<Job> foundJobs = new List<Job> {};
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand ("SELECT jobs.* FROM keywords JOIN jobs_keywords ON (keywords.id = jobs_keywords.keyword_id) JOIN jobs ON (jobs_keywords.job_id = jobs.id) WHERE jobs.company_id = @CompanyId AND keywords.word = @Keyword;", conn);

      SqlParameter companyIdParameter = new SqlParameter();
      companyIdParameter.ParameterName = "@CompanyId";
      companyIdParameter.Value = this.GetId();

      SqlParameter keywordParameter = new SqlParameter();
      keywordParameter.ParameterName = "@Keyword";
      keywordParameter.Value = searchKeyword.ToLower();

      cmd.Parameters.Add(keywordParameter);
      cmd.Parameters.Add(companyIdParameter);

      rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        int foundJobId = rdr.GetInt32(0);
        string foundJobName = rdr.GetString(1);
        string foundJobDescription = rdr.GetString(2);
        int foundJobSalary = rdr.GetInt32(3);
        int foundCompanyId = rdr.GetInt32(4);
        int foundCategoryId = rdr.GetInt32(5);

        Job foundJob = new Job (foundJobName, foundJobDescription, foundJobSalary, foundCompanyId, foundCategoryId, foundJobId);
        foundJobs.Add(foundJob);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundJobs;
    }
    public Dictionary<string, int> GetPopularWords(int topNumber)
    {
      Dictionary<int, int> popularWords = new Dictionary<int, int>{};

      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand("SELECT jobs_keywords.* FROM companies JOIN jobs ON (companies.id = jobs.company_id) JOIN jobs_keywords ON (jobs.id = jobs_keywords.job_id) WHERE companies.id = @CompanyId;", conn);

      SqlParameter companyIdParameter = new SqlParameter();
      companyIdParameter.ParameterName = "@CompanyId";
      companyIdParameter.Value = this.GetId();

      cmd.Parameters.Add(companyIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int keywordId = rdr.GetInt32(2);
        int numberOfRepeats = rdr.GetInt32(3);
        if(popularWords.ContainsKey(keywordId))
        {
          popularWords[keywordId]+=numberOfRepeats;
        }
        else
        {
          popularWords.Add(keywordId, numberOfRepeats);
        }
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      Dictionary<string, int> rankedWords = new Dictionary<string, int>();
      var sorted = from pair in popularWords orderby pair.Value descending select pair;
      int count=0;
      foreach (KeyValuePair<int, int> pair in sorted)
      {
        if(count<topNumber)
        {
          rankedWords.Add(Keyword.Find(pair.Key).GetWord(), pair.Value);
        }
        count++;
      }
      return rankedWords;
    }
    public List<Category> GetCategories()
    {
      List<Category> allCategories = new List<Category> {};
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand ("SELECT categories.* FROM companies JOIN jobs ON (companies.id = jobs.company_id) JOIN categories ON (categories.id = jobs.category_id) WHERE companies.id = @CompanyId;", conn);

      SqlParameter companyIdParameter = new SqlParameter();
      companyIdParameter.ParameterName = "@CompanyId";
      companyIdParameter.Value = this.GetId();

      cmd.Parameters.Add(companyIdParameter);
      rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        int categoryId = rdr.GetInt32(0);
        string categoryName = rdr.GetString(1);
        Category newCategory = new Category (categoryName, categoryId);
        allCategories.Add(newCategory);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allCategories;
    }
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM companies WHERE id = @CompanyId;", conn);

      SqlParameter companyIdParameter = new SqlParameter();
      companyIdParameter.ParameterName = "@CompanyId";
      companyIdParameter.Value = this.GetId();

      cmd.Parameters.Add(companyIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM companies;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
