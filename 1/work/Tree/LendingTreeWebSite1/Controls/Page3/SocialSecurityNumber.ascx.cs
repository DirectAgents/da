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
            if(!string.IsNullOrEmpty(TextBox1.Text))
                return string.Format("{0}-{1}-{2}", TextBox1.Text, TextBox2.Text, TextBox3.Text);
            else
                return string.Empty;
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
            RequiredFieldValidator2.Enabled = value;
            RequiredFieldValidator3.Enabled = value;
            Optional1.Visible = !value;
        }
    }
}