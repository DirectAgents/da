using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class da_Default : QuickMatchPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GridView1.DataBound += new EventHandler(GridView1_DataBound);

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

    void GridView1_DataBound(object sender, EventArgs e)
    {
        HideFixUrlsForEmptyErrors();
    }

    private void HideFixUrlsForEmptyErrors()
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            var l = (Label)row.FindControl("Label1");
            var h = (HyperLink)row.FindControl("FixHyperLink");
            h.Visible = !string.IsNullOrWhiteSpace(l.Text);
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