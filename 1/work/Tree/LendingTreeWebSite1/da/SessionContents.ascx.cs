using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Collections.Generic;

public partial class da_SessionContents : System.Web.UI.UserControl
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        List<XElement> keyElements = new List<XElement>();
        foreach (string sessionKey in Session.Keys)
        {
            XAttribute attribute = new XAttribute("name", sessionKey);
            XElement keyElement = new XElement("key", attribute, Session[sessionKey].ToString());
            keyElements.Add(keyElement);
        }
        XElement keysElement = new XElement("keys", keyElements);
        SessionContentXmlTransform.DocumentContent = keysElement.ToString(SaveOptions.DisableFormatting);
    }
}