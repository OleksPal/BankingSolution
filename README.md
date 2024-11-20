# Banking Solution
[![.NET](https://github.com/OleksPal/BankingSolution/actions/workflows/dotnet.yml/badge.svg)](https://github.com/OleksPal/BankingSolution/actions/workflows/dotnet.yml)

Simple ASP.NET REST API for a banking application. The API allow users to perform basic banking operations such as creating accounts, making deposits, and transferring funds.

## Prerequirements

* Visual Studio
* .NET Core SDK
* SQL Server

## How To Run

* Open solution in Visual Studio
* Set api project as Startup Project and build the project.
* Run the application.

## Endpoints
* GET api/BankAccount/GetAll: List all acounts.
* GET api/BankAccount/GetBankAccount: Get account details by account number.
* POST api/BankAccount/CreateBankAccount: Create a new account with an initial balance.
* PUT api/BankAccount/Deposit: Deposit funds into an account.
* PUT api/BankAccount/Withdraw: Withdraw funds from an account.
* PUT api/BankAccount/Transfer: Transfer funds between two accounts.

## Built with 
* .NET
* ASP.NET API
* MS SQL
* EF Core
* xUnit