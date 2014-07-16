using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    /**
     * Class object containing all relevant information for a given search result.
     **/
    public class SearchResult
    {
        public string url { get; set; }
        public bool linksToTarget { get; set; }
        public bool containsPhrase { get; set; }
        public bool skip { get; set; }
        public bool exception { get; set; }
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