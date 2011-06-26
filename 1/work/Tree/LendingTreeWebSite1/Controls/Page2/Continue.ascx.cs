using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Controls_Page2_Continue : LendingTreeLib.UserControlBase
{
    public event EventHandler ButtonClick; // TODO: dup

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Button1_ButtonClick(object sender, EventArgs e)
    {
        if (ButtonClick != null)
        {
            ButtonClick(this, EventArgs.Empty);
        }
    }
}