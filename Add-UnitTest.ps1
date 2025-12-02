param (
    [string]$DayNumber
)

$text = @"
using DanielCarey.Day$DayNumber;

using Microsoft.Extensions.Logging;

namespace DailyTests;

[TestClass]
public sealed class Day${DayNumber}Tests : BaseTest
{
    private readonly string _star1DataPath = Path.Combine(TestSetup.RootPath, @"src\Day$DayNumber\Data1.txt");
    private readonly string _star2DataPath = Path.Combine(TestSetup.RootPath, @"src\Day$DayNumber\Data2.txt");

    [TestMethod]
    public async Task Star1Test()
    {
        var logger = _loggerFactory.CreateLogger<Star1>();

        var star = new Star1(logger, _star1DataPath);

        var result = await star.RunAsync();

        Assert.IsTrue(result == -1);
    }

    [TestMethod]
    public async Task Star2Test()
    {
        var logger = _loggerFactory.CreateLogger<Star2>();

        var star = new Star2(logger, _star2DataPath);

        var result = await star.RunAsync();

        Assert.IsTrue(result == -1);
    }
}

"@

$text | Out-File -FilePath "src\DailyTests\Day${DayNumber}Tests.cs"

dotnet add src\DailyTests\DailyTests.csproj reference src\Day$DayNumber\Day$DayNumber.csproj
