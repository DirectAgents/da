using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Controls_Page2_PropertyZip : LendingTreeLib.UserControlBase
{
    // note: an "smart editor" might let you sub variables into strings without string.format
    // note: the whole idea is that of the "local DSL" which the editor controls

    protected void Page_Load(object sender, EventArgs e)
    {
        TextBox1.Attributes.Add("onfocus", "CheckValue(this, zipRequiredMessage)");
        TextBox1.Attributes.Add("onblur", "zipCheck(this, zipRequiredMessage)");
    }

    public string Value { get { return TextBox1.Text; } set { TextBox1.Text = value; } }

    public string ZipValidatorClientID { get { return RegularExpressionValidator1.ClientID; } }

}