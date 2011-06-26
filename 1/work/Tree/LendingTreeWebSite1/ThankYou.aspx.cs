using System;
using System.IO;
using System.Net;
using System.Xml.Linq;
using LendingTreeLib;

public partial class ThankYou : QuickMatchPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!PageEnabled(EPages.ThankYou))
        {
            Response.Redirect(UrlFor(EPages.Page1));
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

            xDoc.Root.SetAttributeValue("affid", this.ReferringCdNumber);

            Logger.Log(
                xDoc.Root,
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
}