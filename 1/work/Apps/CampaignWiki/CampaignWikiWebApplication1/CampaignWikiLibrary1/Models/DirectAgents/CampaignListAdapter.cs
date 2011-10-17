using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CampaignWikiWebApplication1.Models.DirectAgents
{
    public class CampaignListAdapter : DirectAgentsModelContainer
    {
        private int _start;
        private int _max;
        private bool _selectingRange;

        public CampaignListAdapter()
            : base()
        {
        }

        public CampaignListAdapter(int start, int max)
            : base()
        {
            _start = start;
            _max = max;
            _selectingRange = true;
        }

        // TODO: DI
        T SessionValue<T>(string key) { return (T)HttpContext.Current.Session[key]; }
        public string CurrentUserName { get { return SessionValue<string>("UserName"); } }
        public string FilterNameStartsWith { get { return SessionValue<string>("CampaignList_Filter_NameStartsWith"); } }
        public string FilterNameDoesNotStartWith { get { return SessionValue<string>("CampaignList_Filter_NameDoesNotStartWith"); } }

        public IEnumerable<CampaignList.Campaign> Campaigns
        {
            get
            {
                var selected = this.CurrentUser.DirectTrackCampaigns.ToList();
                foreach (var campaign in this.FilteredCampaigns)
                {
                    var result = new CampaignList.Campaign {
                        IsSelected = selected.Contains(campaign),
                        Name = campaign.CampaignName,
                        PID = campaign.CampaignNumber
                    };
                    yield return result;
                }
            }
        }

        public IEnumerable<CampaignList.Campaign> SelectedCampaigns
        {
            get
            {
                foreach (var campaign in this.CurrentUser.DirectTrackCampaigns)
                {
                    var result = new CampaignList.Campaign {
                        IsSelected = true,
                        Name = campaign.CampaignName,
                        PID = campaign.CampaignNumber
                    };
                    yield return result;
                }
            }
        }

        User CurrentUser { get { return Users.First(c => c.Name == this.CurrentUserName); } }

        IQueryable<DirectTrackCampaign> FilteredCampaigns
        {
            get
            {
                var campaigns = DirectTrackCampaigns.AsQueryable();

                if (!string.IsNullOrEmpty(this.FilterNameStartsWith))
                {
                    campaigns = campaigns.Where(c => c.CampaignName.StartsWith(this.FilterNameStartsWith));
                }

                if (!string.IsNullOrEmpty(this.FilterNameDoesNotStartWith))
                {
                    campaigns = campaigns.Where(c => !c.CampaignName.StartsWith(this.FilterNameDoesNotStartWith));
                }

                return _selectingRange
                            ? campaigns.OrderBy(c => c.CampaignName).Skip(_start).Take(_max)
                            : campaigns;
            }
        }
    }
}
