using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DanielCarey.Shared;

public class StarHostedService(IServiceProvider services) : IHostedService
{
    private bool _isRunning;
    private readonly IStar _star1 = services.GetRequiredKeyedService<IStar>("Star1");
    private readonly IStar _star2 = services.GetRequiredKeyedService<IStar>("Star2");
    private readonly CompositeLogLevelController _logLevelController = services.GetRequiredService<CompositeLogLevelController>();

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _isRunning = true;
        await SelectAndRunAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _isRunning = false;
        return Task.CompletedTask;
    }

    // Temporarily change global minimum log level for the duration of the star run,
    // then restore the previous level even if the star throws.
    private async Task RunWithTemporaryLogLevelAsync(IStar star, LogLevel temporaryLevel)
    {
        LogLevel originalLevel = LogLevel.Information;
        try
        {
            _logLevelController.SetMinimumLevel(temporaryLevel);
            await star.RunAsync();
        }
        finally
        {
            _logLevelController.SetMinimumLevel(originalLevel);
        }
    }

    // Main service loop to select which star to compute.
    public async Task SelectAndRunAsync(CancellationToken cancellationToken)
    {
        bool running = _isRunning && !cancellationToken.IsCancellationRequested;

        // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
        while (running)
        {
            await Task.Yield();
            WriteLine();
            Write("""
                  Solve for star.
                  1] Star 1
                  2] Star 2

                  3] Star 1 with Debug Logging
                  4] Star 2 with Debug Logging

                  Any other key to exit.
                  >
                  """);
            var userInput = ReadKey();
            WriteLine();

            int.TryParse($"{userInput.KeyChar}", out int selection);

            switch (selection)
            {
                case 1:
                    await Task.Yield();
                    WriteLine($"Running {_star1.Name}");
                    await _star1.RunAsync();
                    break;
                case 2:
                    await Task.Yield();
                    WriteLine($"Running {_star2.Name}");
                    await _star2.RunAsync();
                    break;
                case 3:
                    await Task.Yield();
                    WriteLine($"Running {_star1.Name} with Debug Logging");
                    await RunWithTemporaryLogLevelAsync(_star1, LogLevel.Debug);
                    break;
                case 4:
                    await Task.Yield();
                    WriteLine($"Running {_star2.Name} with Debug Logging");
                    await RunWithTemporaryLogLevelAsync(_star2, LogLevel.Debug);
                    break;
                default:
                    _isRunning = false;
                    return;
            }
        }
    }
}

