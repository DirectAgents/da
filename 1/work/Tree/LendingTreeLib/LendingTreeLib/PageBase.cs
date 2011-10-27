using System;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using LendingTreeLib.Common;
using LendingTreeLib.Schemas;
using Microsoft.Practices.Unity;

namespace LendingTreeLib
{
    public abstract class PageBase : Page
    {
        [Dependency]
        public LendingTreeModel _Model { set; get; }

        [Dependency]
        public ILogger Logger { get; set; }

        [QueryString]
        public string Debug { get; set; }

        public bool IsAdmin
        {
            get
            {
                string requestIP = Request.UserHostAddress;
                string adminIPs = WebConfigurationManager.AppSettings["AdminIps"];
                bool result = adminIPs.Contains(requestIP);
                return result;
            }
        }

        public ILendingTreeModel Model
        {
            get
            {
                return (ILendingTreeModel)_Model;
            }
        }

        public T SessionValue<T>(string key)
        {
            object sessionValue = Session[key];
            return (sessionValue == null) ? default(T) : (T)sessionValue;
        }

        public bool IsSessionValueTrue(string key)
        {
            string sessionValue = SessionValue<string>(key);
            bool result = (sessionValue != null) && (sessionValue == SessionKeys.True);
            return result;
        }

        public bool IsSessionValueEnabled(string key)
        {
            bool result = IsSessionValueTrue(key + SessionKeys.EnabledSuffix);
            return result;
        }

        public void EnableSessionValue(string key)
        {
            Session[key + SessionKeys.EnabledSuffix] = SessionKeys.True;
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
        /// Get LendingTreeAffiliateRequest from the Session if it exists.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            ProcessQueryStringAttributes();

            if (this.Debug == SessionKeys.True) 
                EnableSessionValue(SessionKeys.AutomationKey);

            string sessionKey = SessionKeys.QuickMatchPrefix + _Model.DataPropertyName;
            if (Session[sessionKey] != null) 
                _Model.Data = (LendingTreeAffiliateRequest)Session[sessionKey];

            base.OnLoad(e);
        }

        void ProcessQueryStringAttributes()
        {
            foreach (var pi in this.GetType().GetProperties())
            {
                var attr = (QueryStringAttribute)Attribute.GetCustomAttribute(pi, typeof(QueryStringAttribute));

                if (attr != null)
                {
                    string set = Request.QueryString[attr.ParameterName ?? pi.Name];

                    if (set != null)
                        pi.SetValue(this, set, null);
                    else if (attr.DefaultValue != null)
                        pi.SetValue(this, attr.DefaultValue, null);
                }
            }
        }

        /// <summary>
        /// When a model property is changed this event stores the value in the session.
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
        /// Object builder functionality of the Unity container.
        /// </summary>
        void InjectDependencies()
        {
            var context = HttpContext.Current;
            if (context != null)
            {
                var accessor = context.ApplicationInstance as IContainerAccessor;
                if (accessor != null)
                {
                    var container = accessor.Container;
                    if (container != null)
                    {
                        container.BuildUp(this.GetType(), this, this.GetType().FullName);
                    }
                    else
                        throw new InvalidOperationException("No Unity container found");
                }
            }
        }

        /// <summary>
        /// Service locator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return (T)(Global.Container.Resolve(typeof(T)));
        }
    }
}
