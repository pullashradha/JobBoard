using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace JobBoard
{
  public class KeywordTest : IDisposable
  {
    public KeywordTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=job_board_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Keyword.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfKeywordsAreTheSame()
    {
      Keyword firstKeyword = new Keyword("Keyword");
      Keyword secondKeyword = new Keyword("Keyword");
      Assert.Equal(firstKeyword, secondKeyword);
    }

    [Fact]
    public void Test_Save_SavesKeywordToDatabase()
    {
      Keyword testKeyword = new Keyword("Keyword");
      testKeyword.Save();

      List<Keyword> testKeywords = new List<Keyword>{testKeyword};
      List<Keyword> resultKeywords = Keyword.GetAll();

      Assert.Equal(testKeywords, resultKeywords);
    }

    [Fact]
    public void Test_Save_AssignsIdToKeyword()
    {

      Keyword testKeyword = new Keyword("Keyword");
      testKeyword.Save();
      Keyword savedKeyword = Keyword.GetAll()[0];

      int result = savedKeyword.GetId();
      int testId = testKeyword.GetId();

      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_Find_FindsKeywordInDatabase()
    {
      Keyword testKeyword = new Keyword("Keyword");
      testKeyword.Save();

      Keyword foundKeyword = Keyword.Find(testKeyword.GetId());

      Assert.Equal(testKeyword, foundKeyword);
    }

    [Fact]
    public void Test_Update_UpdatesKeywordInDatabase()
    {
      Keyword testKeyword = new Keyword("Keyword");
      testKeyword.Save();
      string newWord = "Keyword2";

      testKeyword.Update(newWord);

      Assert.Equal(newWord, testKeyword.GetWord());
    }

    [Fact]
    public void Test_Delete_DeleteKeywordfromDB()
    {
      Keyword firstKeyword = new Keyword("Keyword");
      Keyword secondKeyword = new Keyword("Keyword");
      firstKeyword.Save();
      secondKeyword.Save();

      List<Keyword> allcourses = new List<Keyword>{firstKeyword,secondKeyword};
      allcourses.Remove(firstKeyword);
      firstKeyword.Delete();

      Assert.Equal(allcourses, Keyword.GetAll());
    }

    [Fact]
    public void Test_GetJobs_ReturnsRankedListOfJobs()
    {
      Job firstJob = new Job("Ruby Job", "Know Ruby and Javascript", 45000, 1, 1);
      Job secondJob = new Job("Javascript Job", "Know Javascript and Javascript frameworks", 45000, 1, 1);
      Job thirdJob = new Job("Logger", "Cut down trees. Beard is a must, employee responsible for own flannel", 45000, 1, 1);
      firstJob.Save();
      secondJob.Save();
      firstJob.SaveWords();
      secondJob.SaveWords();
      thirdJob.SaveWords();

      int keywordId = Keyword.KeywordSearch("Javascript");
      Keyword testKeyword = new Keyword("Javascript", keywordId);

      Dictionary<int, int> resultJobs = testKeyword.GetJobs();
      Dictionary<int, int> expectedJobs = new Dictionary<int, int> {{secondJob.GetId(), 2}, {firstJob.GetId(), 1}};

      Assert.Equal(expectedJobs, resultJobs);
    }

    public void Dispose()
    {
      Keyword.DeleteAll();
      Job.DeleteAll();
    }
  }
}
