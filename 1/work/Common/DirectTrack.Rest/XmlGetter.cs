using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using DAgents.Common;
namespace DirectTrack.Rest
{
    public static class XmlGetter
    {
        private const string UrlNoClientId = "https://da-tracking.com/apifleet/rest/1137/";
        private const string UrlWithClientId = UrlNoClientId + "1/";
        public static string GetXml(string url)
        {
            XmlDocument xmldoc = DirectTrackRestCall.GetXmlDoc(url);
            return Utilities.FormatXml(xmldoc);
        }
        public static string ListPayouts(int pid)
        {
            return GetXml(UrlWithClientId + "payout/campaign/" + pid + "/");
        }
        public static string ViewPayout(string payoutID)
        {
            return GetXml(UrlNoClientId + "payout/" + payoutID);
        }
        public static string ViewAffilaite(int affid)
        {
            return GetXml(UrlNoClientId + "affiliate/" + affid)
                .Replace("<accessID />", "<accessID>999</accessID>")
                .Replace("<paymentMethod />", "<paymentMethod>check</paymentMethod>")
                .Replace("<approval />", "");
        }
        public static string ListCampaigns()
        {
            return DirectTrackRestCall.GetXml(UrlWithClientId + "campaign/");
        }
        public static string ViewCampaignGroup(string id)
        {
            return DirectTrackRestCall.GetXml(UrlNoClientId + "campaignGroup/" + id);
        }
        public static string ViewAffiliateGroup(string id)
        {
            return DirectTrackRestCall.GetXml(UrlNoClientId + "affiliateGroup/" + id);
        }
        public static string ListCampaignGroups()
        {
            return DirectTrackRestCall.GetXml(UrlNoClientId + "campaignGroup/");
        }
        public static string ViewCampaign(int pid)
        {
            return DirectTrackRestCall.GetXml(UrlNoClientId + "campaign/" + pid);
        }
        public static string ByAffiliateForASingleDay(int pid, DateTime dt)
        {
            string xml = GetXml(UrlWithClientId + "statCampaign/affiliate/campaign/" + pid + "/" + dt.ToString("yyyy-MM-dd"));
            return xml;
        }
        public static void GetResrouces(string startURL, List<XDocument> result, ILogger logger)
        {
            logger.Log("Getting resources at " + startURL);
            var doc = XDocument.Parse(GetXml(startURL));
            var xs = new XmlSerializer(typeof(resourceList));
            if (xs.CanDeserialize(doc.CreateReader()))
            {
                var rl = (resourceList)xs.Deserialize(doc.CreateReader());
                foreach (var item in rl.resourceURL)
                {
                    GetResrouces(startURL + item.location, result, logger);
                }
            }
            else
            {
                result.Add(doc);
            }
        }
    }
}
