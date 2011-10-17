using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SCS.Web.UI.WebControls
{
    sealed class DefaultTemplate : ITemplate
    {
        void ITemplate.InstantiateIn(Control owner)
        {
            //Label title = new Label();
            //title.DataBinding += new EventHandler(title_DataBinding);

            //LiteralControl linebreak = new LiteralControl("<br/>");

            //Label caption = new Label();
            //caption.DataBinding
            //    += new EventHandler(caption_DataBinding);

            //owner.Controls.Add(title);
            //owner.Controls.Add(linebreak);
            //owner.Controls.Add(caption);
        }

        void caption_DataBinding(object sender, EventArgs e)
        {
            //Label source = (Label)sender;
            //SplitterPanel container = (SplitterPanel)(source.NamingContainer);

            //source.Text = container.Caption;
        }

        void title_DataBinding(object sender, EventArgs e)
        {
            //Label source = (Label)sender;
            //SplitterPanel container = (SplitterPanel)(source.NamingContainer);

            //source.Text = container.Title;
        }
    }
}
