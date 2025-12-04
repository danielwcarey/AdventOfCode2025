using DanielCarey.Day02;

using Microsoft.Extensions.Logging;

namespace DailyTests;

[TestClass]
public sealed class Day02Tests : BaseTest
{
    private readonly string _star1DataPath = Path.Combine(TestSetup.RootPath, @"src\Data\Day02\Data1.txt");
    private readonly string _star2DataPath = Path.Combine(TestSetup.RootPath, @"src\Data\Day02\Data2.txt");

    [TestMethod]
    public async Task Star1Test()
    {
        var logger = _loggerFactory.CreateLogger<Star1>();

        var star = new Star1(logger, _star1DataPath);

        var result = await star.RunAsync();

        Assert.IsTrue(result == 31000881061);
    }

    [TestMethod]
    public async Task Star2Test()
    {
        var logger = _loggerFactory.CreateLogger<Star2>();

        var star = new Star2(logger, _star2DataPath);

        var result = await star.RunAsync();

        // Too Low: 59628886
        // Too Low: 94495549
        Assert.IsTrue(result == 46769308485);
    }
}

