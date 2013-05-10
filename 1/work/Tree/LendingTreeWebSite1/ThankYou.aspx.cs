using System;
using System.IO;
using System.Net;
using System.Xml.Linq;
using LendingTreeLib;

/// <summary>
/// TODO: this code behind contains logic that might be better in a "Data Tier"..?
/// </summary>
public partial class ThankYou : QuickMatchPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!PageEnabled(EPages.ThankYou))
        {
            Response.Redirect(UrlFor(EPages.Page1));
        }

        ShowCreditScoreAd();

        Model.AffiliateSiteID = this.ReferringCdNumber;

        // NOTE: this logic can probably be removed since the ESourceId comes
        // from the theme configuration
        string esid = SessionValue<string>("ESourceId");
        if (esid != null)
        {
            Model.ESourceId = esid;
        }

        var urlForPost = _Model.LendingTreeConfig.PostUrl;
        
        var address = new Uri(urlForPost);

        var request = (HttpWebRequest)WebRequest.Create(address);
        request.ContentType = "text/xml";
        request.Method = "POST";

        using (var writer = new StreamWriter(request.GetRequestStream()))
        {
            var xmlForPost = Model.GetXMLForPost();
            var xDoc = XDocument.Parse(xmlForPost);

            var customLogic = new BeforeXmlPostCustomLogic(xDoc);
            customLogic.ReferringCdNumber = this.ReferringCdNumber;
            //customLogic.Check();

            Logger.Log(
                customLogic.XDoc.Root,
                EEventType.ApplicationSubmitting);

            writer.Write(xmlForPost);
        }

        using (var response = request.GetResponse() as HttpWebResponse)
        {
            var reader = new StreamReader(response.GetResponseStream());
            var xml = reader.ReadToEnd();

            var xl = XElement.Parse(xml);
            _Model.ResonseXml = xl;

            var eventTypeForLogEntry = EEventType.ApplicationCompleted;

            // The model decides if the pixel fires
            if (_Model.ResponseValidForPixelFire)
            {
                EnablePixelFire();
            }

            Logger.Log(
                new XElement("Fragment", new XAttribute("AppID", Model.AppID), xl),
                eventTypeForLogEntry);
        }

        // Now that the pixel has fired, we clear the session so it all has to start over.
        Session.Abandon();
    }

    // Randomly display one of (FreeCreditScore1, MyFreeScoreNow1)
    private void ShowCreditScoreAd()
    {
        FreeCreditScore1.Visible = true;

        // AdRotator functionality disabled per request from SC
        //
        //FreeCreditScore1.Visible = (new Random().Next(2) == 1);
        //MyFreeScoreNow1.Visible = !FreeCreditScore1.Visible;
    }
}