MeDirect_CurrencyExchangeAPI

Project Structure

Project is structred in small code files. Below are structure

Controllers:- hold all controllers
Handler:- Handles automappting,
Model:- All model classes for project
Services:- All business logic classes
Program.cs:- All startup code
CustomerMiddleware:- To handle exception

Comments
I tried to follow Dependency injection most of time to make sure we can reuse all components anywhere. This API is responsible to send the current exchange rates to the customers,once customer get the exchange rate then they can carry out conversion transaction

Improvements
Idea was to create initial structure to make our first api ready by following good practices. Some improvement like segregating core logic and making as standard project/nuget/library will help going forward to everyone, So next time we just need to install these library and follow same approach to boost up our development progress.
