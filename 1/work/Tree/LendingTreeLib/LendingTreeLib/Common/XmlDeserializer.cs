using System.IO;
using System.Xml.Serialization;

namespace LendingTreeLib.Common
{
    public static class XmlDeserializer
    {
        static public T Deserialize<T>(string xml)
        {
            return (T)new XmlSerializer(typeof(T)).Deserialize(new StringReader(xml));
        }
    }
}
