using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Linq;

namespace JobBoard
{
  public class Category
  {
    private string _name;
    private int _id;

    public Category(string name, int id=0)
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

    public override bool Equals(System.Object otherCategory)
    {
      if (!(otherCategory is Category))
      {
        return false;
      }
      else
      {
        Category newCategory = (Category) otherCategory;
        bool idEquality = this.GetId() == newCategory.GetId();
        bool nameEquality = this.GetName() == newCategory.GetName();
        return (idEquality && nameEquality);
      }
    }

    public static List<Category> GetAll()
    {
      List<Category> allCategorys = new List<Category>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM categories;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int CategoryId = rdr.GetInt32(0);
        string CategoryName = rdr.GetString(1);
        Category newCategory = new Category(CategoryName, CategoryId);
        allCategorys.Add(newCategory);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allCategorys;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO categories (name) OUTPUT INSERTED.id VALUES (@CategoryName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@CategoryName";
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

    public static Category Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand ("SELECT * FROM categories WHERE id = @CategoryId;", conn);

      SqlParameter CategoryIdParameter = new SqlParameter();
      CategoryIdParameter.ParameterName = "@CategoryId";
      CategoryIdParameter.Value = id;
      cmd.Parameters.Add(CategoryIdParameter);

      rdr = cmd.ExecuteReader();

      int foundCategoryId = 0;
      string foundCategoryName = null;

      while(rdr.Read())
      {
        foundCategoryId = rdr.GetInt32(0);
        foundCategoryName = rdr.GetString(1);
      }
      Category foundCategory = new Category(foundCategoryName, foundCategoryId);

      if(rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCategory;
    }

    public void Update(string newName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      this.SetName(newName);

      SqlCommand cmd = new SqlCommand("UPDATE categories SET name = @NewName WHERE id = @CategoryId;", conn);

      SqlParameter newNameParameter = new SqlParameter();
      newNameParameter.ParameterName = "@NewName";
      newNameParameter.Value = newName;
      cmd.Parameters.Add(newNameParameter);

      SqlParameter CategoryIdParameter = new SqlParameter();
      CategoryIdParameter.ParameterName = "@CategoryId";
      CategoryIdParameter.Value = this.GetId();
      cmd.Parameters.Add(CategoryIdParameter);


      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM categories WHERE id = @CategoryId;", conn);

      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@CategoryId";
      categoryIdParameter.Value = this.GetId();

      cmd.Parameters.Add(categoryIdParameter);
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
      SqlCommand cmd = new SqlCommand("DELETE FROM categories;", conn);
      cmd.ExecuteNonQuery();
    }
    public List<Job> GetJobs ()
    {
      List<Job> foundJobs = new List<Job> {};
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      SqlCommand cmd = new SqlCommand ("SELECT * FROM jobs WHERE category_id = @CategoryId;", conn);

      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@CategoryId";
      categoryIdParameter.Value = this.GetId();

      cmd.Parameters.Add(categoryIdParameter);

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
      SqlCommand cmd = new SqlCommand ("SELECT jobs.* FROM keywords JOIN jobs_keywords ON (keywords.id = jobs_keywords.keyword_id) JOIN jobs ON (jobs_keywords.job_id = jobs.id) WHERE jobs.category_id = @CategoryId AND keywords.word = @Keyword;", conn);

      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@CategoryId";
      categoryIdParameter.Value = this.GetId();

      SqlParameter keywordParameter = new SqlParameter();
      keywordParameter.ParameterName = "@Keyword";
      keywordParameter.Value = searchKeyword.ToLower();

      cmd.Parameters.Add(keywordParameter);
      cmd.Parameters.Add(categoryIdParameter);

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

      SqlCommand cmd = new SqlCommand("SELECT jobs_keywords.* FROM categories JOIN jobs ON (categories.id = jobs.category_id) JOIN jobs_keywords ON (jobs.id = jobs_keywords.job_id) WHERE categories.id = @CategoryId;", conn);

      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@CategoryId";
      categoryIdParameter.Value = this.GetId();

      cmd.Parameters.Add(categoryIdParameter);

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
    public List<Company> GetCompanies()
    {
      List<Company> allCompanies = new List<Company> {};
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand ("SELECT companies.* FROM categories JOIN jobs ON (categories.id = jobs.category_id) JOIN companies ON (companies.id = jobs.company_id) WHERE categories.id = @CategoryId;", conn);

      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@CategoryId";
      categoryIdParameter.Value = this.GetId();

      cmd.Parameters.Add(categoryIdParameter);
      rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        int companyId = rdr.GetInt32(0);
        string companyName = rdr.GetString(1);
        Company newCompany = new Company (companyName, companyId);
        allCompanies.Add(newCompany);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allCompanies;
    }
  }
}
