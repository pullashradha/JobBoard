using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Linq;

namespace JobBoard
{
  public class Account
  {
    private int _id;
    private string _firstName;
    private string _lastName;
    private string _email;
    private string _phone;
    private int _education;
    private string _resume;
    private string _username;

    public Account (string firstName, string lastName, string email, string phone, int education, string resume, string username, int id = 0)
    {
      _id = id;
      _firstName = firstName.Trim();
      _lastName = lastName.Trim();
      _email = email.Trim();
      _phone = phone.Trim();
      _education = education;
      _resume = resume.Trim();
      _username = username.Trim();

    }

    public override bool Equals(System.Object otherAccount)
    {
      if (!(otherAccount is Account))
      {
        return false;
      }
      else
      {
        Account newAccount = (Account) otherAccount;
        bool idEquality = this.GetId() == newAccount.GetId();
        bool firstNameEquality = this.GetFirstName() == newAccount.GetFirstName();
        bool lastNameEquality = this.GetLastName() == newAccount.GetLastName();
        bool emailEquality = this.GetEmail() == newAccount.GetEmail();
        bool phoneEquality = this.GetPhone() == newAccount.GetPhone();
        bool educationEquality = this.GetEducation() == newAccount.GetEducation();
        bool resumeEquality = this.GetResume() == newAccount.GetResume();
        bool usernameEquality = this.GetUsername() == newAccount.GetUsername();
        return (idEquality && firstNameEquality && lastNameEquality && emailEquality && phoneEquality && educationEquality && resumeEquality && usernameEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetFirstName()
    {
      return _firstName;
    }
    public string GetLastName()
    {
      return _lastName;
    }
    public string GetEmail()
    {
      return _email;
    }
    public string GetPhone()
    {
      return _phone;
    }
    public int GetEducation()
    {
      return _education;
    }
    public string GetResume()
    {
      return _resume;
    }
    public string GetUsername()
    {
      return _username;
    }

    public void SetFirstName(string newFirstName)
    {
      _firstName = newFirstName;
    }
    public void SetLastName(string newLastName)
    {
      _lastName = newLastName;
    }
    public void SetEmail(string newEmail)
    {
      _email = newEmail;
    }
    public void SetPhone(string newPhone)
    {
      _phone = newPhone;
    }
    public void SetEducation(int newEducation)
    {
      _education = newEducation;
    }
    public void SetResume(string newResume)
    {
      _resume = newResume;
    }
    public void SetUsername(string newUsername)
    {
      _username = newUsername;
    }


    public static List<Account> GetAll()
    {
      List<Account> allAccounts = new List<Account>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM accounts;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int accountId = rdr.GetInt32(0);
        string accountFirstName = rdr.GetString(1);
        string accountLastName = rdr.GetString(2);
        string accountEmail = rdr.GetString(3);
        string accountPhone = rdr.GetString(4);
        int accountEducation = rdr.GetInt32(5);
        string accountResume = rdr.GetString(6);
        string accountUsername = rdr.GetString(7);

        Account newAccount = new Account(accountFirstName, accountLastName, accountEmail, accountPhone, accountEducation, accountResume, accountUsername, accountId);
        allAccounts.Add(newAccount);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allAccounts;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO accounts (first_name, last_name, email, phone, education, resume, username) OUTPUT INSERTED.id VALUES (@FirstName, @LastName, @Email, @Phone, @Education, @Resume, @Username);", conn);

      SqlParameter firstNameParameter = new SqlParameter();
      firstNameParameter.ParameterName = "@FirstName";
      firstNameParameter.Value = this.GetFirstName().Trim();
      cmd.Parameters.Add(firstNameParameter);

      SqlParameter lastNameParameter = new SqlParameter();
      lastNameParameter.ParameterName = "@LastName";
      lastNameParameter.Value = this.GetLastName().Trim();
      cmd.Parameters.Add(lastNameParameter);

      SqlParameter emailParameter = new SqlParameter();
      emailParameter.ParameterName = "@Email";
      emailParameter.Value = this.GetEmail().Trim();
      cmd.Parameters.Add(emailParameter);

      SqlParameter phoneParameter = new SqlParameter();
      phoneParameter.ParameterName = "@Phone";
      phoneParameter.Value = this.GetPhone().Trim();
      cmd.Parameters.Add(phoneParameter);

      SqlParameter educationParameter = new SqlParameter();
      educationParameter.ParameterName = "@Education";
      educationParameter.Value = this.GetEducation();
      cmd.Parameters.Add(educationParameter);

      SqlParameter resumeParameter = new SqlParameter();
      resumeParameter.ParameterName = "@Resume";
      resumeParameter.Value = this.GetResume().Trim();
      cmd.Parameters.Add(resumeParameter);

      SqlParameter usernameParameter = new SqlParameter();
      usernameParameter.ParameterName = "@Username";
      usernameParameter.Value = this.GetUsername().Trim();
      cmd.Parameters.Add(usernameParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Account Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM accounts WHERE id = @AccountId;", conn);
      SqlParameter accountIdParameter = new SqlParameter();
      accountIdParameter.ParameterName = "@AccountId";
      accountIdParameter.Value = id.ToString();
      cmd.Parameters.Add(accountIdParameter);
      rdr = cmd.ExecuteReader();

      List<Account> foundAccounts = new List<Account>{};

      while(rdr.Read())
      {
        int foundAccountId = rdr.GetInt32(0);
        string foundAccountFirstName = rdr.GetString(1);
        string foundAccountLastName = rdr.GetString(2);
        string foundAccountEmail = rdr.GetString(3);
        string foundAccountPhone = rdr.GetString(4);
        int foundAccountEducation = rdr.GetInt32(5);
        string foundAccountResume = rdr.GetString(6);
        string foundAccountUsername = rdr.GetString(7);
        Account foundAccount = new Account(foundAccountFirstName, foundAccountLastName, foundAccountEmail, foundAccountPhone, foundAccountEducation, foundAccountResume, foundAccountUsername, foundAccountId);
        foundAccounts.Add(foundAccount);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundAccounts[0];
    }

    public static int FindUserId(string username)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM accounts WHERE username = @AccountUsername;", conn);
      SqlParameter accountUsernameParameter = new SqlParameter();
      accountUsernameParameter.ParameterName = "@AccountUsername";
      accountUsernameParameter.Value = username;
      cmd.Parameters.Add(accountUsernameParameter);
      rdr = cmd.ExecuteReader();

      int foundAccountId = -1;

      while(rdr.Read())
      {
        foundAccountId = rdr.GetInt32(0);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundAccountId;
    }

    public void Update(string newFirstName, string newLastName, string newEmail, string newPhone, int newEducation, string newResume, string newUsername)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE accounts SET first_name = @NewFirstName, last_name = @NewLastName, email = @NewEmail, phone = @NewPhone, education = @NewEducation, resume = @NewResume, username = @NewUsername OUTPUT INSERTED.first_name, INSERTED.last_name, INSERTED.email, INSERTED.phone, INSERTED.education, INSERTED.resume, INSERTED.username WHERE id = @AccountId;", conn);

      SqlParameter newFirstNameParameter = new SqlParameter();
      newFirstNameParameter.ParameterName = "@NewFirstName";
      newFirstNameParameter.Value = newFirstName.Trim();
      cmd.Parameters.Add(newFirstNameParameter);

      SqlParameter newLastNameParameter = new SqlParameter();
      newLastNameParameter.ParameterName = "@NewLastName";
      newLastNameParameter.Value = newLastName.Trim();
      cmd.Parameters.Add(newLastNameParameter);

      SqlParameter newEmailParameter = new SqlParameter();
      newEmailParameter.ParameterName = "@NewEmail";
      newEmailParameter.Value = newEmail.Trim();
      cmd.Parameters.Add(newEmailParameter);

      SqlParameter newPhoneParameter = new SqlParameter();
      newPhoneParameter.ParameterName = "@NewPhone";
      newPhoneParameter.Value = newPhone.Trim();
      cmd.Parameters.Add(newPhoneParameter);

      SqlParameter newEducationParameter = new SqlParameter();
      newEducationParameter.ParameterName = "@NewEducation";
      newEducationParameter.Value = newEducation;
      cmd.Parameters.Add(newEducationParameter);

      SqlParameter newResumeParameter = new SqlParameter();
      newResumeParameter.ParameterName = "@NewResume";
      newResumeParameter.Value = newResume.Trim();
      cmd.Parameters.Add(newResumeParameter);

      SqlParameter newUsernameParameter = new SqlParameter();
      newUsernameParameter.ParameterName = "@NewUsername";
      newUsernameParameter.Value = newUsername.Trim();
      cmd.Parameters.Add(newUsernameParameter);

      SqlParameter accountIdParameter = new SqlParameter();
      accountIdParameter.ParameterName = "@AccountId";
      accountIdParameter.Value = this.GetId();
      cmd.Parameters.Add(accountIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._firstName = rdr.GetString(0);
        this._lastName = rdr.GetString(1);
        this._email = rdr.GetString(2);
        this._phone = rdr.GetString(3);
        this._education = rdr.GetInt32(4);
        this._resume = rdr.GetString(5);
        this._username = rdr.GetString(6);
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

    public void DeleteOne()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM accounts WHERE id = @AccountId;", conn);

      SqlParameter accountIdParameter = new SqlParameter();
      accountIdParameter.ParameterName = "@AccountId";
      accountIdParameter.Value = this.GetId();

      cmd.Parameters.Add(accountIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public Dictionary<string, int> UniqueWordCount()
    {
      List<string> prepositions = new List<string>{"aboard", "about", "above", "across", "after", "against", "along", "amid", "among", "anti", "around", "as", "at", "before", "behind", "below", "beneath", "beside", "besides", "between", "beyond", "but", "by", "concerning", "considering", "despite", "down", "during", "except", "excepting", "excluding", "following", "for", "from", "in", "inside", "into", "like", "minus", "near", "of", "off", "on", "onto", "opposite", "outside", "over", "past", "per", "plus", "regarding", "round", "save", "since", "than", "through", "to", "toward", "towards", "underneath", "under", "unlike", "until", "up", "upon", "versus", "via", "with", "within", "without", "a", "an", "the"};
      List<string> commonWords = new List<string>{"any","that","our","you","just","and","this","or","is","will","are","be","can","have","had","requirements","compentencies","duties","responsibilities","qualifications","essential","such","each"};
      Dictionary<string, int> UniqueWords = new Dictionary<string, int>{};
      string resume = this.GetResume().ToLower() + " ";
      string backTrimmedResume = Regex.Replace(resume, @"[\.,\,,\?,\!,\),\;,\:] ", " ");
      string trimmedResume = Regex.Replace(backTrimmedResume, @" [\(]", " ");
      Regex noncharacter = new Regex(@"[^A-Za-z0-9+#., ]+");
      string mistranslatedCharsRemoved = noncharacter.Replace(trimmedResume, "");
      Regex whitespace = new Regex(@"\s+");
      string[] wordList = whitespace.Split(mistranslatedCharsRemoved);
      for(int i=0; i < wordList.Length-1; i++)
      {
        if(!prepositions.Contains(wordList[i].ToLower()) && !commonWords.Contains(wordList[i].ToLower()))
        {
          int count=0;
          string noPuncWord = Regex.Replace(wordList[i], @"[\.,\,,\?,\!,\),\;,\:]", "");
          if(!UniqueWords.ContainsKey(wordList[i].ToLower()))
          {
            for(int j = i; j < wordList.Length-1; j++)
            {
              if(wordList[i]==wordList[j]) count+=1;
            }
            if (noPuncWord.Length > 1)
            {
              if(Char.IsUpper(noPuncWord[0]) && Char.IsUpper(noPuncWord[1])) count *= 2;
              if(Char.IsUpper(noPuncWord[0]) && !Char.IsUpper(noPuncWord[1])) count *= 2;
            }
            UniqueWords.Add(wordList[i].ToLower(), count);
          }
        }
      }
      Dictionary<string, int> items = new Dictionary<string, int>();
      var sorted = from pair in UniqueWords orderby pair.Value descending select pair;
      foreach (KeyValuePair<string, int> pair in sorted)
      {
        items.Add(pair.Key, pair.Value);
      }

      return items;
    }

    public Dictionary<string, int> CompoundWordCount()
    {
      List<string> prepositions = new List<string>{"aboard", "about", "above", "across", "after", "against", "along", "amid", "among", "anti", "around", "as", "at", "before", "behind", "below", "beneath", "beside", "besides", "between", "beyond", "but", "by", "concerning", "considering", "despite", "down", "during", "except", "excepting", "excluding", "following", "for", "from", "in", "inside", "into", "like", "minus", "near", "of", "off", "on", "onto", "opposite", "outside", "over", "past", "per", "plus", "regarding", "round", "save", "since", "than", "through", "to", "toward", "towards", "underneath", "under", "unlike", "until", "up", "upon", "versus", "via", "with", "within", "without", "a", "an", "the"};
      List<string> commonWords = new List<string>{"any","that","our","you","just","and","this","or","is","will","are","be","can","have","had","requirements","compentencies","duties","responsibilities","qualifications","essential","such","each"};
      Dictionary<string, int> compoundWords = new Dictionary<string, int>{};
      string resume = this.GetResume() + " ";
      string backTrimmedResume = Regex.Replace(resume, @"[\.,\,,\?,\!,\),\;,\:] ", " ");
      string trimmedResume = Regex.Replace(backTrimmedResume, @" [\(]", " ");
      Regex noncharacter = new Regex(@"[^A-Za-z0-9+#., ]+");
      string mistranslatedCharsRemoved = noncharacter.Replace(trimmedResume, "");

      Regex whitespace = new Regex(@"\s+");
      string[] wordList = whitespace.Split(mistranslatedCharsRemoved);
      for(int i=0; i < wordList.Length-2; i++)
      {
        int count = 0;
        if(!compoundWords.ContainsKey(wordList[i] + " " + wordList[i+1]) && char.IsUpper(wordList[i][0]) && char.IsUpper(wordList[i+1][0]) && !prepositions.Contains(wordList[i].ToLower()) && !commonWords.Contains(wordList[i].ToLower()))
        {
          for(int j = i; j < wordList.Length-1; j++)
          {
            if(wordList[i] + " " + wordList[i+1] == wordList[j] + " " + wordList[j+1]) count+=3;
          }
          compoundWords.Add(wordList[i] + " " + wordList[i+1], count);
        }
      }
      Dictionary<string, int> items = new Dictionary<string, int>();
      var sorted = from pair in compoundWords orderby pair.Value descending select pair;
      foreach (KeyValuePair<string, int> pair in sorted)
      {
        items.Add(pair.Key, pair.Value);
      }
      return items;
    }

    public Dictionary<Job, int> GetRankedJobs()
    {
      Dictionary<string, int> resumeWords = this.UniqueWordCount();
      Dictionary<int, int> scoredJobs = new Dictionary<int, int>{};
      foreach (KeyValuePair<string, int> pair in resumeWords)
      {
        int keywordId=Keyword.KeywordSearch(pair.Key);
        if(keywordId!=-1)
        {
          Keyword keyword = new Keyword(pair.Key, keywordId);
          Dictionary<int, int> keywordJobs = keyword.GetJobs();
          foreach(KeyValuePair<int, int> jobScore in keywordJobs)
          {
            if(scoredJobs.ContainsKey(jobScore.Key))
            {
              scoredJobs[jobScore.Key]+=(jobScore.Value*pair.Value);
            }
            else
            {
              scoredJobs.Add(jobScore.Key, jobScore.Value);
            }
          }
        }
      }
      Dictionary<Job, int> rankedJobs = new Dictionary<Job, int>();
      var sorted = from pair in scoredJobs orderby pair.Value descending select pair;
      foreach (KeyValuePair<int, int> pair in sorted)
      {
        rankedJobs.Add(Job.Find(pair.Key), pair.Value);
      }
      return rankedJobs;
    }

    public static void DeleteAll()
    {
     SqlConnection conn = DB.Connection();
     conn.Open();
     SqlCommand cmd = new SqlCommand("DELETE FROM accounts;", conn);
     cmd.ExecuteNonQuery();
    }
  }
}
