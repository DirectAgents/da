using LendingTreeLib.Schemas;
using System.IO;

namespace LendingTreeLib
{
    public class LendingTreeConfig
    {
        public SourceOfRequestType SourceOfRequest;
        public string PostUrl;

        static public LendingTreeConfig Create(string file)
        {
            string xml = File.ReadAllText(file);

            LendingTreeConfig o = Common.XmlDeserializer.Deserialize<LendingTreeConfig>(xml);
            o.SourceOfRequest.VisitorIPAddress = "127.0.0.1";
            o.SourceOfRequest.VisitorURL = "http://test.com";

            return o;
        }
    }
}
