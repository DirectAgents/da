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
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //if (FromDateSelector.SelectedDate == DateTime.MinValue)
        //{
        //    FromDateSelector.SelectedDate = DateTime.Today;
        //}
        //if (ToDateSelector.SelectedDate == DateTime.MinValue)
        //{
        //    ToDateSelector.SelectedDate = DateTime.Today;
        //}
    }

    protected void GraphsCheckBox_CheckChanged(object sender, EventArgs e)
    {
        //GraphsPanel.Visible = GraphsCheckBox.Checked;
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
        //string fromTicks = FromDateSelector.SelectedDate.Ticks.ToString();
        //string toTicks = ToDateSelector.SelectedDate.Ticks.ToString();
        string fromTicks = DateTime.Today.AddDays(7).Ticks.ToString();
        string toTicks = DateTime.Today.Ticks.ToString();
        string urlFormat = "Report.ashx?from={0}&to={1}";
        string url = string.Format(urlFormat, fromTicks, toTicks);
        Response.Redirect(url);
    }

    protected void DropSessionButton_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("~/da");
    }

    protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.CommandTimeout = 10000;
    }
}