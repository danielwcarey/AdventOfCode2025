using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DanielCarey.Shared;
public static class Program
{
    public static async Task RunAsync<TStar1, TStar2>()
        where TStar1 : class, IStar
        where TStar2 : class, IStar
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Services
            .AddLogging(config =>
            {
                config.AddConsole();
                config.AddDebug();
                
            });

        builder.Services.AddHostedService<StarHostedService>();

        builder.Services.AddKeyedTransient<IStar, TStar1>("Star1");
        builder.Services.AddKeyedTransient<IStar, TStar2>("Star2");

        using var app = builder.Build();

        await app.StartAsync();
    }
}
