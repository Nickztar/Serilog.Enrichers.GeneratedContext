using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Serilog.Enrichers.GeneratedContext
{
    /// <summary>
    /// Extends <see cref="SyntaxNode"/> used in source generators with additional methods.
    /// </summary>
    public static class SourceGeneratorExtensions
    {
        /// <summary>
        /// Returns the first ancestor of type <see cref="{TAncestor}"/> of a node, or a default value if such ancestor can't be found.
        /// </summary>
        /// <typeparam name="TAncestor"> The type of the ancestor.</typeparam>
        /// <param name="node">The node.</param>
        /// <param name="@default">The default value to return if no ancestor of type <see cref="{TAncestor}"/> is found.</param>
        /// <returns><paramref name="@default"/> if <paramref name="node"/> is null or if no ancestor of type <see cref="{TAncestor}"/>
        /// is found; otherwise, the first ancestor or <paramref name="node"/> that is of type <see cref="{TAncestor}"/>.</returns>
        public static TAncestor? FirstAncestorOrDefault<TAncestor>(this SyntaxNode? node, TAncestor? @default = default) where TAncestor : class
        {
            if (node == null) return @default;

            var parent = node.Parent;

            while (parent != null)
            {
                if (parent is TAncestor Parent) return Parent;

                parent = parent.Parent;
            }

            return @default;
        }
        
        public static T? RetrieveValue<T>(this AttributeData? attribute, string key)
        {
            return attribute.TryGetValue<T>(key, out var value) ? value : default;
        }
        
        public static bool TryGetValue<T>(this AttributeData? attribute, string key, out T? value)
        {
            value = default;

            if (attribute is null)
                return false;

            var named = attribute.NamedArguments.FirstOrDefault(p => p.Key == key);
            if (named.Key == key && TryRead<T>(named.Value, out value))
                return true;

            var ctor = attribute.AttributeConstructor;
            if (ctor is null) return false;
            for (var i = 0; i < ctor.Parameters.Length; i++)
            {
                if (ctor.Parameters[i].Name == key &&
                    i < attribute.ConstructorArguments.Length &&
                    TryRead<T>(attribute.ConstructorArguments[i], out value))
                {
                    return true;
                }
            }

            return false;
        }
        
        private static bool TryRead<T>(TypedConstant constant, out T? value)
        {
            value = default;

            if (constant.IsNull)
                return true;

            if (typeof(T).IsEnum && constant.Value is not null)
            {
                value = (T)Enum.ToObject(typeof(T), constant.Value);
                return true;
            }

            if (constant.Value is not T t) return false;
            value = t;
            return true;

        }
    }
}

 namespace System.Runtime.CompilerServices
 {
     internal static class IsExternalInit {}
 }
