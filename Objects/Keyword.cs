using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Linq;

namespace JobBoard
{
  public class Keyword
  {
    private string _word;
    private int _id;

    public Keyword(string word, int id=0)
    {
      _word = word;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetWord()
    {
      return _word;
    }

    public void SetWord(string newWord)
    {
      _word = newWord;
    }

    public override bool Equals(System.Object otherKeyword)
    {
      if (!(otherKeyword is Keyword))
      {
        return false;
      }
      else
      {
        Keyword newKeyword = (Keyword) otherKeyword;
        bool idEquality = this.GetId() == newKeyword.GetId();
        bool wordEquality = this.GetWord() == newKeyword.GetWord();
        return (idEquality && wordEquality);
      }
    }

    public static List<Keyword> GetAll()
    {
      List<Keyword> allKeywords = new List<Keyword>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM keywords;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int KeywordId = rdr.GetInt32(0);
        string KeywordWord = rdr.GetString(1);
        Keyword newKeyword = new Keyword(KeywordWord, KeywordId);
        allKeywords.Add(newKeyword);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allKeywords;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO keywords (word) OUTPUT INSERTED.id VALUES (@KeywordWord);", conn);

      SqlParameter wordParameter = new SqlParameter();
      wordParameter.ParameterName = "@KeywordWord";
      wordParameter.Value = this.GetWord();

      cmd.Parameters.Add(wordParameter);

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

    public static Keyword Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand ("SELECT * FROM keywords WHERE id = @KeywordId;", conn);

      SqlParameter KeywordIdParameter = new SqlParameter();
      KeywordIdParameter.ParameterName = "@KeywordId";
      KeywordIdParameter.Value = id;
      cmd.Parameters.Add(KeywordIdParameter);

      rdr = cmd.ExecuteReader();

      int foundKeywordId = 0;
      string foundKeywordWord = null;

      while(rdr.Read())
      {
        foundKeywordId = rdr.GetInt32(0);
        foundKeywordWord = rdr.GetString(1);
      }
      Keyword foundKeyword = new Keyword(foundKeywordWord, foundKeywordId);

      if(rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundKeyword;
    }

    public void Update(string newWord)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      this.SetWord(newWord);

      SqlCommand cmd = new SqlCommand("UPDATE keywords SET word = @NewWord WHERE id = @KeywordId;", conn);

      SqlParameter newWordParameter = new SqlParameter();
      newWordParameter.ParameterName = "@NewWord";
      newWordParameter.Value = newWord;
      cmd.Parameters.Add(newWordParameter);

      SqlParameter KeywordIdParameter = new SqlParameter();
      KeywordIdParameter.ParameterName = "@KeywordId";
      KeywordIdParameter.Value = this.GetId();
      cmd.Parameters.Add(KeywordIdParameter);


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

      SqlCommand cmd = new SqlCommand("DELETE FROM keywords WHERE id = @KeywordId;", conn);

      SqlParameter keywordIdParameter = new SqlParameter();
      keywordIdParameter.ParameterName = "@KeywordId";
      keywordIdParameter.Value = this.GetId();

      cmd.Parameters.Add(keywordIdParameter);
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
      SqlCommand cmd = new SqlCommand("DELETE FROM keywords;", conn);
      cmd.ExecuteNonQuery();
    }
    public Dictionary<int, int> GetJobs()
    {
      Dictionary<int, int> matchedJobs = new Dictionary<int, int>{};

      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand("SELECT * FROM jobs_keywords WHERE keyword_id = @KeywordId;", conn);

      SqlParameter keywordIdParameter = new SqlParameter();
      keywordIdParameter.ParameterName = "@KeywordId";
      keywordIdParameter.Value = this.GetId();

      cmd.Parameters.Add(keywordIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int jobId = rdr.GetInt32(1);
        int numberOfRepeats = rdr.GetInt32(3);
        matchedJobs.Add(jobId, numberOfRepeats);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      Dictionary<int, int> rankedJobs = new Dictionary<int, int>();
      var sorted = from pair in matchedJobs orderby pair.Value descending select pair;
      int count=0;
      foreach (KeyValuePair<int, int> pair in sorted)
      {
        if(count<20)
        {
          rankedJobs.Add(pair.Key, pair.Value);
        }
        count++;
      }
      return rankedJobs;
    }

    public static int KeywordSearch(string searchString)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand("SELECT * FROM keywords WHERE word = @KeywordName;", conn);

      SqlParameter keywordNameParameter = new SqlParameter();
      keywordNameParameter.ParameterName = "@KeywordName";
      keywordNameParameter.Value = searchString.ToLower();

      cmd.Parameters.Add(keywordNameParameter);

      rdr = cmd.ExecuteReader();

      int keywordId = -1;

      while(rdr.Read())
      {
        keywordId = rdr.GetInt32(0);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return keywordId;
    }
  }
}
