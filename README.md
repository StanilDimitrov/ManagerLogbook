## Manager Logbook

----

#### Made by Team: Alpha Pro

- Stanil Dimitrov
- Peter Penev

#### Team leader
- Stanil Dimitrov

### Project Purpose
The web application allows Restaurant / Hotel customers to log comments about the service and view others’ feedback without to need any registration in advance.                    
Moderators have access to the customer’s feedback posts and are able to filter out or censor posts that does not comply with common sense rules.</p> 
Managers of Restaurant / Hotel can take notes (logs) about things happening on specific shift.     
System administrators can manage the accounts of managers and feedback moderators. 
They are able to initialize new Logbooks and Categories/Tags for them.

#### Technologies and other important information

The General technologies used in the developing the ManagerLogbook are these:
  - ASP.NET Core 2.2
  - ASP.NET Identity System with admin, manager and moderator area 
  - Entity Framework Core 2.2 to access database (used Code First approach)
  - Microsoft SQL Server for data storage and Entity 
  - Razor for all of the apps pages
  - AJAX for dynamic representation
  - Bootstrap 4
  - Used caching for XX data 
  - Created server-side paging and sorting for Notes of Managers??? 
  - Integration of application with a Continuous Integration server of Azure
  - Used GitHub advantages to work with branches durring development ot features
  - XX % Unit test code coverage of the business logic
  
#### Features
ManagerLogbook has following functionalities:
  - Login/Register functionalities 


#### Database Diagram

- DbDiagram20190617.jpg ![](DbDiagram20190617.jpg)

#### Repository [repo]
#### Azure [azure]

[repo]: https://github.com/stanildimitrov/managerlogbook
[azure]: https://managerlogbookweb20190615103454223.azurewebsites.net/?fbclid=IwAR2NK81wGZhW-qtoO74LpaSn7eoIISDMKlaWv_QXPqL_mXIdng6zEK6tvlI

#### Manager Logbook
[![Build status](https://dev.azure.com/stanildimitrov/Manager%20Logbook/_apis/build/status/ManagerLogbook%20CI)](https://dev.azure.com/stanildimitrov/Manager%20Logbook/_build/latest?definitionId=2)
