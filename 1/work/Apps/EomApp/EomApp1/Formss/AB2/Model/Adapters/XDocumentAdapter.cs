using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;

namespace EomApp1.Formss.AB2.Model.Adapters
{
    public class XDocumentAdapter : IAdaptable<XDocument>, IMapper
    {
        public XDocumentAdapter(XDocument xDoc)
        {
            From = xDoc;
            string rootElement = xDoc.Root.Name.LocalName;
            Type adapter = this.GetType().GetNestedTypes().Where(c => c.Name == rootElement).First();
            Adapted = Activator.CreateInstance(adapter, xDoc);
        }

        #region IAdaptable

        public XDocument From { get; set; }

        public object Adapted { get; set; } 

        #endregion

        // Maps the adapted object to the target property with the same name as its type
        #region IMapper

        public void MapTo<TTo>(TTo obj)
        {
            var prop = from c in obj.GetType().GetProperties()
                       where c.PropertyType.IsAssignableFrom(Adapted.GetType())
                       select c;

            if (prop.Count() != 1) throw new Exception("ambiguous");

            prop.First().SetValue(this, Adapted, null);
        }

        #endregion

        // An adapter class with the same name as the XDoc root element name that 
        // inherits from the target entity type
        #region Adapters

        public class campaign : DirectTrackCampaign
        {
            public campaign(XDocument xDoc)
            {
            }
        }

        public class campaignGroup : DirectTrackCampaignGroup
        {
            public campaignGroup(XDocument xDoc)
            {
                var cg = new DAgents.Synch.CampaignGroup(xDoc.ToString());

                this.Name = cg.Name;

                // TODO: add campaign associations...
                //this.DirectTrackCampaigns.Add(
            }
        }

        #endregion
    }
}
