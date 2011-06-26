using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Controls_Page3_DateOfBirth : LendingTreeLib.UserControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AddWatermark(TextBox1, Resources.String.String54);
    }
    public string Value
    {
        get
        {
            return TextBox1.Text;
        }
    }
}