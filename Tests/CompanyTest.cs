using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace JobBoard
{
  public class CompanyTest : IDisposable
  {
    public CompanyTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=job_board_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Company.GetAll().Count;
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueIfCompanysAreTheSame()
    {
      Company firstCompany = new Company("Company");
      Company secondCompany = new Company("Company");
      Assert.Equal(firstCompany, secondCompany);
    }
    [Fact]
    public void Test_Save_SavesCompanyToDatabase()
    {
      Company testCompany = new Company("Company");
      testCompany.Save();

      List<Company> testCompanys = new List<Company>{testCompany};
      List<Company> resultCompanys = Company.GetAll();

      Assert.Equal(testCompanys, resultCompanys);
    }
    [Fact]
    public void Test_Save_AssignsIdToCompany()
    {

      Company testCompany = new Company("Company");
      testCompany.Save();
      Company savedCompany = Company.GetAll()[0];

      int result = savedCompany.GetId();
      int testId = testCompany.GetId();

      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_Find_FindsCompanyInDatabase()
    {
      Company testCompany = new Company("Company");
      testCompany.Save();

      Company foundCompany = Company.Find(testCompany.GetId());

      Assert.Equal(testCompany, foundCompany);
    }
    [Fact]
    public void Test_Update_UpdatesCompanyInDatabase()
    {
      Company testCompany = new Company("Company");
      testCompany.Save();
      string newName = "Corporation";

      testCompany.Update(newName);

      Assert.Equal(newName, testCompany.GetName());
    }
    [Fact]
    public void Test_GetJobs_ReturnsAllJobsInCompany()
    {
      Company newCompany = new Company ("Company");
      newCompany.Save();

      Job newJob = new Job ("Job A", "A job, but not cool job", 46000, newCompany.GetId(), 1);
      newJob.Save();
      newJob.SaveWords();

      List<Job> testJobList = new List<Job> {newJob};
      List<Job> resultJobList = newCompany.GetJobs();

      Assert.Equal(testJobList, resultJobList);
    }
    [Fact]
    public void Test_FindJobs_ReturnsJobsInCompanyWithKeyword()
    {
      Company newCompany = new Company ("Company");
      newCompany.Save();

      Job newJob = new Job ("Job A", "A job, but not cool job", 46000, newCompany.GetId(), 1);
      newJob.Save();
      newJob.SaveWords();

      List<Job> testJobList = new List<Job> {newJob};
      List<Job> resultJobList = newCompany.FindJobs("cool");

      Assert.Equal(testJobList, resultJobList);
    }
    [Fact]
    public void Test_GetPopularWords_ReturnsTopNumberOfMostPopularKeywordsForCompany()
    {
      Company newCompany = new Company ("Company");
      newCompany.Save();

      Job newJob = new Job ("Job A", "A job, but not cool job. Apply now!", 46000, newCompany.GetId(), 1);
      newJob.Save();
      newJob.SaveWords();

      Job newJob2 = new Job ("Job B", "We do not yet know what this job will consist of. Cool?", 46000, newCompany.GetId(), 1);
      newJob2.Save();
      newJob2.SaveWords();

      Job newJob3 = new Job ("Job c", "You are not allowed to apply for this job yet", 46000, newCompany.GetId(), 1);
      newJob3.Save();
      newJob3.SaveWords();

      Dictionary<string, int> expectedWords = new Dictionary<string, int> {{"job", 4}, {"not", 3}, {"cool", 2}, {"apply", 2}, {"yet", 2}};
      Dictionary<string, int> resultWords = newCompany.GetPopularWords(5);
      Assert.Equal(expectedWords, resultWords);
    }
    [Fact]
    public void Test_GetCategories_ReturnsAllCategoriesByCompany()
    {
      Company newCompany = new Company("Company");
      newCompany.Save();

      Category firstCategory = new Category("Category");
      Category secondCategory = new Category("Category2");
      firstCategory.Save();
      secondCategory.Save();

      Job newJob = new Job ("Job A", "A job, but not cool job. Apply now!", 46000, newCompany.GetId(), firstCategory.GetId());
      newJob.Save();
      newJob.SaveWords();

      List<Category> testCategoryList = new List<Category> {firstCategory};
      List<Category> resultCategoryList = newCompany.GetCategories();

      Assert.Equal(testCategoryList, resultCategoryList);
    }
    [Fact]
    public void Test_Delete_DeleteCompanyfromDB()
    {
      Company firstCompany = new Company("Company");
      Company secondCompany = new Company("Company");
      firstCompany.Save();
      secondCompany.Save();

      List<Company> allcourses = new List<Company>{firstCompany,secondCompany};
      allcourses.Remove(firstCompany);
      firstCompany.Delete();

      Assert.Equal(allcourses, Company.GetAll());
    }
    public void Dispose()
    {
      Company.DeleteAll();
      Job.DeleteAll();
      Keyword.DeleteAll();
      Category.DeleteAll();
    }
  }
}
