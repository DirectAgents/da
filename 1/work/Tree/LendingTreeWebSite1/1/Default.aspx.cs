using System;
using System.Web.UI;
using LendingTreeLib;
using System.Web.Configuration;

public partial class _1_Default : QuickMatchPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Model.RequiresInitialization())
        {
            Model.Initialize(); // visitor ip and url as params?
            _Model.VisitorIPAddress = Request.UserHostAddress;
            _Model.VisitorURL = WebConfigurationManager.AppSettings["BaseUrl"]; // T4 template for app settings?
        }
    }
    protected void GetQuoteNow1_ButtonClick(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            Save();
            EnablePage(EPages.Page2); // EnableAndGoto?
            Response.Redirect(UrlFor(EPages.Page2));
        }
    }

    // mapping attributes?
    private void Save()
    {
        Model.LoanType = LoanType1.Value;
        Model.PropertyState = PropertyState1.Value;
        Model.CreditRating = CreditRating1.Value;
        Session[SessionKeys.QuickMatchPrefix + SessionKeys.PropertyStateName] = PropertyState1.Name; // GetQuickMatchSessionKey(...)?
    }
}