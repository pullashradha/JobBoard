# Top Candidate Webpage

#### Team Project for Epicodus, 07/29/2016

#### By Sami Al-Jamal & Shradha Pulla & Charlie Baxter & Charlie Ewel

## Description

This program will allow a user to create an account to search for jobs in the database. The user can search for jobs by the company or category the jobs are under, search for the top keywords in a job description, and compare their resume to job descriptions. This program also allows companies to create profiles and add jobs to their profile.

## Setup/Installation Requirements

This program can only be accessed on a PC with Windows 10, and with Git, Atom, and Sql Server Management Studio (SSMS) installed.

* Clone this repository
* Import the database and test database:
  * Open SSMS
  * If you want an empty database, use the job_board.sql file. For a database with some sample data, use the job_board_filled.sql file.  These files can be opened by going to to File menu in SSMS, selecting "Open," selecting "File..." and then navigating to the location where you cloned the repository.  The database scripts are located in the "Sql Databases" directory.
  * To create the database: click the "!Execute" button on the top nav bar
  * Refresh SSMS
  * Repeat the above steps to import the test database, called job_board_test.sql.
* Test the program if desired:
  * Type the following command into PowerShell > dnu restore
  * Next type > dnx test
  * All tests should be passing, if not run dnx test again. Otherwise, fix the errors before launching the program on the browser.
* View the web page:
  * Type the following command into PowerShell > dnu restore
  * Type following command into PowerShell > dnx kestrel
  * Open Chrome and type in the following address: localhost:5004

## Database Creation Instructions

To build the databases from scratch, type the commands below in the Windows PowerShell:
  * Desktop> SQLCMD -S "Server-Name";
    * 1> CREATE DATABASE job_board;
    * GO
    * USE job_board;
    * GO
    * CREATE TABLE accounts
    * (
    * id INT IDENTITY(1,1),
    * first_name VARCHAR(355),
    * last_name VARCHAR(355),
    * email VARCHAR(255),
    * phone VARCHAR(255),
    * education INT,
    * resume VARCHAR(5000),
    * username VARCHAR(255)  
    * );
    * CREATE TABLE categories
    * (
    * id INT IDENTITY(1,1),
    * name VARCHAR(255)
    * );
    * CREATE TABLE companies
    * (
    * id INT IDENTITY(1,1),
    * name VARCHAR(255)
    * );
    * CREATE TABLE jobs
    * (
    * id INT IDENTITY(1,1),
    * title VARCHAR(255),
    * description VARCHAR(5000),
    * salary INT,
    * company_id INT,
    * category_id INT
    * );
    * CREATE TABLE keywords
    * (
    * id INT IDENTITY(1,1),
    * word VARCHAR(255)
    * );
    * CREATE TABLE jobs_keywords
    * (
    * id INT IDENTITY(1,1),
    * job_id INT,
    * keyword_id INT,
    * number_of_repeats INT,
    * );
    * GO
  * Exit out of SQLCMD by typing> QUIT
  * Open SSMS, click open Databases folder and check that the job_board database has been created
  * Click "New Query" button on top nav bar (above "!Execute")
  * Type following command into query text space to backup database: BACKUP DATABASE job_board TO DISK = 'C:\Users\[Account-Name]\job_board.bak'
  * Click "!Execute"
  * Right click job_board in the Databases folder: Tasks>Restore>Database
  * Confirm that there is a database to restore in the "Backup sets to restore" option field
  * Under the "Destination" input form, change the database name to: "job_board_test"
  * Click "OK", refresh SSMS, and view the new test database in the Database folder

If SQL is not connected in the PowerShell, open SSMS and click the "New Query" button (in nav bar above "!Execute"). Type commands shown above into the text space, starting from "CREATE DATABASE...".

## Known Bugs

No known bugs.

## Specifications

The program should ... | Example Input | Example Output
----- | ----- | -----
Have CRUD functionality for an account | --- | ---
Have CRUD functionality for a job listing | --- | ---
Have CRUD functionality for a company profile | --- | ---
Have CRUD functionality for a category of jobs | --- | ---
View all jobs in a company | Intel | Jobs: Structural Analyst, Cyber Fusion Analyst, etc...
View all jobs in a category | Programming | Jobs: C# Programmer, Senior Software Engineer, etc...
Allow user to log into their account by a custom username | Login: select username, selected "spulla" | "Shradha Pulla"
View all jobs most suited to an account's resume | Resume: C# programmer | Ranked Jobs: C# Programmer
View a list of top keywords for a job's company | C# Programmer | Keywords in job's company: .net, supply chain, dtc, etc...
View a list of top keywords for a job's category | C# Programmer | Keywords in job's category: experience, development, mvs, etc...

## Future Features

HTML | CSS | C#
----- | ----- | -----
Format site to look more like a job board | Black text on white background | List all keywords for a category & company
Change update job form to a textarea for the description input | Generate a word cloud for the keywords lists | Implement authentication of an account, add a password option
Add "Top Candidate" site title to top left of navbar | --- | Refine & refactor algorithm used to generate keywords
--- | --- | Add an API or other program to generate job listings

## Presentation Notes

- Why?
	* Job boards present you with a ton of information and can be overwhelming.
	* Many companies use software that takes a resume and scans it for key words.  These words can be in the actual description.
	* Our project helps you tailor your resume to make it more appealing to employers, in terms of getting a specific job, applying at a certain company, or within a given industry.

- Process
	* We rotated every day.
	* Started every day with a standup to talk about issues we noticed from the previous day, and what we wanted to accomplish in the day ahead.
	* Each pair worked on different documents and features to prevent merge conflicts and issues with the site not running in the way that the other pair expected.
	* We kept an eye out for issues and let the team working on that feature know about it rather than going in and fixing it ourselves (divided responsibilities well).
	* We focused first and foremost on getting a working MVP.  For new features, we decided to set a time limit to see how well we could get it working.  If it didn't work well enough, we moved on.

- Challenges
	* Coming up with the right algorithm. Frequency of words isn't the best indication of what is a keyword in a resume or job posting.
	* Maintaining a login session.  Our site doesn't currently do this.
	* Deciding on the scope of the project.  Wordclouds!  APIs!  Webscrapers!
	* SQL timeout on tests

## Support and Contact Details

Contact Epicodus for support in running this program.

## Technologies Used

* C#
* Nancy
* Razor
* HTML
* CSS
* Bootstrap

## Links

Git Hub Repository: https://github.com/SamiAljamal/Csharp_GroupProject

## License

*This software is licensed under the Microsoft ASP.NET license.*

Copyright (c) 2016 Sami Al-Jamal & Shradha Pulla & Charlie Baxter & Charlie Ewel
