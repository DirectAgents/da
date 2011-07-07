using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

[Themeable(true)]
public partial class Controls_Page3_SSNReadMore : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public bool Visible
    {
        get
        {
            return Panel1.Visible;
        }
        set
        {
            Panel1.Visible = value;
        }
    }
}