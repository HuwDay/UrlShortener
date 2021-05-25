
To Install:
 - Set up a database in SQL Server
 - Update the connection string in the Web.config file to point to the new database
 - Run the SQL script stored in /scripts/sql/Create_Tables.sql
 The web application should run up


 Relevant files are:
	- /App_Code/DataLayer.cs		This provides data access layer functions to move data between the web application and the database
	- /App_Code/ErrorHandler.cs		This class handles logging of errors to the database
	- /App_Code/Functions.cs		This contains a small number of static functions required to create / retrieve a short URL key

	- /Default.aspx (.vb)			The markup and code-behind files. This page handles creating / retrieving short URLs and showing them to the user.
	- /Global.asax.vb				The Application_BeginRequest event here checks for the use of a short URL key and, if detected, performs the required redirect.

	- /Scripts/UrlValidator.js		Contains javascript to validate supplied URLs client-side before firing the postback event.

Minor files:
	- Site.Master(.cs)				A stripped-down version of the default template site master. ASP's default Friendly URL module was removed to prevent potential conflicts.
	- /Content/Site.min.css			Basic styling / presentation for the Default.aspx page