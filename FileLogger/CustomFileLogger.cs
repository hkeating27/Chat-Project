using Microsoft.Extensions.Logging;

namespace FileLogger
{
    public class CustomFileLogger : ILogger
    {
        private readonly string categoryName;
        private string fileName;

        public CustomFileLogger(string categoryName, string fileName)
        {
            this.categoryName = categoryName;
            this.fileName     = fileName;
        }

        //Skipped as described
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }

        //Skipped as described
        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            this.fileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                   + Path.DirectorySeparatorChar
                   + $"CS3500-{categoryName}.log";
            File.AppendAllText(fileName, formatter(state, exception));
        }
    }
}