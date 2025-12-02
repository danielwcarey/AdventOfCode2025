using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DanielCarey.Shared;

public class StarHostedService(IServiceProvider services) : IHostedService
{
    private bool _isRunning;
    private readonly IStar _star1 = services.GetRequiredKeyedService<IStar>("Star1");
    private readonly IStar _star2 = services.GetRequiredKeyedService<IStar>("Star2");

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
                default:
                    _isRunning = false;
                    return;
            }
        }
    }
}

