
using Nancy;
using System.Collections.Generic;

namespace JobBoard
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get ["/"] = _ => View ["index.cshtml", Account.GetAll()];
      Get ["/login"] = _ => View ["login.cshtml", Account.GetAll()];
      Get ["/accounts/new"] = _ =>  View ["account_form.cshtml"];
      Post ["/login"] = _ => {
        int userId = Request.Form ["username"];
        if(userId==-1)
        {
          return View ["user_not_found.cshtml"];
        }
        else
        {
          Account loggedAccount = Account.Find(userId);
          return View ["account.cshtml", loggedAccount];
        }
      };
      Post ["/accounts"] = _ => {
        string newUsername = Request.Form ["account-username"];
        int userId = Account.FindUserId(newUsername);
        if(userId==-1)
        {
          Account newAccount = new Account
          (
            Request.Form ["account-first-name"],
            Request.Form ["account-last-name"],
            Request.Form ["account-email"],
            Request.Form ["account-phone"],
            Request.Form ["account-education"],
            Request.Form ["account-resume"],
            newUsername
          );
          newAccount.Save();
          return View ["login.cshtml", Account.GetAll()];
        }
        else
        {
            return View["username_taken.cshtml"];
        }
      };
      Post["/keyword"]=_=>{
        Job newJob = new Job(Request.Form["title"], Request.Form["descrip"], Request.Form["salary"], 1, 1);
        newJob.Save();
        return View["result.cshtml", newJob];
      };

      Get["/jobmatches/{id}"]=parameters=>{
        Account selectedAccount = Account.Find(parameters.id);
        Dictionary<Job, int> rankedJobs = selectedAccount.GetRankedJobs();
        return View["job_matches.cshtml", rankedJobs];
      };

      Get["/jobs/{id}/keywords"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Job selectedJob = Job.Find(parameters.id);
        Category selectedCategory = Category.Find(selectedJob.GetCategoryId());
        Company selectedCompany = Company.Find(selectedJob.GetCompanyId());

        Dictionary<string, int> categoryWords = selectedCategory.GetPopularWords(20);
        Dictionary<string, int> companyWords = selectedCompany.GetPopularWords(20);

        model.Add("categoryWords", categoryWords);
        model.Add("companyWords", companyWords);
        model.Add("job", selectedJob);

        return View["jobs_keywords.cshtml", model];
      };

      Post["/jobs/{id}/keywordinfo"]=parameters=> {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Job selectedJob = Job.Find(parameters.id);
        Category selectedCategory = Category.Find(selectedJob.GetCategoryId());
        Company selectedCompany = Company.Find(selectedJob.GetCompanyId());

        string keyword = Request.Form["keyword"];

        List<Job> categoryJobs = selectedCategory.GetJobs();
        List<Job> categoryMatchedJobs = selectedCategory.FindJobs(keyword);
        List<Job> companyJobs = selectedCompany.GetJobs();
        List<Job> companyMatchedJobs = selectedCompany.FindJobs(keyword);

        model.Add("keyword", keyword);
        model.Add("categoryJobs", categoryJobs);
        model.Add("categoryMatchedJobs", categoryMatchedJobs);
        model.Add("companyJobs", companyJobs);
        model.Add("companyMatchedJobs", companyMatchedJobs);

        return View["keyword_info.cshtml", model];
      };

      Get ["/accounts/{id}/{first_name}"] = parameters => {
        Account selectedAccount = Account.Find(parameters.id);
        return View ["account.cshtml", selectedAccount];
      };
      Patch ["/accounts/{id}/{first_name}/updated"] = parameters => {
        Account selectedAccount = Account.Find(parameters.id);
        selectedAccount.Update
        (
        Request.Form ["account-first-name"],
        Request.Form ["account-last-name"],
        Request.Form ["account-email"],
        Request.Form ["account-phone"],
        Request.Form ["account-education"],
        Request.Form ["account-resume"],
        Request.Form ["account-username"]
        );
        return View ["account.cshtml", selectedAccount];
      };
      Delete ["/accounts/{id}/{first_name}/deleted"] = parameters => {
        Account selectedAccount = Account.Find(parameters.id);
        selectedAccount.DeleteOne();
        return View ["deleted.cshtml", selectedAccount];
      };
      Get ["/jobs"] = _ => {
        return View ["jobs.cshtml", Job.GetAll()];
      };
      Get ["/jobs/new"] = _ =>{
        Dictionary<string, object> model = new Dictionary<string, object> {};
        List<Company> allCompanies = Company.GetAll();
        List<Category> allCategories = Category.GetAll();
        model.Add("allCompanies", allCompanies);
        model.Add("allCategories", allCategories);
        return View ["job_form.cshtml", model];
      };
      Post ["/jobs"] = _ => {
        Job newJob = new Job
        (
          Request.Form ["job-title"],
          Request.Form ["job-description"],
          Request.Form ["job-salary"],
          Request.Form ["company"],
          Request.Form ["category"]
        );
        newJob.Save();
        newJob.SaveWords();
        return View ["jobs.cshtml", Job.GetAll()];
      };
      Get ["/jobs/{id}/{title}"] = parameters => {
        Job selectedJob = Job.Find(parameters.id);
        return View ["job.cshtml", selectedJob];
      };
      Patch ["/jobs/{id}/{title}/updated"] = parameters => {
        Job selectedJob = Job.Find(parameters.id);
        selectedJob.Update
        (
          Request.Form ["job-title"],
          Request.Form ["job-description"],
          Request.Form ["job-salary"],
          Request.Form ["company-id"],
          Request.Form ["category-id"]
        );
        return View ["job.cshtml", selectedJob];
      };
      Delete ["/jobs/{id}/{title}/deleted"] = parameters => {
        Job selectedJob = Job.Find(parameters.id);
        selectedJob.Delete();
        return View ["deleted.cshtml", selectedJob];
      };
      Get ["/companies"] = _ => {
        return View ["companies.cshtml", Company.GetAll()];
      };
      Get ["/companies/new"] = _ => View ["company_form.cshtml"];

      Post ["/companies"] = _ => {
        Company newCompany = new Company(Request.Form["company-name"]);
        newCompany.Save();
        return View ["companies.cshtml", Company.GetAll()];
      };
      Get ["/companies/{id}/{name}"] = parameters => {
        Company selectedCompany = Company.Find(parameters.id);
        return View ["company.cshtml", selectedCompany];
      };
      Patch ["/companies/{id}/{name}/updated"] = parameters => {
        Company selectedCompany = Company.Find(parameters.id);
        selectedCompany.Update(Request.Form["company-name"]);
        return View["company.cshtml", selectedCompany];
      };
      Delete ["/companies/{id}/{title}/deleted"] = parameters => {
        Company selectedCompany = Company.Find(parameters.id);
        selectedCompany.Delete();
        return View ["deleted.cshtml", selectedCompany];
      };
      Get ["/categories"] = _ => {
        return View ["categories.cshtml", Category.GetAll()];
      };
      Get ["/categories/new"] = _ => View ["category_form.cshtml"];
      Post ["/categories"] = _ => {
        Category newCategory = new Category(Request.Form["category-name"]);
        newCategory.Save();
        return View ["categories.cshtml", Category.GetAll()];
      };
      Get ["/categories/{id}/{name}"] = parameters => {
        Category selectedCategory = Category.Find(parameters.id);
        return View ["category.cshtml", selectedCategory];
      };
      Patch ["/categories/{id}/{name}/updated"] = parameters => {
        Category selectedCategory = Category.Find(parameters.id);
        selectedCategory.Update(Request.Form["category-name"]);
        return View["category.cshtml", selectedCategory];
      };
      Delete ["/categories/{id}/{title}/deleted"] = parameters => {
        Category selectedCategory = Category.Find(parameters.id);
        selectedCategory.Delete();
        return View ["deleted.cshtml", selectedCategory];
      };
      Get ["/accounts/{id}/rankedjobs"] = parameters => {
        Account selectedAccount = Account.Find(parameters.id);
        Dictionary<Job, int> rankedJobs = selectedAccount.GetRankedJobs();
        return View ["ranked.cshtml", rankedJobs];
      };

      Delete["/deleteall"]=_=> {
        Account.DeleteAll();
        Category.DeleteAll();
        Company.DeleteAll();
        Job.DeleteAll();
        Keyword.DeleteAll();
        return View ["index.cshtml"];
      };
    }
  }
}
