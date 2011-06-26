using System;
using System.Web;
using System.Web.UI;
using LendingTreeLib.Common;
using LendingTreeLib.Schemas;
using Microsoft.Practices.Unity;
using System.ComponentModel;
using System.Linq;
using System.Web.Configuration;

namespace LendingTreeLib
{
    public abstract class PageBase : Page/*<T> : Page where T : class, IFoo*/
    {
        [Dependency]
        public LendingTreeModel _Model { set; get; }

        [Dependency]
        public ILogger Logger { get; set; }

        public bool IsAdmin
        {
            get
            {
                string requestIP = Request.UserHostAddress;
                string adminIPs = WebConfigurationManager.AppSettings["AdminIps"];

                if (adminIPs.Contains(requestIP))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public ILendingTreeModel Model
        {
            get
            {
                return (ILendingTreeModel)_Model;
            }
        }

        public TT SessionValue<TT>(string sessionKey)
        {
            object sessionValue = Session[sessionKey];

            if (sessionValue == null)
            {
                return default(TT);
            }
            else
            {
                return (TT)sessionValue;
            }
        }

        protected void SetVisible(bool visible, System.Web.UI.Control[] controls)
        {
            foreach (var control in controls)
            {
                control.Visible = visible;
            }
        }

        /// <summary>
        /// Here is where things get kicked off in terms of a page request.
        /// Dependency injection is performed and events are subscribed to
        /// in the model which we use to manage session state.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            InjectDependencies();
            _Model.PropertyChanged += Model_PropertyChanged;
            base.OnPreInit(e);
        }

        /// <summary>
        /// Here we get the LendingTreeAffiliateRequest back from the Session if
        /// it is there.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            string sessionKey = SessionKeys.QuickMatchPrefix + _Model.DataPropertyName;
            if (Session[sessionKey] != null)
            {
                _Model.Data = (LendingTreeAffiliateRequest)Session[sessionKey];
            }
            base.OnLoad(e);
        }

        /// <summary>
        /// When a model property is changed this event stores the value in the session.
        /// 
        /// NOTE: The Data property contains the actual loan application object that will be 
        /// serialized to an XML POST.  By keeping the entire Data object in session, we do 
        /// not need to store values of other most other properties, as it would be
        /// redundant.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="e"></param>
        protected void Model_PropertyChanged(object model, PropertyChangedEventArgs e)
        {
            string propertyName = e.PropertyName;
            var propertiesToSave = new string[] 
            { 
                _Model.DataPropertyName, 
                SessionKeys.AppID
            };
            if (propertiesToSave.Contains(propertyName))
            {
                Session[SessionKeys.QuickMatchPrefix + propertyName] = (model as ILendingTreeModel)[propertyName];
            }
        }

        /// <summary>
        /// Here we are using the "object builder" functionality of the Unity container.
        /// Since instances of a page class are created by the ASP.NET runtime, we obtain
        /// DI on existing objects by passing "this" into the "BuildUp" method.
        /// </summary>
        void InjectDependencies()
        {
            var context = HttpContext.Current;
            if (context == null)
            {
                return;
            }
            var accessor = context.ApplicationInstance as IContainerAccessor;
            if (accessor == null)
            {
                return;
            }
            var container = accessor.Container;
            if (container == null)
            {
                throw new InvalidOperationException("No Unity container found");
            }
            container.BuildUp(this.GetType(), this, this.GetType().FullName);
        }
    }
}
