using System;
using System.Linq;
using System.Web.UI;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.XPath;

public partial class Page2 : QuickMatchPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!PageEnabled(EPages.Page2))
        {
            Response.Redirect(Resources.Url.Page1);
        }
        AdjustControlVisibility();

        if (Request["ltv"] != null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "setltv",
                string.Format("g.MaxLTV = {0};", Convert.ToDecimal((string)Request["ltv"])), true);
        }
    }

    protected string GenerateJSON()
    {
        return
            GenerateJSONForOptions("ApproximatePropertyValue") + "\n" +
            GenerateJSONForOptions("MortgageBalance") + "\n" +
            GenerateJSONForOptions("CashOut");
    }

    private string GenerateJSONForOptions(string optionName)
    {
        var sb = new StringBuilder();
        int count = 0;
        foreach (var option in
            XDocument.Load(MapPath("~/App_Data/Data.xml"))
                .XPathSelectElements("./data/options[@question='" + optionName + "']/option")
                //.Skip(1)
                )
        {
            sb.AppendFormat("{0}:'{1}',", option.Attribute("value").Value, option.Value);
            count++;
        }
        var resultFormat = "{0}; {1};";
        var result = string.Format(resultFormat,
            optionName + "Options = {\n" + sb.ToString().TrimEnd(',') + "}",
            optionName + "Length = " + count);
        return result;
    }

    void AdjustControlVisibility()
    {
        if (IsPurchase)
            SetVisible(false, new Control[] {
                ApproximatePropertyValue1,
                MortgageBalance1,
                CashOut1,
                PropertyZip1,
                MonthlyMortgagePayment1
            });
        if (IsRefi)
            SetVisible(false, new Control[] {
                PurchasePrice1,
                DownPayment1,
                PropertyCity1
            });
    }

    protected void Continue1_ButtonClick(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            Save();
            EnablePage(EPages.Page3);
            Response.Redirect(UrlFor(EPages.Page3));
        }
    }

    private void Save()
    {
        Model.PropertyType = PropertyType1.Value;
        Model.PropertyUse = PropertyPurpose1.Value;
        Model.LendingTreeLoansOptIn = LendingTreeLoansOptIn1.Value;
        Model.IsVetran = VetranStatus1.Value;
        Model.BankruptcyDischarged = HadBakruptcy1.Value;
        Model.ForeclosureDischarged = HadForeclosures1.Value;
        if (IsRefi)
        {
            Model.PropertyZip = PropertyZip1.Value;
            Model.PropertyApproximateValue = Decimal.Parse(ApproximatePropertyValue1.Value);
            Model.EstimatedMortgageBalance = Decimal.Parse(MortgageBalance1.Value);
            Model.CashOut = Decimal.Parse(CashOut1.Value);
            Model.MonthlyPayment = Decimal.Parse(MonthlyMortgagePayment1.Value);
        }
        if (IsPurchase)
        {
            Model.PurchasePrice = Decimal.Parse(PurchasePrice1.Value);
            Model.DownPayment = Decimal.Parse(DownPayment1.Value);
            Model.PropertyCity = PropertyCity1.Value;
        }
    }
}