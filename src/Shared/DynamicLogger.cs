using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Options;

namespace DanielCarey.Shared;

public static class DynamicLoggerExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDynamicLogging(Action<ILoggingBuilder>? config = null)
        {
            DynamicLogLevelProvider? consoleWrapper = null;
            DynamicLogLevelProvider? debugWrapper = null;

            config ??= (internalConfig) => {
                internalConfig.SetMinimumLevel(LogLevel.Debug);
                internalConfig.ClearProviders();
            };

            services.AddLogging(config);

            services.AddSingleton<ILoggerProvider>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptionsMonitor<ConsoleLoggerOptions>>();
                ConsoleLoggerProvider consoleProvider = new ConsoleLoggerProvider(options);
                consoleWrapper = new DynamicLogLevelProvider(consoleProvider);
                return consoleWrapper;
            });

            services.AddSingleton<ILoggerProvider>(serviceProvider =>
            {
                DebugLoggerProvider debugProvider = new DebugLoggerProvider();
                debugWrapper = new DynamicLogLevelProvider(debugProvider);
                return debugWrapper;
            });

            services.AddSingleton(serviceProvider =>
                new CompositeLogLevelController(consoleWrapper!, debugWrapper!));

            return services;
        }
    }
}

public class CompositeLogLevelController(params DynamicLogLevelProvider[] providers)
{
    public void SetMinimumLevel(LogLevel level)
    {
        foreach (DynamicLogLevelProvider provider in providers)
        {
            provider.SetMinimumLevel(level);
        }
    }
}

public class DynamicLogLevelProvider : ILoggerProvider
{
    private readonly ILoggerProvider _innerProvider;
    private LogLevel _currentMinLevel = LogLevel.Information;
    private readonly object _lock = new object();

    public DynamicLogLevelProvider(ILoggerProvider innerProvider)
    {
        _innerProvider = innerProvider;
    }

    public void SetMinimumLevel(LogLevel level)
    {
        lock (_lock)
        {
            _currentMinLevel = level;
        }
    }

    public ILogger CreateLogger(string categoryName)
    {
        ILogger innerLogger = _innerProvider.CreateLogger(categoryName);
        return new DynamicLogger(innerLogger, this);
    }

    public void Dispose()
    {
        _innerProvider?.Dispose();
    }

    private class DynamicLogger : ILogger
    {
        private readonly ILogger _innerLogger;
        private readonly DynamicLogLevelProvider _provider;

        public DynamicLogger(ILogger innerLogger, DynamicLogLevelProvider provider)
        {
            _innerLogger = innerLogger;
            _provider = provider;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return _innerLogger.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            lock (_provider._lock)
            {
                return logLevel >= _provider._currentMinLevel && _innerLogger.IsEnabled(logLevel);
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                _innerLogger.Log(logLevel, eventId, state, exception, formatter);
            }
        }
    }
}
