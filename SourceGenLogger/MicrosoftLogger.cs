using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Serilog.Enrichers.GeneratedContext;
using Serilog.Events;

namespace SourceGenLogger;

[MicrosoftLoggerGenerate(8, "MethodContext")]
public static partial class MicrosoftLogger
{
    public static void TestLog(ILogger<MicrosFoot> logger)
    {
        logger.Information("Hello World! Logging is {Description}.", "fun"); // INF MicrosoftLogger.TestLog - Hello World! Logging is "fun".
        logger.Information("Hello World! Logging is."); // INF MicrosoftLogger.TestLog - Hello World! Logging is.
        logger.Information(new Exception("test"), "Hello World! Logging is {Description}.", "cool"); // INF MicrosoftLogger.TestLog - Hello World! Logging is "cool".
    }
}

public class MicrosFoot
{
    
}