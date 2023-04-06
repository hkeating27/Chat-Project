using Microsoft.Extensions.Logging;

namespace FileLogger
{
    /// <summary>
    /// This class represents the logger that will be used throughout the
    /// program via Dependency Injection.
    /// Written By: Nathaniel Taylor and Hunter Keating
    /// Debugged By: Hunter Keating and Nathaniel Taylor
    /// </summary>
    public class CustomFileLogger : ILogger
    {
        //Fields
        private readonly string categoryName;
        private string fileName;

        /// <summary>
        /// Creates a new CustomFileLogger object and initializes the private fields
        /// </summary>
        /// <param name="categoryName">The categoryName value for the private field</param>
        /// <param name="fileName">The fileName value for the private field</param>
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

        /// <summary>
        /// Creates a new file and writes logging text to that file
        /// </summary>
        /// <typeparam name="TState">The state of the logger</typeparam>
        /// <param name="logLevel">Defines logging severity</param>
        /// <param name="eventId">Identifies a logging event</param>
        /// <param name="state"></param>
        /// <param name="exception">Represents errors that occur</param>
        /// <param name="formatter">The formatter of the file</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            this.fileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                   + Path.DirectorySeparatorChar
                   + $"CS3500-{categoryName}.log";
            File.AppendAllText(fileName, formatter(state, exception));
        }
    }
}