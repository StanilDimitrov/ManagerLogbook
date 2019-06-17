## Manager Logbook

----

#### Made by Team: Alpha Pro

- Stanil Dimitrov
- Peter Penev

#### Team leader
- Stanil Dimitrov

### Project Purpose
The web application allows Restaurant / Hotel customers to log comments about the service and view others’ feedback without to need any registration in advance.                    
Moderators have access to the customer’s feedback posts and are able to filter out or censor posts that does not comply with common sense rules.
Managers of Restaurant / Hotel can take notes (logs) about things happening on specific shift.     
System administrators can manage the accounts of managers and feedback moderators. 
They are able to initialize new Business units, Logbooks and Categories/Tags for them.

#### Technologies and other important information

The General technologies used in the developing the ManagerLogbook are following:
  - ASP.NET Core 
  - ASP.NET Identity System with admin, manager and moderator area 
  - Entity Framework Core to access database (used Code First approach)
  - Microsoft SQL Server for data storage and Entity 
  - Razor for all of the apps pages
  - AJAX for dynamic representation
  - Bootstrap
  - Used caching for business unit categories, note categories, towns and error page 404
  - Used DTO (data transfer objects) to transfer data between different application layers
  - Created server-side pagination for the list of notes
  - Integration of application with a Continuous Integration server of Azure DevOps
  - Used GitHub advantages to work with branches durring development ot features
  - Used Azure DevOps to manage personal development tasks
  - 73 % Unit test code coverage of the business logic
  
#### Features
ManagerLogbook has public and private part.

The public part of application is visible without authentication. 
The following functionalities are available for customers:
 - to access start page and perform a search via keyword and/or category and/or town location.
 - to read information about business unit (hotel, restaurant,etc.) - short description, address, telephone, email, etc.
 - to add and view comments (log entries) with positive/negative service feedback. 
 
 - Search.jpg ![](Search.jpg)
 - Statistics.jpg ![](Statistics.jpg)
 - BusinessUnitDetails.jpg ![](BusinessUnitDetails.jpg)
 
 The private part of application is visible with authentication.
 There are 3 parts: 
 
 I. Administration Part (available only for users in role "Admin").
 The following functionalities for administrator are available:
 
 - to create account, business unit and/or logbook.
 - to update account, business unit and/or logbook.
 - to specify the access for each manager for each logbook.
 - to specify the access for each moderator for each business unit.
 
 II. Moderator part (available only for users in role "Moderator").
 The following functionalities for moderators are available:
 
 - to access customer’s feedback posts via dashboard section
 - to edit customer’s feedback posts 
 - to deactivate (hide) customer’s feedback posts
 
 III. Manager Part (available only for users in role "Manager").
 The following functionalities for managers are available:
 - to add notes
 - to edit note (only his own)
 - to deactivate note 
 - to change status of note 
 - to see connected image (if it is attached)
 - to search for given word in the note
 - filter notes by category
 - filter note by given date/date range
 - to filter notes by current  day, last 7 days, last 30 days, with active status
 - to find information for last added manager, active note and total note in given logbook
 
Managers are working with all notes, added from managers responsible for given logbook. 

- CreateNote.jpg ![](CreateNote.jpg)
- LogbookDetails.jpg ![](LogbookDetails.jpg)
- ListNotes.jpg ![](ListNotes.jpg)

#### Database Diagram

- DbDiagram20190617.jpg ![](DbDiagram20190617.jpg)

#### Repository [repo]
#### Azure [azure]

[repo]: https://github.com/stanildimitrov/managerlogbook
[azure]: https://managerlogbookweb20190615103454223.azurewebsites.net/?fbclid=IwAR2NK81wGZhW-qtoO74LpaSn7eoIISDMKlaWv_QXPqL_mXIdng6zEK6tvlI

#### Manager Logbook
[![Build status](https://dev.azure.com/stanildimitrov/Manager%20Logbook/_apis/build/status/ManagerLogbook%20CI)](https://dev.azure.com/stanildimitrov/Manager%20Logbook/_build/results?buildId=46)

