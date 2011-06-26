using System;
using System.Web.UI;

public partial class Page2 : QuickMatchPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!PageEnabled(EPages.Page2))
        {
            Response.Redirect(Resources.Url.Page1);
        }
        AdjustControlVisibility();
    }

    void AdjustControlVisibility()
    {
        if (IsPurchase)
            SetVisible(false, new Control[] {
                ApproximatePropertyValue1,
                MortgageBalance1,
                AmountDesiredAtClosing1,
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
            Model.CashOut = Decimal.Parse(AmountDesiredAtClosing1.Value);
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