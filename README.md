# ThoughtWorksTrains

### Design

I broke the solution down to 2 classes. A dictionary for <”{{town id}}”, ”{{Town}}” > and a Town class which had an id and a dictionary <”{{destination town id}}”, {{distance}}> from there I could traverse the townMap dictionary which included all the towns and each town had a list of possible routes.

I started with GetRouteDistance and building the test GetRouteDistance_CalculateRouteAB because I knew this should return 5 and it was a good starting point. 

### Tech

ThoughtWorks Trains requires a few open source frameworks to run properly:

* [ASP.Net Core 2.1] - web app
* [Microsoft.Extensions.DependencyInjection] - ASP.Net Core default DI
* [nUnit] - Unit testing framework
* [Moq] - Mocking framework
* [Swagger] - API documentation
* [Gulp] - To build the scss
* [Razor] - ASP.Net html rendering framework
* [Twitter Bootstrap] - great UI boilerplate for modern web apps
* [jQuery] - JS framework

Some technologies I would have liked to implement but couldn’t due to time constraints:

* [MVM Framework] - I chose not to implement an MVM framework just out of time requirements, I started implementing Angular 7 with TypeScript 3 through a WebPack but it was taking to much time and the local dev environment setup was a lot harder
* [Webpack] - To handle all the front end components
* [Security] - Protect the API call with a client_id and client_secret which could be encrypted into a cookie on the user's browser
* [Feature Toggles] - To help support having all commits are releasable

### Prerequisite

*[Visual Studio 2017]*

*[Node]* - https://nodejs.org/en/download/ Verify that you are running at least Node.js version 8.x or greater and npm version 5.x or greater by running node -v and npm -v in a terminal/console window. Older versions produce errors, but newer versions are fine.

### Installation


1. Open the solution in Visual Studios
2. Right click on the ThoughtWorksTrains solution in the Solution Explorer and click ‘Set Startup Projects...’
3. Set ThoughtWorksTrains.API and ThoughtWorksTrains.Web both to ‘Start’  
4. Build the solution so NuGet/NPM can download the required packages
5. Press F5 and run the project using visual studios debugger and IIS Express
6. 2 browsers should open, 1 is the swagger docs, the other is the front end