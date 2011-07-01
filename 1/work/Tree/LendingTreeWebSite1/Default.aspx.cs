using System;
using System.IO;
using LendingTreeLib;

public partial class _Default : QuickMatchPageBase
{
    [LendingTreeLib.QueryStringParameter("v", "1")]
    public string SiteVariantId { get; set; }

    void InitThemeName()
    {
        ThemeName = "V" + SiteVariantId;
    }

    string ESourceIdFilePath
    {
        get
        {
            return Server.MapPath(String.Format("~/App_Themes/{0}/{1}", ThemeName, Resources.ThemeConfig.ESourceIdFileName));
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        InitThemeName();

        if (File.Exists(ESourceIdFilePath))
        {
            Model.ESourceId = File.ReadAllLines(ESourceIdFilePath)[0];
        }

        Response.Redirect(Resources.Url.Page1);
    }
}