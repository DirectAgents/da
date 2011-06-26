using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_Page1_GetQuoteNow : LendingTreeLib.UserControlBase
{
    public event EventHandler ButtonClick;

    protected void Button1_ButtonClick(object sender, EventArgs e)
    {
        if (ButtonClick != null)
        {
            ButtonClick(this, EventArgs.Empty);
        }
    }
}
