#!/bin/bash

DayNumber=$1

dotnet new advent -o "src/Day$DayNumber"

mkdir -p "src\Data\Day$DayNumber"
touch "src\Data\Day$DayNumber\.gitkeep"
touch "src\Data\Day$DayNumber\Data1.txt"
touch "src\Data\Day$DayNumber\Data2.txt"

dotnet sln ./AdventOfCode2025.slnx add "./src/Day$DayNumber/Day$DayNumber.csproj"
