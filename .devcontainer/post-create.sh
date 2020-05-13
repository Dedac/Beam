#!/bin/bash
/opt/mssql-tools/bin/sqlcmd -l 60 -S sql -U SA -P "@#^!fcIen&*asd" -Q "CREATE DATABASE BEAM"
dotnet restore 
dotnet tool restore
dotnet ef database update --project ./Beam.Server