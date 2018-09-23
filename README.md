Code demonstration of React/Redux Front end w/ .netCore 2.0 Serverless web api backend. 

This is a .net core 2.0 WEB API that serves as the backend.


botna.github.io/BankServerUI

please do not goto the website on mobile, it will hurt your brain and my pride.


TO RUN LOCALLY:
Clone Repo
install .netcore 2.0 SDK (https://www.microsoft.com/net/download/dotnet-core/2.0)
nuget restore

Launch locally, and either utilize the postman collection found in the repo or naviate to localhost:4697/swagger/index.html for execution instructions (new version of swagger is confusing..clicky the 'try it out' under each tab.

Uses a singleton via DI for the caching of the data, so each restart of the application will require users to be re-created.  Uses a non .netCore nuget package for JWT creation, so if that causes any difficulty, please refer to the
serverless Backend repository found here.https://github.com/Botna/ServerlessBankServer

I did a controller and a provider level set of unit tests to show the different concepts for those, most importantly mocking.
