#!/bin/bash

# Get the day number from the first argument
DayNumber=$1

# Save the current working directory
pushd .

# Change to the target directory
cd "./src/Day$DayNumber"

# Run the dotnet command
dotnet run --project "Day$DayNumber.csproj"

# Restore the original working directory
popd
