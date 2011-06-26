using System;
using System.IO;
using System.Web.UI;
using System.Xml;
using System.Xml.Schema;
using System.Net;

//public partial class Page3 : QuickMatchPageBase<Page3>
public partial class Page3 : QuickMatchPageBase
{
    public string State 
    {
        get
        {
            return Model.PropertyState;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!PageEnabled(EPages.Page3))
        {
            Response.Redirect(UrlFor(EPages.Page2));
        }
        if (ZipCode1.IsEmpty)
        {
            ZipCode1.Value = Model.PropertyZip;
        }
    }

    protected void ShowMyResults1_ButtonClick(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            Save();
            EnablePage(EPages.Processing);
            Response.Redirect(UrlFor(EPages.Processing));
        }
    }

    private void Save()
    {
        Model.DOB = DateOfBirth1.Value;
        Model.Email = Email1.Value;
        Model.FirstName = FirstName1.Value;
        Model.HomePhone = HomePhone1.Value;
        Model.LastName = LastName1.Value;
        if (!string.IsNullOrWhiteSpace(SocialSecurityNumber1.Value))
        {
            Model.SSN = SocialSecurityNumber1.Value;
        }
        Model.Address = StreetAddress1.Value;
        if (!string.IsNullOrWhiteSpace(WorkPhone1.Value))
        {
            Model.WorkPhone = WorkPhone1.Value;
        }
        Model.ApplicantZipCode = ZipCode1.Value;
    }
}
