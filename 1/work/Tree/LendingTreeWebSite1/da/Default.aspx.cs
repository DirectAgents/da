using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class da_Default : QuickMatchPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsAdmin)
        {
            Response.Redirect("~/");
        }

        GridView1.DataBound += new EventHandler(GridView1_DataBound);

        if (!IsPostBack)
        {
            Session["fromtime"] = DateTime.Today.ToShortDateString();
            Session["totime"] = DateTime.Today.AddDays(1).ToShortDateString();
        }
    }

    protected void GraphsCheckBox_CheckChanged(object sender, EventArgs e)
    {
        GraphsPanel.Visible = GraphsCheckBox.Checked;
    }

    protected void CalendarFrom_SelectionChanged(object sender, EventArgs e)
    {
        Session["fromtime"] = CalendarFrom.SelectedDate.ToString("MM/dd/yyyy");
    }

    protected void CalendarFrom_SelectionChanged2(object sender, EventArgs e)
    {
        Session["totime"] = CalendarTo.SelectedDate.AddDays(1).ToString("MM/dd/yyyy");
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