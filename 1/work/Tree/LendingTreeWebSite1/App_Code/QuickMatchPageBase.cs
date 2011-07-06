using System;
using LendingTreeLib;
using LendingTreeLib.Schemas;

public class QuickMatchPageBase : PageBase
{
    /// <summary>
    /// Set theme of page to value specified in session
    /// or default if session key not found.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        if (Session[SessionKeys.Theme] == null)
        {
            Page.Theme = Resources.ThemeConfig.DefaultThemeName;
        }
        else
        {
            Page.Theme = SessionValue<string>(SessionKeys.Theme);
        }

        base.OnPreInit(e);
    }


    public string ThemeName
    {
        get
        {
            return SessionValue<string>(SessionKeys.Theme) ?? Page.Theme;
        }
    }

    /// <summary>
    /// Extracts cd number from query string.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLoad(EventArgs e)
    {
        // Extract cd number from query string.
        string cdNumber = Request.QueryString[Resources.QueryStringKey.QueryStringParamForCdNumber];  
        if (cdNumber != null)
        {
            this.ReferringCdNumber = cdNumber;
        }
        base.OnLoad(e);
    }

    protected string ReferringCdNumber
    {
        get
        {
            return (string)Session[SessionKeys.QuickMatchPrefix + SessionKeys.ReferringCdNumber] ?? String.Empty;
        }
        set
        {
            Session[SessionKeys.QuickMatchPrefix + SessionKeys.ReferringCdNumber] = value;
        }
    }

    protected string UrlFor(EPages page)
    {
        string url = String.Empty;
        switch (page)
        {
            case EPages.Page1:
                url = Resources.Url.Page1;
                break;
            case EPages.Page2:
                url = Resources.Url.Page2;
                break;
            case EPages.Page3:
                url = Resources.Url.Page3;
                break;
            case EPages.Processing:
                url =  Resources.Url.Processing;
                break;
            case EPages.ThankYou:
                url = Resources.Url.ThankYou;
                break;
            default:
                throw new Exception("invalid page");
        }
        return url;
    }

    protected string FullUrlFor(EPages page)
    {
        return ResolveUrl(UrlFor(page));
    }

    protected void EnablePage(EPages page)
    {
        Session[GetSessionKeyForPage(page)] = 1;
    }

    protected bool PageEnabled(EPages page)
    {
        return Session[GetSessionKeyForPage(page)] != null;
    }

    string GetSessionKeyForPage(EPages page)
    {
        return SessionKeys.EnablePagePrefix + Enum.GetName(typeof(EPages), page);
    }

    protected bool IsRefi
    {
        get
        {
            return Model.LoanType == Enum.GetName(typeof(ELoanType), ELoanType.REFINANCE);// todo: model.IsRefi
        }
    }

    protected bool IsPurchase
    {
        get
        {
            return Model.LoanType == Enum.GetName(typeof(ELoanType), ELoanType.PURCHASE);// todo:...
        }
    }

    protected string PropertyStateName
    {
        get
        {
            var sessionKey = SessionKeys.QuickMatchPrefix + SessionKeys.PropertyStateName;
            var sessionValue = (string)Session[sessionKey];
            return sessionValue ?? "";
        }
    }

    protected void EnablePixelFire()
    {
        Session[SessionKeys.QuickMatchPrefix + SessionKeys.PixelFireEnabled] = true;
    }
}