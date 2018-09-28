# ConsoleRssChecker
A console program for parsing a RSS Feed and checking its last build date.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. For simplification purposes, serveral assumptions are made during creating the program:
1. The input will be in a CSV file name "companyRss.txt" and each line contains a company name and its RSS Feed url delimited by a comma. 
2. The ```<lastBuildDate>``` element in the XML file is used to check the last activity after the pull request (HTTP GET). This element is optional in the XML file. Another possible solution is that, checking the ```<pubDate>``` element for each ```<item>``` element and the latest pub date will be the last activity date.     

This program is created in Visual Studio 2017 and the target framework is .NET Core 2.1. The configuration for given inactive days can be set in the "appsettings.json" file.

Unit tests have been created for the Service module and can be run in the Test Explorer in Visual Studio.


