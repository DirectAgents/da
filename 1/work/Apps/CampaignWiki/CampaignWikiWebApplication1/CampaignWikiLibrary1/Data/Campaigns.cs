using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace CampaignWikiWebApplication1.Data
{
    public class Campaigns
    {
        public List<Models.CampaignList.Campaign> GetCampaigns(int start, int max)
        {
            using (var adapter = new Models.DirectAgents.CampaignListAdapter(start, max))
            {
                return adapter.Campaigns.OrderBy(c => c.Name).ToList();
            }
        }

        public int CountCampaigns()
        {
            using (var adapter = new Models.DirectAgents.CampaignListAdapter())
            {
                return adapter.Campaigns.Count();
            }
        }

        public List<Models.CampaignList.Campaign> GetSelectedCampaigns(int start, int max)
        {
            using (var adapter = new Models.DirectAgents.CampaignListAdapter())
            {
                return adapter.SelectedCampaigns.OrderBy(c => c.Name).ToList();
            }
        }

        public int CountSelectedCampaigns()
        {
            using (var adapter = new Models.DirectAgents.CampaignListAdapter())
            {
                return adapter.SelectedCampaigns.Count();
            }
        }
    }
}
