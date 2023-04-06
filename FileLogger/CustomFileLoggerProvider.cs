using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FileLogger
{
    /// <summary>
    /// This class represents a FileLoggerProvider that will allow
    /// Services to provide a CustomFileLogger for Dependency Injection use.
    /// </summary>
    public class CustomFileLoggerProvider : ILoggerProvider
    {
        /// <summary>
        /// Creates a new CustomeFileLogger object
        /// </summary>
        /// <param name="categoryName">The logging message</param>
        /// <returns>The CustomeFileLogger object</returns>
        public ILogger CreateLogger(string categoryName)
        {         
            return new CustomFileLogger(categoryName, "Logging");
        }

        //No need to implement if we use appendalltext
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
