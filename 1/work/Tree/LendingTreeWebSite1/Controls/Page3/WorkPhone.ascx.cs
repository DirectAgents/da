using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Controls_Page3_WorkPhone : LendingTreeLib.UserControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    public string Value
    {
        get
        {
            return PhoneNumber1.Value;
        }
    }
}