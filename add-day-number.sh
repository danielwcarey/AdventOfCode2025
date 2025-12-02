#!/bin/bash

DayNumber=$1

dotnet new advent -o "src/Day$DayNumber"

dotnet sln ./AdventOfCode2024.sln add "./src/Day$DayNumber/Day$DayNumber.csproj"
