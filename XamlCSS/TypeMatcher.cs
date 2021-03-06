﻿using XamlCSS.CssParsing;
using XamlCSS.Dom;

namespace XamlCSS
{
    public class TypeMatcher : SelectorMatcher
    {
        private StyleSheet initializedWith;

        public TypeMatcher(CssNodeType type, string text) : base(type, text)
        {
            
        }

        private void Initialize(StyleSheet styleSheet)
        {
            var namespaceSeparatorIndex = Text.IndexOf('|');
            string @namespace = null;
            //string prefix = null;
            string alias = "";
            string tagName = null;
            if (namespaceSeparatorIndex > -1)
            {
                alias = Text.Substring(0, namespaceSeparatorIndex);
                tagName = Text.Substring(namespaceSeparatorIndex + 1);
                if (alias != "*")
                {
                    @namespace = styleSheet.GetNamespaceUri(alias, tagName);
                }
                else
                {

                }
            }
            else
            {
                tagName = Text;
                @namespace = styleSheet.GetNamespaceUri("", tagName);
            }

            this.Alias = alias;
            this.TagName = tagName;
            this.NamespaceUri = @namespace;
            this.initializedWith = styleSheet;
        }

        public override MatchResult Match<TDependencyObject>(StyleSheet styleSheet, ref IDomElement<TDependencyObject> domElement, SelectorMatcher[] fragments, ref int currentIndex)
        {
            if (initializedWith != styleSheet)
            {
                Initialize(styleSheet);
            }

            var isMatch = domElement.TagName == TagName && (Alias == "*" || domElement.AssemblyQualifiedNamespaceName == NamespaceUri);
            return isMatch ? MatchResult.Success : MatchResult.ItemFailed;
        }

        public string Alias { get; private set; }
        public string TagName { get; private set; }
        public string NamespaceUri { get; private set; }
    }
}
