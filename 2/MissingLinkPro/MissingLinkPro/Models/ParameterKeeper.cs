using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using IdentitySample.Models;

namespace MissingLinkPro.Models
{

    public class ParameterKeeper
    {
        [Display(Name = "Search Query")]
        [Required(ErrorMessage = "Search Query cannot be left blank.")]
        public string BingSearchQuery { get; set; }

        [Display(Name = "Phrase Search")]
        public string PhraseSearchString { get; set; }

        [Display(Name = "Excluding")]
        public string ExcludeString { get; set; }
        
        [Required(ErrorMessage = "You must provide at least one website to seek out.")]
        public string ClientWebsite { get; set; }
        
        [Range(1, 1000, ErrorMessage = "Number of results must be between 1 and 1000.")]
        public int top { get; set; }

        public int PingTime { get; set; }
        public int LoadTime { get; set; }

        public string ResultType { get; set; }
        public bool ExcludeLinkbackResults { get; set; }
        public bool DisplayAllResults { get; set; }

        [Range(1, 1001, ErrorMessage = "Can only jump between Rank IDs 1 and 1001.")]
        public int skip { get; set; }

        public int InitialSkip { get; set; }

        public List<MissingLinkPro.Models.ProcessHub.SearchResult> ParsedResults { get; set; }
        public int OmitCount { get; set; }
        public int MaxResultRange { get; set; }
        public bool OmitLinkbackResultsEnabled { get; set; }
        public bool SearchErrorEncountered { get; set; }
        public string SearchErrorMsg { get; set; }

        public ParameterKeeper()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Setting PingTimeSetting = db.Settings.SingleOrDefault(setting => setting.SettingName == "PingTime");
            Setting LoadTimeSetting = db.Settings.SingleOrDefault(setting => setting.SettingName == "LoadTime");
            top = 50;
                skip = 1;
                ExcludeLinkbackResults = true;
                DisplayAllResults = true;
                ResultType = "news";
                MaxResultRange = 1000;
                InitialSkip = 1;
            if (PingTimeSetting == null || LoadTimeSetting == null)
            {
                PingTime = 8000;
                LoadTime = 15000;
            }
            else
            {
                PingTime = Convert.ToInt32(PingTimeSetting.Value);
                LoadTime = Convert.ToInt32(LoadTimeSetting.Value);
            }
        }

    }
}