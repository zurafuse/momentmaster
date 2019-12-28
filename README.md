# Moment Master
Project and Time Management Tool

## Description
Moment Master is a time management tool that gives users the ability to create new projects. You can enter the number of estimated hours for the project, the project’s start date, the project’s status, and a description. Users can add comments to the project for everyone to view. Subtasks can also be created that tie back to the parent project. All hours worked for subtasks are applied to the parent project. As the project progresses, all employees/users can log their time to that project. You can also use this software to generate time sheets for your employees.

## Ownership and Licensing
Moment Master's code is founded and owned by Timothy Horton. You have my permission to clone the application and site or use any of the code in these files, provided that you do not use my graphics and images as your own.

## Setup
1. Clone or download this repository.
2. Extract the contents of the repository.
3. Create a new database in Microsoft SQL Server called "Moment Master".
4. Open the configuration file named "appsettings.json" and replace the DefaultConnection with your database's connection string.
5. In the extracted folder, open MomentMaster.sln using Visual Studio.
6. Go to the Package Manager Console window and enter the following commands to create the tables in the database:
   Update-Database -Context UserContext\
   Update-Database -Context TimeObjectContext\
   Update-Database -Context CommentContext\
   Update-Database -Context HoursContext
7. In your SQL Server database, insert a new record into the User table to give yourself a username and password.
8. In Visual Studio, press the Debug button (or press F5) to start the web application.


## How to Use
1. Follow the setup instructures above.
2. Use your username and password to log into the site.
3. Press the "Create Project" link to create a new project. Enter the needed information. Both date and time fields are required, along with the PM/AM specification. Press "Create" to save your new project.
4. Press the "Details" link beside a project to view details and to perform actions such as adding a time entry, comment, or subtask.
5. Press the "Time Sheets" link at the top of the page for the option to generate time sheets for each user.
