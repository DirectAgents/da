using System;
using LendingTreeLib;

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

    protected void Page_Load(object sender, EventArgs e)
    {
        Visible = PageBase.SessionValue<bool>(SessionKeys.QuickMatchPrefix + SessionKeys.PixelFireEnabled);
    }
}
