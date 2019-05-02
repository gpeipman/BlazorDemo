# BlazorDemo

This is my simple Blazor application that demonstrates how to build SPA on Blazor and how to communicate with ASP.NET Core backend. 
Demo application is simple books database. 

Solution contains:

* Sample database BACPAC file (can be imported to MSSQL using SSMS)
* Client application with Blazor UI
* Basic select and CRUD operations are implemented in UI and in back-end
* Displaying of delete confirmation dialog and deleting of books
* Fully functioning add/edit form
* Pager component and support for data paging
* Dependency injection with custom service classes
* Protecting Blazor application and Azure Functions based back-end using Azure AD

## Azure AD example

For Azure AD there are two project in solution:

* BlazorDemo.AdalClient - Blazor web application that supports Azure AD
* BlazorDemo.AzureFunctionsBackend - Azure Functions project with all functions that form back-end for Blazor application

On Azure the following services are needed:

* Azure AD - free one is okay
* Azure SQL - instance with minimal size is okay
* Azure Search - free tier is okay
* App regitration on Azure AD - Web/Web API type of application
* Azure Functions - minimal App Service where functions run is okay
* Azure Storage GPv2 with static websites enabled (optional)

Configuration in code files:

* BlazorDemo.AdalClient project wwwroot/js/app.js - Azure AD tenant ID and application ID
* BlazorDemo.AdalClient project BooksAzureFunctionsClient.cs - Azure Functions host and Azure AD application ID
* BlazorDemo.AzureFunctionsBackend project AzureSearchClient - Azure Search service and index name, access key

To use search you have to comment in calls to search service in BooksAzureFunctionsClient.cs (BlazorDemo.AdalClient project)

BACPAC for SQL Server is in External Files folder. After creating database on SQL Azure it is possible to import it as a data-tier application. Same way it is possible to import it to SQL Server LocalDb used by BlazorDemo.Client project.

More information is available in my blog post [Azure AD authentication in Blazor using ADAL.js](https://gunnarpeipman.com/aspnet/blazor-azure-ad-adal/).

## Tools

As of 2018-05-09 the following tooling is needed to build Blazor applications:

* [.NET Core 3.0 Preview 4 SDK](https://www.microsoft.com/net/download/thank-you/dotnet-sdk-2.1.301-windows-x86-installer)
* [Visual Studio 2019](https://www.visualstudio.com/vs/)
* ASP.NET and web development workload for Visual Studio (activate during VS installation)
* [ASP.NET Core Blazor Language Services extension](https://go.microsoft.com/fwlink/?linkid=870389)
* [Getting started with Blazor](http://gunnarpeipman.com/2018/04/blazor-preview/)

## Your opinion matters

If you tried out this solution and you understand how Blazor works then please find some moments to 
take [brief survey by Microsoft](https://go.microsoft.com/fwlink/?linkid=873042) helping to make Blazor 
even better.

## References

* [Azure AD authentication in Blazor using ADAL.js](https://gunnarpeipman.com/aspnet/blazor-azure-ad-adal/)
* [Hosting Azure Functions backed Blazor application on Azure Storage static website](https://gunnarpeipman.com/azure/blazor-azure-function-static-website/)
* [Building confirm delete dialog on Blazor](https://gunnarpeipman.com/aspnet/blazor-confirm-delete-dialog/)
* [Dependency injection in Blazor](https://gunnarpeipman.com/aspnet/blazor-dependency-injection/)
* [Building Blazor pager component](https://gunnarpeipman.com/aspnet/blazor-pager-component/)
* [Separating code and presentation of Blazor pages](https://gunnarpeipman.com/aspnet/blazor-code-behind/)
* [WebAssembly apps with Blazor](https://gunnarpeipman.com/aspnet/blazor-preview/)