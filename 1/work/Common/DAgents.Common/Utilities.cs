using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Dynamic;
using System.Collections;
using System.Xml.Linq;
using System.Linq;
using System;
namespace DAgents.Common
{
    public static class Utilities
    {
        public static string MakeLegalFilename(string fileName)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }

            return fileName;
        }

        public static string FormatXml(XmlDocument xdoc)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var xw = new XmlTextWriter(sw);
            xw.Formatting = Formatting.Indented;
            xdoc.Save(xw);
            return sb.ToString();
        }

        public static string GetWindowsIdentityNameLower()
        {
            var ident = System.Security.Principal.WindowsIdentity.GetCurrent();
            return ident.Name;
        }

        public class XSettings : DynamicObject, IEnumerable
        {
            private XElement _xe;

            public XSettings(string xml)
            {
                this._xe = XElement.Parse(xml);
            }

            public XSettings(XElement xe)
            {
                this._xe = xe;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                string name = binder.Name;

                if (name == "Count")
                {
                    result = _xe.Elements().Count();
                    return true;
                }

                result = new XSettings(_xe.Element(name));
                return true;
            }

            public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
            {
                int index = (int)indexes[0];
                result = new XSettings(_xe.Elements().ElementAt(index));
                return true;
            }

            public static implicit operator string(XSettings x)
            {
                return x._xe.Value;
            }

            public static implicit operator DateTime(XSettings x)
            {
                return DateTime.Parse(x._xe.Value);
            }

            public static implicit operator decimal(XSettings x)
            {
                return Decimal.Parse(x._xe.Value);
            }

            public IEnumerator GetEnumerator()
            {
                return _xe.Elements().Select(c => new XSettings(c)).GetEnumerator();
            }
        }

        public static IList<dynamic> GetExpandoFromXml(XDocument doc, string elementName)
        {
            var list = new List<dynamic>();

            var nodes = from node in doc.Root.Descendants(elementName)
                        select node;

            foreach (var node in nodes)
            {
                dynamic expando = new ExpandoObject();

                foreach (var child in node.Descendants())
                {
                    (expando as IDictionary<String, object>)[child.Name.ToString()] = child.Value.Trim();
                }

                list.Add(expando);
            }

            return list;
        }

        public class ConsoleLogger : ILogger
        {
            public void Log(string message)
            {
                Console.WriteLine(message);
            }

            public void LogError(string message)
            {
                Console.WriteLine("Error! " + message);
            }
        }
    }
}
