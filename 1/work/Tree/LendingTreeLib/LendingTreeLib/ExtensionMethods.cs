using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Reflection;

namespace LendingTreeLib
{
    static public class ExtensionMethods
    {
        static public string FormatAsXml(this String source)
        {
            return XElement.Parse(source).ToString();
        }

        /// <summary>
        /// Preconditions:
        ///     - The web control supports a Text property
        ///     - The text is valid XML    
        /// Applies formatting to XML in a web control's Text.
        /// </summary>
        /// <param name="source"></param>
        static public void FormatTextAsXml(this System.Web.UI.Control source)
        {
            if (source != null)
            {

                var t = source.GetType();
                var pi = t.GetProperty("Text", typeof(String));
                var text = pi.GetValue(source, null) as string;
                var xml = XElement.Parse(text).ToString();
                pi.SetValue(source, xml, null);
            }     
        }
    }
}
