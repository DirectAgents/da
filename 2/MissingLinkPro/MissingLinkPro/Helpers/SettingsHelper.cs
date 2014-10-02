using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MissingLinkPro.Helpers
{
    public static class SettingsHelper
    {
        private static Setting RetrieveSetting(string settingName) {
            Setting setting;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var settings = from s in db.Settings
                               where s.SettingName.Equals(settingName)
                               select s;
                setting = settings.FirstOrDefault();
            }
            return setting;
        } // RetrieveDailyLimitSetting

        public static int? RetrieveDailyLimitSetting()
        {
            var setting = RetrieveSetting("Daily Limit");
            if (setting != null)
                return Convert.ToInt32(setting.Value);
            else
                return null;
        }

    } // SettingsHelper
}