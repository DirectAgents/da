using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Xml.Linq;

public partial class Controls_Shared_ViewSession : LendingTreeLib.UserControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (WebConfigurationManager.AppSettings["ViewSession"] == "Y")
        {
            var xe =
                new XElement("keys",
                    from key in EnumerateKeys()
                    select new XElement("key", new XAttribute("name", key), Session[key].ToString())
                );

            Xml1.DocumentContent = xe.ToString(SaveOptions.DisableFormatting);
        }
        else
        {
            Visible = false;
        }
    }

    private IEnumerable<string> EnumerateKeys()
    {
        foreach (string item in Session.Keys)
        {
            yield return item;
        }
    }
}