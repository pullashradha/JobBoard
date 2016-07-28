# Job Board Webpage

#### Team Project for Epicodus, 07/29/2016

#### By Sami Al-Jamal & Shradha Pulla & Charlie Baxter & Charlie Ewel

## Description

This program will allow a user to create an account to search for jobs in the database. The user can search for jobs by the company or category the jobs are under, search for the top keywords in a job description, and compare their resume to job descriptions. This program also allows companies to create profiles and add jobs to their profile.

## Setup/Installation Requirements

This program can only be accessed on a PC with Windows 10, and with Git, Atom, and Sql Server Management Studio (SSMS) installed.

* Clone this repository
* Import the database and test database:
  * Open SSMS
  * Select the following buttons from the top nav bar to open the database scripts file: File>Open>File>"Desktop\RepositoryName\Sql Databases\job_board.sql"
  * Save the job_board.sql file
  * To create the database: click the "!Execute" button on the top nav bar
  * To create the database another way, type the following into the top of the sql file:
    * CREATE DATABASE job_board;
    * GO
  * Refresh SSMS
  * Repeat the above steps to import the test database
* Test the program:
  * Type the following command into PowerShell > dnu restore
  * Next type > dnx test
  * All tests should be passing, if not run dnx test again. Otherwise, fix the errors before launching the program on the browser.
* View the web page:
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
View all jobs in a company | --- | ---
View all jobs in a category | --- | ---
Allow user to log into their account by a custom username | --- | ---
View all jobs most suited to an account's resume | --- | ---
Create a keywords list with the most common words used in a job description | --- | ---
View a list of top keywords for a job's company | --- | ---
View a list of top keywords for a job's category | --- | ---
View all jobs in a company that contain a specific keyword | --- | ---
View all jobs in a category that contain a specific keyword | --- | ---

## Future Features

HTML | CSS | C#
----- | ----- | -----
--- | --- | ---

## Support and Contact Details

Contact Epicodus for support in running this program.

## Technologies Used

* HTML
* CSS
* Bootstrap
* C#

## Links

Git Hub Repository: https://github.com/SamiAljamal/Csharp_GroupProject

## License

*This software is licensed under the Microsoft ASP.NET license.*

Copyright (c) 2016 Sami Al-Jamal & Shradha Pulla & Charlie Baxter & Charlie Ewel
