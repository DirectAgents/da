using System;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CampaignWikiWebApplication1.WebUserControls
{
    public partial class ItemList : System.Web.UI.UserControl
    {
        class Helper : IDisposable
        {
            readonly Models.DirectAgents.DirectAgentsModelContainer _model = new Models.DirectAgents.DirectAgentsModelContainer();
            readonly int _selectedCampaignNumber;

            internal Helper(GridView gridView)
            {
                _selectedCampaignNumber = (int)gridView.SelectedValue;
            }

            internal Helper()
            {
                _selectedCampaignNumber = -1;
            }


            internal Models.DirectAgents.DirectTrackCampaign SelectedCampaign
            {
                get 
                {
                    if (_selectedCampaignNumber == -1) throw new Exception("selected campaign number not initialized");
                    return _model.DirectTrackCampaigns.FirstOrDefault(c => c.CampaignNumber == _selectedCampaignNumber); 
                }
            }

            internal EntityCollection<Models.DirectAgents.DirectTrackCampaign> SelectedCampaigns
            {
                get { return User.DirectTrackCampaigns; }
            }

            internal Models.DirectAgents.User User
            {
                get { return _model.Users.First(c => c.Name == UserName); }
            }

            internal string UserName
            {
                get { return (string)HttpContext.Current.Session["UserName"]; }
            }

            public void Dispose()
            {
                if (_model != null)
                {
                    _model.SaveChanges();
                    _model.Dispose();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CampaignsFilterDropDownList.Items.Add(new ListItem { Text = "all campaigns", Value = "" });
                CampaignsFilterDropDownList.Items.Add(new ListItem { Text = "US", Value = "US" });
                CampaignsFilterDropDownList.Items.Add(new ListItem { Text = "INTL", Value = "INTL" });
            }
        }

        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);

        //    // dynamic radio button list
        //    BulletedList bl = new BulletedList();
        //    bl.DataSource = CampaignsDataSource;
        //    bl.DataTextField = "Name";
        //    bl.Visible = true;
        //    bl.DataBind();
        //    Panel2.Controls.Add(bl);
        //    Panel2.Visible = true;
        //}

        protected void CampaignsFilterDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filter = (sender as DropDownList).SelectedValue;
            Session["CampaignList_Filter_NameStartsWith"] = "";
            Session["CampaignList_Filter_NameDoesNotStartWith"] = "";
            if (filter == "US")
            {
                Session["CampaignList_Filter_NameStartsWith"] = "US";
                Session["CampaignList_Filter_NameDoesNotStartWith"] = "";
            }
            if (filter == "INTL")
            {
                Session["CampaignList_Filter_NameStartsWith"] = "";
                Session["CampaignList_Filter_NameDoesNotStartWith"] = "US";
            }
            ReBind(CampaignsGridView);
        }

        protected void CampaignsPageSizeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            CampaignsGridView.PageSize = int.Parse(CampaignsPageSizeDropDownList.SelectedValue);
        }

        protected void CampaignsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var helper = new Helper(CampaignsGridView))
            {
                var campaign = helper.SelectedCampaign;
                var campaigns = helper.SelectedCampaigns;
                if(campaigns.Contains(campaign))
                    campaigns.Remove(campaign);
                else
                    campaigns.Add(campaign);
            }
            ReBind(CampaignsGridView, SelectedCampaignsGridView);
        }

        protected void SelectedCampaignsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var helper = new Helper(SelectedCampaignsGridView))
                helper.SelectedCampaigns.Remove(helper.SelectedCampaign);
            ReBind(CampaignsGridView, SelectedCampaignsGridView);
        }

        protected void ClearSelectedCampaignsButton_Click(object sender, EventArgs e)
        {
            using (var helper = new Helper())
                foreach (var campaign in helper.SelectedCampaigns.ToList())
                    helper.SelectedCampaigns.Remove(campaign);

            ReBind(CampaignsGridView, SelectedCampaignsGridView);
        }

        protected void CampaignsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LinkButton button = (LinkButton)e.Row.FindControl("SelectButton");
            if (button != null)
            {
                var campaign = (Models.CampaignList.Campaign)e.Row.DataItem;
                if (campaign.IsSelected)
                    button.Text = "Unselect";
            }
        }

        void ReBind(params GridView[] gridViews)
        {
            foreach (var gridView in gridViews)
                gridView.DataBind();
        }

        protected void CheckBox1_OnCheckedChanged(object sender, EventArgs e)
        {
        }

        protected void CampaignsGridView_PageIndexChanged(object sender, EventArgs e)
        {
            //GridView1.PageIndex = (sender as GridView).PageIndex;
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CampaignsGridView.DataBind();
        }
    }
}