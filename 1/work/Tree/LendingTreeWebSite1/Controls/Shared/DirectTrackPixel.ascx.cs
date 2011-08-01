using System;
using LendingTreeLib;
using System.Web.UI;

[Themeable(true)]
public partial class Controls_Shared_DirectTrackPixel : LendingTreeLib.UserControlBase
{
    public string OptionalInformation
    {
        get
        {
            return PageBase.Model.AppID;
        }
    }

    public string Pid
    {
        get
        {
            return (string)Session[SessionKeys.DirectTrackCampaignId];
        }
    }

    public bool ExtraPixel
    {
        get
        {
            return ExtraPixelPanel.Visible;
        }
        set
        {
            ExtraPixelPanel.Visible = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Visible = PageBase.SessionValue<bool>(SessionKeys.QuickMatchPrefix + SessionKeys.PixelFireEnabled);
    }
}
