using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Serilog.Enrichers.GeneratedContext
{
    internal sealed class ClassOrInterfaceAttributeSyntaxReceiver : ISyntaxReceiver
    {
        private const string attributePostfix = nameof(Attribute);

        private readonly string attributeIdentifierName;
        private readonly string attributeIdentifierNameShort;

        public ClassOrInterfaceAttributeSyntaxReceiver(string attributeIdentifierName)
        {
            if (attributeIdentifierName.EndsWith(attributePostfix))
            {
                this.attributeIdentifierName = attributeIdentifierName;
                attributeIdentifierNameShort = attributeIdentifierName.Substring(0, attributeIdentifierName.Length - attributePostfix.Length);
            }
            else
            {
                this.attributeIdentifierName = attributeIdentifierName + attributePostfix;
                attributeIdentifierNameShort = attributeIdentifierName;
            }
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is not AttributeSyntax attribute) return;

            var identifierName = (attribute.Name as IdentifierNameSyntax)?.Identifier.Text;
            if (identifierName != attributeIdentifierName && identifierName != attributeIdentifierNameShort) return;

            var @class = attribute.FirstAncestorOrDefault<ClassDeclarationSyntax>();
            var @interface = attribute.FirstAncestorOrDefault<InterfaceDeclarationSyntax>();
            if (@class == null && @interface == null) throw new SourceGeneratorException("Couldn't get the class/interface marked with the attribute");

            var @namespace = @class != null ? @class.FirstAncestorOrDefault<NamespaceDeclarationSyntax>() : @interface!.FirstAncestorOrDefault<NamespaceDeclarationSyntax>();
            var fileScopedNamespace = @class != null ? @class.FirstAncestorOrDefault<FileScopedNamespaceDeclarationSyntax>() : @interface!.FirstAncestorOrDefault<FileScopedNamespaceDeclarationSyntax>();
            if (@namespace == null && fileScopedNamespace == null) throw new SourceGeneratorException("Couldn't get the namespace enclosing the marked class/interface");

            Captures.Add(new(@namespace, fileScopedNamespace, attribute, @class, @interface));
        }

        public readonly struct Capture
        {
            public NamespaceDeclarationSyntax? NamespaceDeclaration { get; }
            public FileScopedNamespaceDeclarationSyntax? FileScopedNamespaceDeclaration { get; }
            public AttributeSyntax AttributeDeclaration { get; }
            public ClassDeclarationSyntax? ClassDeclaration { get; }
            public InterfaceDeclarationSyntax? InterfaceDeclaration { get; }

            public Capture(NamespaceDeclarationSyntax? @namespace, FileScopedNamespaceDeclarationSyntax? fileScopedNamespaceDeclaration, AttributeSyntax attribute, ClassDeclarationSyntax? @class, InterfaceDeclarationSyntax? @interface)
            {
                if (@class == null && @interface == null) throw new ArgumentNullException(nameof(@class), "Either a class or interface must be provided");
                if (@namespace == null && fileScopedNamespaceDeclaration == null) throw new ArgumentNullException(nameof(@class), "Either a namespace or file scoped namespace must be provided");

                NamespaceDeclaration = @namespace;
                FileScopedNamespaceDeclaration = fileScopedNamespaceDeclaration;
                AttributeDeclaration = attribute;
                ClassDeclaration = @class;
                InterfaceDeclaration = @interface;
            }

            public string Namespace => NamespaceDeclaration?.Name.ToString() ?? FileScopedNamespaceDeclaration!.Name.ToString();
            public string Modifiers => ClassDeclaration?.Modifiers.ToString() ?? InterfaceDeclaration!.Modifiers.ToString();
            public string Keyword => ClassDeclaration?.Keyword.Text ?? InterfaceDeclaration!.Keyword.Text;
            public string Name => ClassDeclaration?.Identifier.Text ?? InterfaceDeclaration!.Identifier.Text;
        }

        public List<Capture> Captures { get; } = new();
    }
}
