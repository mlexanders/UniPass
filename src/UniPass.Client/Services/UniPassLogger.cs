using Microsoft.Extensions.Logging;
using UniPass.Infrastructure.Services;

namespace UniPass.Client.Services;

public class UniPassClientLogger<T> : UniPassBaseLogger<T> where T : class
{
    public void Log(Exception e)
    {
        Console.WriteLine("---");
        Console.WriteLine(typeof(T).Name);
        Console.WriteLine(e.Message);
        Console.WriteLine(e.StackTrace);
        Console.WriteLine("---");
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter)
    {
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        return new NoopDisposable();
    }

    private class NoopDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}