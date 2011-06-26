using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class da_Default : QuickMatchPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsAdmin)
        {
            if (!Page.IsPostBack)
            {
                CalendarFrom.SelectedDate = DateTime.Today;
                CalendarTo.SelectedDate = DateTime.Now;
            }
        }
        else
        {
            Response.Redirect("~/");
        }
    }
    protected void TimestampClicked(object sender, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Request":
                Response.Write("<h2>REQUEST " + e.CommandArgument + "</h2>");
                break;
            case "Response":
                Response.Write("<h2>RESPONSE " + e.CommandArgument + "</h2>");
                break;
            default:
                break;
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string fromTicks = CalendarFrom.SelectedDate.Ticks.ToString();
        string toTicks = CalendarTo.SelectedDate.Ticks.ToString();
        string urlFormat = "Report.ashx?from={0}&to={1}";
        string url = string.Format(urlFormat, fromTicks, toTicks);
        Response.Redirect(url);
    }
}