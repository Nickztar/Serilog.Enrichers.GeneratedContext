using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Serilog.Enrichers.GeneratedContext
{
    [Generator]
    internal sealed class LoggerIncrementalSourceGenerator : IIncrementalGenerator
    {
        private const bool Debug = false;
        private const string Tab = "    ";
        private static readonly string NewLine = new StringBuilder().AppendLine().ToString();

        /*
         * 0: tab
         * 1: exception documentation comment (prefixed with newline or empty)
         * 2: propertyValue(s/0..n) documentation comment (prefixed with newline or empty)
         * 3: generic parameters (with brackets or empty)
         * 4: exception parameter (postfixed with a comma or empty)
         * 5: propertyValue(s/0..n) parameter(s) (prefixed with a comma or empty)
         * 6: exception argument (prefixed with a comma or empty)
         * 7: propertyValue(s/0..n) argument(s)
         * 8: LogEventLevel
         * 9: LogEventLevel argument (or empty)
         * 10: example code
         * 11: forwarded method
         */
        private const string LevelMethodTemplate = """
                                                   {0}/// <summary>
                                                   {0}/// Write a log event with the <see cref="LogEventLevel.{8}"/> level and associated exception.
                                                   {0}/// </summary>{1}
                                                   {0}/// <param name="messageTemplate">Message template describing the event.</param>{2}
                                                   {0}/// <example><code>
                                                   {0}/// {10}
                                                   {0}/// </code></example>
                                                   {0}[MessageTemplateFormatMethod("messageTemplate")]
                                                   {0}public static void {8}{3}({4}string messageTemplate{5}, Serilog.Enrichers.GeneratedContext.LogWithOptionalParameterList _ = default(Serilog.Enrichers.GeneratedContext.LogWithOptionalParameterList), [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int _sourceLineNumber = 0)
                                                   {0}{0}=> {11}({9}{6}, messageTemplate, {7});
                                                   """;

        private static readonly Dictionary<string, string> ExampleCodesWithoutException = new()
        {
            { "Verbose", "Log.Verbose(\"Staring into space, wondering if we're alone.\");" },
            { "Debug", "Log.Debug(\"Starting up at {StartedAt}.\", DateTime.Now);" },
            { "Information", "Log.Information(\"Processed {RecordCount} records in {TimeMS}.\", records.Length, sw.ElapsedMilliseconds);" },
            { "Warning", "Log.Warning(\"Skipped {SkipCount} records.\", skippedRecords.Length);" },
            { "Error", "Log.Error(\"Failed {ErrorCount} records.\", brokenRecords.Length);" },
            { "Fatal", "Log.Fatal(\"Process terminating.\");" },
        };

        private static readonly Dictionary<string, string> ExampleCodesWithException = new()
        {
            { "Verbose", "Log.Verbose(ex, \"Staring into space, wondering where this comet came from.\");" },
            { "Debug", "Log.Debug(ex, \"Swallowing a mundane exception.\");" },
            { "Information", "Log.Information(ex, \"Processed {RecordCount} records in {TimeMS}.\", records.Length, sw.ElapsedMilliseconds);" },
            { "Warning", "Log.Warning(ex, \"Skipped {SkipCount} records.\", skippedRecords.Length);" },
            { "Error", "Log.Error(ex, \"Failed {ErrorCount} records.\", brokenRecords.Length);" },
            { "Fatal", "Log.Fatal(ex, \"Process terminating.\");" },
        };

        private static readonly string ExceptionDocumentationComment = $"{NewLine}{Tab}/// <param name=\"exception\">Exception related to the event.</param>";
        private const string ExceptionParameter = "Exception? exception, ";
        private const string ExceptionArgument = ", exception";

        /// <summary>
        /// Forward write to internal Serilog write, but adding the context for the method
        /// </summary>
        private const string WriteMethod = """Serilog.Log.ForContext("{0}", $"{{Path.GetFileNameWithoutExtension(sourceFilePath)}}.{{memberName}}{1}").Write""";
        
        private static readonly string[] AllLogLevels =
        [
            "Verbose",
            "Debug",
            "Information",
            "Warning",
            "Error",
            "Fatal"
        ];
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var constants = context.SyntaxProvider
                .ForAttributeWithMetadataName(
                    "Serilog.Enrichers.GeneratedContext.LoggerGenerateAttribute",
                    predicate: (node, _) => node is ClassDeclarationSyntax,
                    transform: GetConfiguration);
            context.RegisterSourceOutput(constants, Build);
        }

        internal record LoggerData(string Namespace, string Modifiers, string Keyword, string Name, LoggerAttributeData Attribute);
        internal record LoggerAttributeData(int GenericOverrideCount, string ContextName, string? ContextSuffix);
        private static LoggerData? GetConfiguration(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
        {
        
#if DEBUG
            if (Debug && !Debugger.IsAttached) Debugger.Launch();
#endif
            cancellationToken.ThrowIfCancellationRequested();
            if (context.TargetSymbol is not INamedTypeSymbol typeSymbol) return null;
            var attribute = context.Attributes.FirstOrDefault(p => p.AttributeClass?.Name == "LoggerGenerateAttribute");
            return new LoggerData(
                typeSymbol.ContainingNamespace.ToDisplayString(), 
                typeSymbol.DeclaredAccessibility.ToString().ToLowerInvariant(), 
                typeSymbol.TypeKind.ToString().ToLowerInvariant(), 
                typeSymbol.Name, 
                new LoggerAttributeData(
                    attribute.RetrieveValue<int>("genericOverrideCount"),
                    attribute.RetrieveValue<string?>("contextName") ?? "MethodName",
                    attribute.RetrieveValue<string?>("contextSuffix") ?? " - "
                )
            );
        }
        private static void Build(SourceProductionContext context, LoggerData? data)
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            if (data is null) return;
            var sourceCode = SourceCodeBuilder.Build(ref data);
            context.AddSource($"{data.Name}.g.cs", SourceText.From(sourceCode, Encoding.UTF8));
        }

        private static class SourceCodeBuilder
        {
            public static string Build(ref LoggerData logData)
            {
                return $$"""
                         // <auto-generated>
                         // This code was auto generated by a source code generator.
                         // </auto-generated>
                         #nullable enable

                         using Serilog;
                         using Serilog.Events;
                         using Serilog.Core;
                         using System.Runtime.CompilerServices;

                         #pragma warning disable CS0618
                         #pragma warning disable CS0612
                         namespace {{logData.Namespace}} 
                         {
                             {{logData.Modifiers}} static partial {{logData.Keyword}} {{logData.Name}} 
                             {
                         {{BuildLogMethods(ref logData)}}
                             }
                         }
                         #pragma warning restore CS0618
                         #pragma warning restore CS0612
                         """;
            }

            private static string BuildLogMethods(ref LoggerData logData)
            {
                var builder = new StringBuilder();
                var writeMethod = string.Format(WriteMethod, logData.Attribute.ContextName, logData.Attribute.ContextSuffix);
                foreach (var logEvenLevel in AllLogLevels)
                {
                    // method without exception and without propertyValues
                    // replace here to fix issue with trailing comma.
                    builder.AppendFormat(LevelMethodTemplate.Replace(", {7}", "{7}"), Tab,
                        /* exception doccumentation comment */ "",
                        /* propertyValue(s/0..n) documentation comment */ "",
                        /* generic parameters */ "",
                        /* exception parameter */ "",
                        /* propertyValue(s/0..n) parameter(s) */ "",
                        /* exception argument */ "",
                        /* propertyValue(s/0..n) argument(s) */ "",
                        /* LogEventLevel */ logEvenLevel,
                        /* LogEventLevel argument */ $"LogEventLevel.{logEvenLevel}",
                        /* example code */ ExampleCodesWithoutException[logEvenLevel],
                        /* forwarded method */ writeMethod
                    ).AppendLine().AppendLine();

                    // method with exception and without propertyValues
                    // replace here to fix issue with trailing comma.
                    builder.AppendFormat(LevelMethodTemplate.Replace(", {7}", "{7}"), Tab,
                        /* exception doccumentation comment */ ExceptionDocumentationComment,
                        /* propertyValue(s/0..n) documentation comment */ "",
                        /* generic parameters */ "",
                        /* exception parameter */ ExceptionParameter,
                        /* propertyValue(s/0..n) parameter(s) */ "",
                        /* exception argument */ ExceptionArgument,
                        /* propertyValue(s/0..n) argument(s) */ "",
                        /* LogEventLevel */ logEvenLevel,
                        /* LogEventLevel argument */ $"LogEventLevel.{logEvenLevel}",
                        /* example code */ ExampleCodesWithException[logEvenLevel],
                        /* forwarded method */ writeMethod
                    ).AppendLine().AppendLine();

                    // generic methods with and without exception, and with propertyValue(s)
                    if (logData.Attribute.GenericOverrideCount == 0) continue;
                    
                    var propValueComments = new StringBuilder().AppendLine().Append(Tab);
                    var genericParams = new StringBuilder().Append('<');
                    var propValueParams = new StringBuilder().Append(", ");
                    var propValueForwardArgs = new StringBuilder();

                    for (var i = 0; i < logData.Attribute.GenericOverrideCount; i++)
                    {
                        // skip "0" when we only have 1 propertyValue
                        var istr = i == 0 ? "" : i.ToString();

                        if (i == 1)
                        {
                            // return the "0" we skipped above
                            propValueComments.Replace("propertyValue", "propertyValue0");
                            genericParams.Append(0);
                            propValueParams.Replace("T propertyValue", "T0 propertyValue0");
                            propValueForwardArgs.Append(0);
                        }

                        if (i != 0)
                        {
                            propValueComments.AppendLine().Append(Tab);
                            genericParams.Append(", ");
                            propValueParams.Append(", ");
                            propValueForwardArgs.Append(", ");
                        }

                        propValueComments.Append("/// <param name=\"propertyValue").Append(istr)
                            .Append("\">Object positionally formatted into the message template.</param>");
                        genericParams.Append('T').Append(istr).Append('>');
                        propValueParams.Append('T').Append(istr).Append(" propertyValue").Append(istr);
                        propValueForwardArgs.Append("propertyValue").Append(istr);

                        // method without exception and with generic propertyValues
                        builder.AppendFormat(LevelMethodTemplate, Tab,
                            /* exception doccumentation comment */ "",
                            /* propertyValue(s/0..n) documentation comment */ propValueComments,
                            /* generic parameters */ genericParams,
                            /* exception parameter */ "",
                            /* propertyValue(s/0..n) parameter(s) */ propValueParams,
                            /* exception argument */ "",
                            /* propertyValue(s/0..n) argument(s) */ propValueForwardArgs,
                            /* LogEventLevel */ logEvenLevel,
                            /* LogEventLevel argument */ $"LogEventLevel.{logEvenLevel}",
                            /* example code */ ExampleCodesWithoutException[logEvenLevel],
                            /* forwarded method */ writeMethod
                        ).AppendLine().AppendLine();

                        // method with exception and with generic propertyValues
                        builder.AppendFormat(LevelMethodTemplate, Tab,
                            /* exception doccumentation comment */ ExceptionDocumentationComment,
                            /* propertyValue(s/0..n) documentation comment */ propValueComments,
                            /* generic parameters */ genericParams,
                            /* exception parameter */ ExceptionParameter,
                            /* propertyValue(s/0..n) parameter(s) */ propValueParams,
                            /* exception argument */ ExceptionArgument,
                            /* propertyValue(s/0..n) argument(s) */ propValueForwardArgs,
                            /* LogEventLevel */ logEvenLevel,
                            /* LogEventLevel argument */ $"LogEventLevel.{logEvenLevel}",
                            /* example code */ ExampleCodesWithException[logEvenLevel],
                            /* forwarded method */ writeMethod
                        ).AppendLine().AppendLine();

                        genericParams.Length--; // undo last .Append('>')
                    }
                }

                builder.Length -= NewLine.Length * 2; // undo last 2 .AppendLine()
                return builder.ToString();
            }
        }
    }
}