using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FileLogger
{
    public class CustomFileLoggerProvider : ILoggerProvider
    {

        public ILogger CreateLogger(string categoryName)
        {         
            return new CustomFileLogger(categoryName, "logging");
        }

        //No need to implement if we use appendalltext
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
