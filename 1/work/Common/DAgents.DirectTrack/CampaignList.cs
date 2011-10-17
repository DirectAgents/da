using System.Collections.Generic;
using DirectTrack.Rest;

namespace DAgents.Synch
{
    public class CampaignList : RestEntity<resourceList>
    {
        private CampaignList(string xml)
            : base(xml)
        {
        }

        public static CampaignList PullFromDirectTrack()
        {
            return new CampaignList(XmlGetter.ListCampaigns());
        }

        public IEnumerable<CampaignItem> CampaignItems
        {
            get
            {
                foreach (var resourceURL in this.inner.resourceURL)
                {
                    yield return new CampaignItem(resourceURL);
                }
            }
        }
    }
}
