param (
    [string]$DayNumber
)
# Save the current working directory
Push-Location

# Change to the target directory
Set-Location ".\src\Day$DayNumber"

# Run the dotnet command
dotnet run --project "Day$DayNumber.csproj"

# Restore the original working directory
Pop-Location

