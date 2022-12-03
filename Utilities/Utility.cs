using HtmlAgilityPack;
using System;

namespace banks.Utilities
{
    public delegate T TryParseDelegate<T>();
    public static class Utility
    {
        public static string TryNodeToInnerText(HtmlNode htmlNode)
        {
            return htmlNode is not null ? htmlNode.InnerText.RemoveWhitesSpaces() : String.Empty;
        }

        public static string TryNodeToInnerTextByXPth(HtmlNode htmlNode, string XPath)
        {
            return htmlNode is not null ? TryParse(() => htmlNode.SelectSingleNode(XPath).InnerText.RemoveWhitesSpaces()) : String.Empty;
        }

        public static string TryNodeToAttributeValue(HtmlNode htmlNode, string Attribute, string concatenate)
        {
            return TryParse(() => string.Concat(concatenate, htmlNode.GetAttributeValue(Attribute, string.Empty)).RemoveWhitesSpaces());
        }

        public static string TryNodesToInnerText(HtmlNodeCollection htmlNodes, int index)
        {
            return htmlNodes is not null ? TryParse(() => htmlNodes[index].InnerText.RemoveWhitesSpaces()) : string.Empty;
        }
        public static string TryNodesToInnerTextByXPath(HtmlNodeCollection htmlNodes, string XPath, int index)
        {
            return htmlNodes is not null ? TryParse(() => htmlNodes[index].SelectSingleNode(XPath).InnerText.RemoveWhitesSpaces()) : string.Empty;
        }

        public static string TryNodeToAttributeValue(HtmlNodeCollection htmlNode, string XPath, int index, string Attribute, string concatenate)
        {
            return TryParse(() => string.Concat(concatenate, htmlNode[index].SelectSingleNode(XPath).GetAttributeValue(Attribute, string.Empty)).RemoveWhitesSpaces());
        }

        public static T TryParse<T>(TryParseDelegate<T> tryParseDelegate)
        {
            try
            {
                return tryParseDelegate();
            }
            catch (Exception)
            {
                return (T)Convert.ChangeType(typeof(T) == typeof(string) ? string.Empty : null, typeof(T));
            }
        }

        public static string RemoveWhitesSpaces(this string text)
        {
            return text.Trim().Replace("\r\n", string.Empty);
        }
    }
}
