using System;
using System.Runtime.Serialization;

namespace Serilog.Enrichers.GeneratedContext
{
    [Serializable]
    public sealed class SourceGeneratorException : Exception
    {
        public SourceGeneratorException() : base()
        {
        }

        public SourceGeneratorException(string message) : base(message)
        {
        }

        public SourceGeneratorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        private SourceGeneratorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
