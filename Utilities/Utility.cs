using HtmlAgilityPack;
using System;

namespace banks.Utilities
{
    public delegate T TryParseDelegate<T>();
    public static class Utility
    {
        public static string TryNodeToInnerText(HtmlNode htmlNode)
        {
            return htmlNode is not null ? htmlNode?.InnerText?.RemoveWhitesSpaces() : String.Empty;
        }

        public static string TryNodeToInnerTextByXPth(HtmlNode htmlNode, string XPath)
        {
            var nodeSeleted = htmlNode is not null ? htmlNode.SelectSingleNode(XPath) : null;
            var value = nodeSeleted != null ? nodeSeleted.InnerText : null;
            return value is not null ? value.RemoveWhitesSpaces() : null;
        }

        public static string TryNodeToAttributeValue(HtmlNode htmlNode, string Attribute, string concatenate)
        {
            var value = htmlNode is not null ? htmlNode.GetAttributeValue(Attribute, string.Empty) : null;
            return value is not null ? string.Concat(concatenate, value) : String.Empty;
        }

        public static string TryNodesToInnerText(HtmlNodeCollection htmlNodes, int index)
        {
            var node = htmlNodes is not null ? htmlNodes[index] : null;
            var value = node is not null ? htmlNodes[index].InnerText : null;
            return value is not null ? value.RemoveWhitesSpaces() : string.Empty;
        }
        public static string TryNodesToInnerTextByXPath(HtmlNodeCollection htmlNodes, string XPath, int index)
        {
            var node = htmlNodes is not null ? htmlNodes[index] : null;
            var nodeSeleted = node is not null ? node.SelectSingleNode(XPath) : null;
            var value = nodeSeleted is not null ? nodeSeleted.InnerText : null;
            return value is not null ? value.RemoveWhitesSpaces() : String.Empty;
        }

        public static string TryNodeToAttributeValue(HtmlNodeCollection htmlNodes, string XPath, int index, string Attribute, string concatenate)
        {
            var node = htmlNodes is not null ? htmlNodes[index] : null;
            var nodeSeleted = node is not null ? node.SelectSingleNode(XPath) : null;
            var attribute = nodeSeleted is not null ? nodeSeleted.GetAttributeValue(Attribute, string.Empty) : string.Empty;
            return attribute is not null ? string.Concat(concatenate, attribute.RemoveWhitesSpaces()) : string.Empty;
        }

        public static T TryParse<T>(TryParseDelegate<T> tryParseDelegate)
        {
            try
            {
                return tryParseDelegate();
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static string RemoveWhitesSpaces(this string text)
        {
            return text.Trim().Replace("\r\n", string.Empty);
        }
    }
}
