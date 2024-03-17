@echo off
cls
rmdir /S /Q Migrations
dotnet-ef migrations add CreationBDD
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet-ef migrations script