using DanielCarey.Day01;

using Microsoft.Extensions.Logging;

namespace DailyTests;

[TestClass]
public sealed class Day01Tests : BaseTest
{
    private readonly string _star1DataPath = Path.Combine(TestSetup.RootPath, @"src\Data\Day01\Data1.txt");
    private readonly string _star2DataPath = Path.Combine(TestSetup.RootPath, @"src\Data\Day01\Data2.txt");

    [TestMethod]
    public async Task Star1Test()
    {
        var logger = _loggerFactory.CreateLogger<Star1>();

        var star = new Star1(logger, _star1DataPath);

        var result = await star.RunAsync();

        Assert.IsTrue(result == 1150);
    }

    [TestMethod]
    public async Task Star2Test()
    {
        var logger = _loggerFactory.CreateLogger<Star2>();

        var star = new Star2(logger, _star2DataPath);

        var result = await star.RunAsync();

        // Too Low: 6698
        // Too High: 6773
        // 6738
        Assert.IsTrue(result == 6738);
    }
}

