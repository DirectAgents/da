using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MissingLinkPro.Models
{
    /**
     * Class object containing all relevant information for a given search result.
     **/
    public class SearchResult
    {
        [Display(Name = "URL")]
        public string url { get; set; }

        [Display(Name = "Links To Target")]
        public bool linksToTarget { get; set; }

        [Display(Name = "Contains Phrase")]
        public bool containsPhrase { get; set; }

        [Display(Name = "Exclude Positive Results")]
        public bool skip { get; set; }

        public bool exception { get; set; }

        [Display(Name = "Error Message")]
        public string errorMsg { get; set; }

        public SearchResult(string param_url) {
            url = param_url;
            linksToTarget = false;
            containsPhrase = false;
            skip = false;
            exception = false;
            errorMsg = "";
        }

    }
}