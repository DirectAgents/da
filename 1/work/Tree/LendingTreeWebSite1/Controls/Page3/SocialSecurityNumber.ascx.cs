using System;
using System.Web.UI;

[Themeable(true)]
public partial class Controls_Page3_SocialSecurityNumber : LendingTreeLib.UserControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    public string Value
    {
        get
        {
            return TextBox1.Text;
        }
    }
    public bool Required
    {
        get
        {
            return RequiredFieldValidator1.Enabled;
        }
        set
        {
            RequiredFieldValidator1.Enabled = value;
            Optional1.Visible = !value;
        }
    }
}