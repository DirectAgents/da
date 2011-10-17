using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Button b = (Button)SplitterPanel1.RightOwner.FindControl("Poster");

        //Button b = (Button)SplitterPanel1.FindControl("Poster");

        b.Text = "Postback";
    }
}
