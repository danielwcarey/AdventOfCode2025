using Microsoft.Extensions.DependencyInjection;

namespace DailyTests;

[TestClass]
public static class TestSetup
{
    public static ServiceProvider? ServiceProvider { get; private set; }
    public static string? RootPath { get; private set; }

    [AssemblyInitialize]
    public static void AssemblyInit(TestContext context)
    {
        var services = new ServiceCollection();

        services.AddLogging();

        // Register your dependencies:
        // services.AddTransient<IMyService, MyServiceImplementation>();
        // services.AddSingleton<ISomethingElse, SomethingElseImpl>();

        ServiceProvider = services.BuildServiceProvider();

        RootPath = context.TestRunDirectory?.Split("TestResults")[0];
    }
}
