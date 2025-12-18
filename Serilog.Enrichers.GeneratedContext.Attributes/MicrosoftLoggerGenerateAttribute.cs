using System;
using System.CodeDom.Compiler;

namespace Serilog.Enrichers.GeneratedContext;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
[GeneratedCode("Serilog.Enrichers.GeneratedContext", "1.0.0.0")]
public sealed class MicrosoftLoggerGenerateAttribute : Attribute
{
    public int GenericOverrideCount { get; }
    public string ContextName { get; }
    public string ContextSuffix { get; }

    public MicrosoftLoggerGenerateAttribute(int genericOverrideCount, string contextName = "MethodName", string contextSuffix = "")
    {
        GenericOverrideCount = Math.Max(0, genericOverrideCount);
        ContextName = contextName;
        ContextSuffix = contextSuffix;
    }
}