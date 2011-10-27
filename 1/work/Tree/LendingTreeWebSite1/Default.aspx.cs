using System;
using System.IO;
using LendingTreeLib;

public partial class _Default : QuickMatchPageBase
{
    const string QueryStringParameterNameForTheme = "v";
    const string ThemeNameFormat = "V{0}";

    /// <summary>
    /// Support specifying the theme as a query parameter to Default.aspx.
    /// 
    /// If a theme number is present when the page loads (i.e. Default.aspx?v=1)
    /// it is used to initialize the theme name that will be used for all subsequent
    /// requests.
    /// 
    /// TODO: create some kind of documentation artifact to track the moving parts of how
    /// this works
    /// </summary>
    [LendingTreeLib.QueryString(QueryStringParameterNameForTheme, "1")]
    public string SiteVariantId { get; set; }

    /// <summary>
    /// Setting the ESourceId for the theme is very important so leads are tracked 
    /// correctly.
    /// 
    /// TODO: create tests/guards that ensure the esource id matches the theme
    /// (and that the current theme matches the initially set theme too).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Store theme in session.
        Session[SessionKeys.Theme] = string.Format(ThemeNameFormat, SiteVariantId);

        // Load theme specific ESourceId from a text file with the value to
        // use on the first line.
        if (File.Exists(ESourceIdFilePath))
        {
            Model.ESourceId = File.ReadAllLines(ESourceIdFilePath)[0];
        }

        // Load theme specific campaign id to use in pixel from a text file with the value to
        // use on the first line.
        if (File.Exists(DirectTrackCampaignIdFilePath))
        {
            Session[SessionKeys.DirectTrackCampaignId] = File.ReadAllLines(DirectTrackCampaignIdFilePath)[0];
        }
        else
        {
            Session[SessionKeys.DirectTrackCampaignId] = "1708";
        }

        Response.Redirect(Resources.Url.Page1);
    }

    string ESourceIdFilePath
    {
        get
        {
            return Server.MapPath(
                String.Format("~/App_Themes/{0}/{1}", ThemeName, Resources.ThemeConfig.ESourceIdFileName));
        }
    }

    string DirectTrackCampaignIdFilePath 
    {
        get
        {
            return Server.MapPath(
                String.Format("~/App_Themes/{0}/{1}", ThemeName, Resources.ThemeConfig.DirectTrackCampaignIdFileName));
        }
    }
}