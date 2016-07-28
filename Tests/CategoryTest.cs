using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace JobBoard
{
  public class CategoryTest : IDisposable
  {
    public CategoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=job_board_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Category.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfCategorysAreTheSame()
    {
      Category firstCategory = new Category("Category");
      Category secondCategory = new Category("Category");
      Assert.Equal(firstCategory, secondCategory);
    }

    [Fact]
    public void Test_Save_SavesCategoryToDatabase()
    {
      Category testCategory = new Category("Category");
      testCategory.Save();

      List<Category> testCategorys = new List<Category>{testCategory};
      List<Category> resultCategorys = Category.GetAll();

      Assert.Equal(testCategorys, resultCategorys);
    }

    [Fact]
    public void Test_Save_AssignsIdToCategory()
    {

      Category testCategory = new Category("Category");
      testCategory.Save();
      Category savedCategory = Category.GetAll()[0];

      int result = savedCategory.GetId();
      int testId = testCategory.GetId();

      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_Find_FindsCategoryInDatabase()
    {
      Category testCategory = new Category("Category");
      testCategory.Save();

      Category foundCategory = Category.Find(testCategory.GetId());

      Assert.Equal(testCategory, foundCategory);
    }

    [Fact]
    public void Test_Update_UpdatesCategoryInDatabase()
    {
      Category testCategory = new Category("Category");
      testCategory.Save();
      string newName = "Category2";

      testCategory.Update(newName);

      Assert.Equal(newName, testCategory.GetName());
    }

    [Fact]
    public void Test_Delete_DeleteCategoryfromDB()
    {
      Category firstCategory = new Category("Category");
      Category secondCategory = new Category("Category");
      firstCategory.Save();
      secondCategory.Save();

      List<Category> allcourses = new List<Category>{firstCategory,secondCategory};
      allcourses.Remove(firstCategory);
      firstCategory.Delete();

      Assert.Equal(allcourses, Category.GetAll());
    }
    [Fact]
    public void Test_GetJobs_ReturnsAllJobsInCategory()
    {
      Category newCategory = new Category ("Category");
      newCategory.Save();

      Job newJob = new Job ("Job A", "A job, but not cool job", 46000, 1, newCategory.GetId());
      newJob.Save();
      newJob.SaveWords();

      List<Job> testJobList = new List<Job> {newJob};
      List<Job> resultJobList = newCategory.GetJobs();

      Assert.Equal(testJobList, resultJobList);
    }

    [Fact]
    public void Test_FindJobs_ReturnsJobsInCategoryWithKeyword()
    {
      Category newCategory = new Category ("Category");
      newCategory.Save();

      Job newJob = new Job ("Job A", "A job, but not cool job", 46000, 1, newCategory.GetId());
      newJob.Save();
      newJob.SaveWords();

      List<Job> testJobList = new List<Job> {newJob};
      List<Job> resultJobList = newCategory.FindJobs("cool");

      Assert.Equal(testJobList, resultJobList);
    }
    [Fact]
    public void Test_GetPopularWords_ReturnsTopNumberOfMostPopularKeywordsForCategory()
    {
      Category newCategory = new Category ("Category");
      newCategory.Save();

      Job newJob = new Job ("Job A", "A job, but not cool job. Apply now!", 46000, 1, newCategory.GetId());
      newJob.Save();
      newJob.SaveWords();

      Job newJob2 = new Job ("Job B", "We do not yet know what this job will consist of. Cool?", 46000, 1, newCategory.GetId());
      newJob2.Save();
      newJob2.SaveWords();

      Job newJob3 = new Job ("Job c", "You are not allowed to apply for this job yet", 46000, 1, newCategory.GetId());
      newJob3.Save();
      newJob3.SaveWords();

      Dictionary<string, int> expectedWords = new Dictionary<string, int> {{"job", 4}, {"not", 3}, {"cool", 2}, {"apply", 2}, {"yet", 2}};
      Dictionary<string, int> resultWords = newCategory.GetPopularWords(5);
      Assert.Equal(expectedWords, resultWords);
    }
    [Fact]
    public void Test_GetCompanies_ReturnsAllCompaniesByCategory()
    {
      Category newCategory = new Category("Category");
      newCategory.Save();

      Company firstCompany = new Company("Company");
      Company secondCompany = new Company("Company2");
      firstCompany.Save();
      secondCompany.Save();

      Job newJob = new Job ("Job A", "A job, but not cool job. Apply now!", 46000, firstCompany.GetId(), newCategory.GetId());
      newJob.Save();
      newJob.SaveWords();

      List<Company> testCompanyList = new List<Company> {firstCompany};
      List<Company> resultCompanyList = newCategory.GetCompanies();

      Assert.Equal(testCompanyList, resultCompanyList);
    }
    public void Dispose()
    {
      Category.DeleteAll();
      Keyword.DeleteAll();
      Job.DeleteAll();
      Company.DeleteAll();
    }
  }
}
