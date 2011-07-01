using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using LendingTreeLib;

public partial class da_Fix : PageBase
{
    [QueryStringParameter]
    public string appid { set { AppId.Text = value; } }

    LinkButton Repost { get { return Details1.FindControl("Repost") as LinkButton; } }
    TextBox ResponseBox { get { return Details1.FindControl("ResponseText") as TextBox; } }
    TextBox SubmitBox { get { return Details1.FindControl("PostText") as TextBox; } }

    string ResponseText
    {
        set
        {
            ResponseBox.Text = value;
            FormatResponseBox();
        }
    }

    private void FormatResponseBox()
    {
        ResponseBox.BackColor = IsValidResponse(ResponseBox.Text) ? Color.LightGreen : Color.LightPink;
    }

    private bool IsValidResponse(string s)
    {
        return s.Contains("<ReturnURL>");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Details1.DataBound += (s1, e1) => { FixXmlFormatting(); };
        Repost.Click += (s2, e2) => { DoRepost(); };
        FormatResponseBox();
    }

    private void FixXmlFormatting()
    {
        ResponseBox.FormatTextAsXml();
        SubmitBox.FormatTextAsXml();
    }

    private void DoRepost()
    {
        var config = Resolve<LendingTreeConfig>();  
        var request = (HttpWebRequest)WebRequest.Create(new Uri(config.PostUrl));
        request.ContentType = "text/xml";
        request.Method = "POST";
        using (var writer = new StreamWriter(request.GetRequestStream()))
        {
            var xDoc = XDocument.Parse(SubmitBox.Text);
            writer.Write(xDoc.Root.ToString());
        }
        using (var response = request.GetResponse() as HttpWebResponse)
        {
            var reader = new StreamReader(response.GetResponseStream());
            ResponseText = XElement.Parse(reader.ReadToEnd()).ToString();
        }
    }
}
