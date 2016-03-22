﻿namespace AngleSharp.Core.Tests.External
{
    using AngleSharp.Dom.Xml;
    using AngleSharp.Parser.Xml;
    using AngleSharp.Xml;
    using System;
    using System.IO;

    sealed class SiteMapping
    {
        readonly String _fileName;
        readonly IXmlDocument _xml;

        public SiteMapping(String fileName)
        {
            _fileName = fileName;
            var parser = new XmlParser();
            var content = File.Exists(fileName) ? File.ReadAllText(_fileName) : "<entries></entries>";
            _xml = parser.Parse(content);
        }

        public String this[String url]
        {
            get
            {
                var element = _xml.QuerySelector("entry[url='" + url + "']");
                return element != null ? element.TextContent : null;
            }
        }

        public Boolean Contains(String url)
        {
            return this[url] != null;
        }

        public void Add(String url, String fileName)
        {
            if (Contains(url) == false)
            {
                var element = _xml.CreateElement("entry");
                element.SetAttribute("url", url);
                element.TextContent = fileName;
                _xml.DocumentElement.AppendChild(element);

                using (var writer = File.CreateText(_fileName))
                {
                    _xml.ToHtml(writer, XmlMarkupFormatter.Instance);
                }
            }
        }
    }
}
