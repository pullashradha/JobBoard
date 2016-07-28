using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace JobBoard
{
  public class AccountTest : IDisposable
  {
    public AccountTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=job_board_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_GetAll_ReturnsZeroWhenDatabaseEmpty()
    {
      List<Account> testList = Account.GetAll();
      Assert.Equal(0, testList.Count);
    }

    [Fact]
    public void Test_Equals_ReturnsEqualForSameAccount()
    {
      Account testAccount = new Account("John", "Doe", "johndoe@gmail.com", "503-555-5555", 1, "This is my resume.", "jdoe");
      Account testAccount2 = new Account("John", "Doe", "johndoe@gmail.com", "503-555-5555", 1, "This is my resume.", "jdoe");
      Assert.Equal(testAccount, testAccount2);
    }

    [Fact]
    public void Test_Save_SavesAccountToDatabase()
    {
      Account testAccount = new Account("John", "Doe", "johndoe@gmail.com", "503-555-5555", 1, "This is my resume.", "jdoe");
      testAccount.Save();
      List<Account> resultAccounts = Account.GetAll();
      Assert.Equal(testAccount, resultAccounts[0]);
    }

    [Fact]
    public void Test_Find_ReturnsFoundAccount()
    {
      Account testAccount = new Account("John", "Doe", "johndoe@gmail.com", "503-555-5555", 1, "This is my resume.", "jdoe");
      testAccount.Save();
      Account foundAccount = Account.Find(testAccount.GetId());
      Assert.Equal(testAccount, foundAccount);
    }

    [Fact]
    public void Test_FindUserId_ReturnsAccount()
    {
      Account testAccount = new Account("John", "Doe", "johndoe@gmail.com", "503-555-5555", 1, "This is my resume.", "jdoe");
      testAccount.Save();

      int foundAccountId = Account.FindUserId(testAccount.GetUsername());

      Assert.Equal(testAccount.GetId(), foundAccountId);

    }

    [Fact]
    public void Test_Update_UpdatesAccountInDatabase()
    {
      Account testAccount = new Account("John", "Doe", "johndoe@gmail.com", "503-555-5555", 1, "This is my resume.", "jdoe");
      testAccount.Save();
      string testLastName = "Jones";
      testAccount.Update(testAccount.GetFirstName(), testLastName, testAccount.GetEmail(), testAccount.GetPhone(), 1, testAccount.GetResume(), testAccount.GetUsername());
      Assert.Equal(testLastName, testAccount.GetLastName());
    }

    [Fact]
    public void Test_Delete_DeletesAccountFromDatabase()
    {
      Account testAccount = new Account("John", "Doe", "johndoe@gmail.com", "503-555-5555", 1, "This is my resume.", "jdoe");
      testAccount.Save();
      Account testAccount2 = new Account("Jane", "Roe", "janeroee@gmail.com", "971-555-5555", 2, "This is also my resume.", "janedoe");
      testAccount2.Save();
      List<Account> testAccounts = new List<Account> {testAccount2};
      testAccount.DeleteOne();
      Assert.Equal(testAccounts, Account.GetAll());
    }

    // [Fact]
    // public void Test_GetRankedJobs_RanksJobsBasedOnKeywordMatchesInResumeAndDescriptions()
    // {
    //   Account testAccount = new Account("John", "Doe", "johndoe@gmail.com", "503-555-5555", 1, "I am good at JavaScript and also Javascript", "jdoe");
    //   testAccount.Save();
    //   Job testJob = new Job("Javascript Job", "You need to be good at JavaScript, JavaScript, and Ruby", 45000, 1, 1);
    //   testJob.Save();
    //   testJob.SaveWords();
    //   Job testJob2 = new Job("Ruby Job", "You need to be good at Ruby", 45000, 1, 1);
    //   testJob2.Save();
    //   testJob2.SaveWords();
    //
    //
    //   Dictionary<Job, int> expectedJobs = new Dictionary<Job, int>{{testJob, 3}, {testJob2, 1}};
    //   Dictionary<Job, int> resultJobs = testAccount.GetRankedJobs();
    //
    //   Assert.Equal(expectedJobs, resultJobs);
    // }

    // [Fact]
    // public void Test_GetRankedJobs_RanksJobsWithLarge()
    // {
    //   Account testAccount = new Account("John", "Doe", "johndoe@gmail.com", "503-555-5555", 1, "I am good at JavaScript and also Javascript", "jdoe");
    //   testAccount.Save();
    //   Job testJob = new Job("Javascript Job", "You need to be good at JavaScript, JavaScript, and Ruby", 45000, 1, 1);
    //   testJob.Save();
    //   testJob.SaveWords();
    //   Job testJob2 = new Job("Ruby Job", "You need to be good at Ruby", 45000, 1, 1);
    //   testJob2.Save();
    //   testJob2.SaveWords();
    //
    //
    //   Dictionary<Job, int> expectedJobs = new Dictionary<Job, int>{};
    //   Dictionary<Job, int> resultJobs = testAccount.GetRankedJobs();
    //
    //   Assert.Equal(expectedJobs, resultJobs);
    // }

    public void Dispose()
    {
      Account.DeleteAll();
      Job.DeleteAll();
      Keyword.DeleteAll();
    }
  }
}
