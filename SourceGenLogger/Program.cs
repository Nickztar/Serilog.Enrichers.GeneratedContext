// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Logging;
using Serilog;
using SourceGenLogger;

Console.WriteLine("Hello, World!");
Logger.Configure();
Logger.Debug("tst"); // DBG Program.<Main>$ -  - tst
Logger.TestLog();

using var factory = LoggerFactory.Create(builder => builder.AddSerilog());
var logger = factory.CreateLogger<Program>();
logger.Information("Hello World! Logging is {Description}.", "fun"); // INF Program.<Main>$ - Hello World! Logging is "fun".
logger.Information("Hello World! Logging is."); // INF Program.<Main>$ - Hello World! Logging is.
logger.Information(new Exception("test"), "Hello World! Logging is {Description}.", "cool"); 
// INF Program.<Main>$ - Hello World! Logging is "cool".
// System.Exception: test

MicrosoftLogger.TestLog(factory.CreateLogger<MicrosFoot>());
