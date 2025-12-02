using Microsoft.Extensions.Logging;

namespace DailyTests;
public abstract class BaseTest
{
    internal ILoggerFactory _loggerFactory = new LoggerFactory();

    public void SetWorkingDirectory(string path)
    {
        var day01Path = Path.Combine(TestSetup.RootPath ?? "", path);
        Directory.SetCurrentDirectory(
            Path.Combine(TestSetup.RootPath ?? "", path)
            );
    }
}
