using System.Runtime.CompilerServices;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers.GeneratedContext;
using Serilog.Events;

namespace SourceGenLogger;

//TODO: This might be improved by using, but unsure how:
//https://www.milanjovanovic.tech/blog/structured-logging-in-asp-net-core-with-serilog
//https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.defaultinterpolatedstringhandler?view=net-7.0
[LoggerGenerate(
    genericOverrideCount: 4,
    contextName: "MethodContext",
    contextSuffix: " - "
)]
public static partial class Logger
{
    public static void Configure()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System.Net.Http", LogEventLevel.Warning)
            .MinimumLevel.Override("Hangfire", LogEventLevel.Warning)
#if DEBUG
            .WriteTo.Console(outputTemplate: "{Level:u3} {MethodContext} - {Message}{NewLine}{Exception}")
#endif
#if !DEBUG
                .WriteTo.Console(outputTemplate: "{Level:u3} {Message}{NewLine}{Exception}", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
#endif
            .CreateLogger();
    }

    // private static string FormatForException(this string message, Exception? ex)
    // {
    //     Serilog.Log.ForContedxt()
    // }
    //
    //
    public static void TestLog()
    {
        Debug("tst {Unknown} {Unknown} {Unknown}", 1, 2, 4);
        Debug("tst {Unknown} {Unknown} {Unknown} {Unknown}", 1, 2, 4, 4);
        Debug("test");
        Serilog.Log.Write(LogEventLevel.Debug, "test", null);
    }
    //
    // public static void Verbose(
    //     string message,
    //     [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //     Serilog.Log.Verbose(
    //         message
    //             .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // public static void Verbose(
    //     string message,
    //     Exception ex,
    //     [CallerMemberName] string memberName = "",
    //     [CallerFilePath] string sourceFilePath = "",
    //     [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //     Serilog.Log.Verbose(
    //         message
    //             .FormatForException(ex)
    //             .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // public static void Verbose(
    //     Exception ex,
    //     [CallerMemberName] string memberName = "",
    //     [CallerFilePath] string sourceFilePath = "",
    //     [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //
    //     Serilog.Log.Verbose(
    //         (ex != null ? ex.ToString() : "")
    //         .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // public static void Debug(
    //     string message,
    //     [CallerMemberName] string memberName = "",
    //     [CallerFilePath] string sourceFilePath = "",
    //     [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //     Serilog.Log.Debug(
    //         message
    //             .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // public static void Debug(
    //     string message,
    //     Exception ex,
    //     [CallerMemberName] string memberName = "",
    //     [CallerFilePath] string sourceFilePath = "",
    //     [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //     Serilog.Log.Debug(
    //         message
    //             .FormatForException(ex)
    //             .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // public static void Debug(
    //     Exception ex,
    //     [CallerMemberName] string memberName = "",
    //     [CallerFilePath] string sourceFilePath = "",
    //     [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //
    //     Serilog.Log.Debug(
    //         (ex != null ? ex.ToString() : "")
    //         .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // public static void Info(
    //     string message,
    //     [CallerMemberName] string memberName = "",
    //     [CallerFilePath] string sourceFilePath = "",
    //     [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //     Serilog.Log.Information(
    //         message
    //             .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // public static void Info(
    //     string message,
    //     Exception ex,
    //     [CallerMemberName] string memberName = "",
    //     [CallerFilePath] string sourceFilePath = "",
    //     [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //
    //     Serilog.Log.Information(
    //         message
    //             .FormatForException(ex)
    //             .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // public static void Info(
    //     Exception ex,
    //     [CallerMemberName] string memberName = "",
    //     [CallerFilePath] string sourceFilePath = "",
    //     [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //
    //     Serilog.Log.Information(
    //         (ex != null ? ex.ToString() : "")
    //         .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // public static void Warn(string message,
    //     [CallerMemberName] string memberName = "",
    //     [CallerFilePath] string sourceFilePath = "",
    //     [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //
    //     Serilog.Log.Warning(
    //         message
    //             .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // public static void Warn(
    //     string message,
    //     Exception ex,
    //     [CallerMemberName] string memberName = "",
    //     [CallerFilePath] string sourceFilePath = "",
    //     [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //
    //     Serilog.Log.Warning(
    //         message
    //             .FormatForException(ex)
    //             .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // public static void Warn(
    //     Exception ex,
    //     [CallerMemberName] string memberName = "",
    //     [CallerFilePath] string sourceFilePath = "",
    //     [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //
    //     Serilog.Log.Warning(
    //         (ex != null ? ex.ToString() : "")
    //         .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // public static void Error(
    //     string message,
    //     [CallerMemberName] string memberName = "",
    //     [CallerFilePath] string sourceFilePath = "",
    //     [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //
    //     Serilog.Log.Error(
    //         message
    //             .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // public static void Error(
    //     string message,
    //     Exception ex,
    //     [CallerMemberName] string memberName = "",
    //     [CallerFilePath] string sourceFilePath = "",
    //     [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //
    //     Serilog.Log.Error(
    //         message
    //             .FormatForException(ex)
    //             .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // public static void Error(
    //     Exception ex,
    //     [CallerMemberName] string memberName = "",
    //     [CallerFilePath] string sourceFilePath = "",
    //     [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //
    //     Serilog.Log.Error(
    //         (ex != null ? ex.ToString() : "")
    //         .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // public static void Fatal(
    //     string message,
    //     [CallerMemberName] string memberName = "",
    //     [CallerFilePath] string sourceFilePath = "",
    //     [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //     FatalAction();
    //
    //     Serilog.Log.Error(
    //         message
    //             .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // public static void Fatal(
    //     string message,
    //     Exception ex,
    //     [CallerMemberName] string memberName = "",
    //     [CallerFilePath] string sourceFilePath = "",
    //     [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //     FatalAction();
    //
    //     Serilog.Log.Error(
    //         message
    //             .FormatForException(ex)
    //             .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // public static void Fatal(
    //     Exception ex,
    //     [CallerMemberName] string memberName = "",
    //     [CallerFilePath] string sourceFilePath = "",
    //     [CallerLineNumber] int sourceLineNumber = 0)
    // {
    //     FatalAction();
    //
    //     Serilog.Log.Error(
    //         ex.Message
    //             .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
    //     );
    // }
    //
    // private static void FatalAction()
    // {
    //     Environment.ExitCode = -1;
    // }
}