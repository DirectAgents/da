using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//public partial class Processing : QuickMatchPageBase<Processing>
public partial class Processing : QuickMatchPageBase

{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!PageEnabled(EPages.Processing))
        {
            Response.Redirect(Resources.Url.Page3);
        }

        EnablePage(EPages.ThankYou);

        // NOTE: a Javascript redirect is used to redirect
    }
}