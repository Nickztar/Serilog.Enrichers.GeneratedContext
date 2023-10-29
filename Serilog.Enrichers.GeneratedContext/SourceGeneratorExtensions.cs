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
    }
}
