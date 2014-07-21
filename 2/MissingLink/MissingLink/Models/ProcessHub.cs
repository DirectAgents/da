using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;

namespace MvcApplication1.Models
{
    /**
     * The model ProcessHub serves as the central point for all of the computing that takes place in the software.
     **/
    public class ProcessHub
    {
        /**
         * The container class SearchResult is integrated into ProcessHub to minimize clutter.
         **/
        public class SearchResult
        {
            public string url, errorMsg;
            public bool linksToTarget, containsPhrase, skip, exception;
            public int id;

            public SearchResult(int num, string param_url)
            {
                url = param_url;
                id = num;
                linksToTarget = false;
                containsPhrase = false;
                skip = false;
                exception = false;
                errorMsg = "";
            }
        } // SearchResult class

        public Stopwatch watch;

        public float delay_time_display { get; set; }
        public float total_time { get; set; }
        public string query { get; set; }
        public string searchString { get; set; }
        public string client_site { get; set; }
        public List<string> target_website { get; set; }
        public string target_website_htmlview { get; set; }
        public int limit { get; set; }
        public int timer { get; set; }
        public int max_googleResults { get; set; }
        public bool exclude { get; set; }
        public bool phraseSearch { get; set; }
        public List<SearchResult> results { get; set; }
        public List<SearchResult> results_excluded { get; set; }

        private static string url;

        /**
         *
         **/
        private List<string> FormatLinks(string[] sites) {
            List<string> bin = new List<string>();
            for (int i = 0; i < sites.Length; i++) {

                string[] breakdown = sites[i].Split('.');

                if (breakdown.Length <= 1) {                            // a single name was provided, nothing more; very vague; default to http[s]://www.[address].com
                    bin.Add("href=\"http://" + sites[i] + ".com");
                    bin.Add("href=\"https://" + sites[i] + ".com");
                    bin.Add("href=\"http://www." + sites[i] + ".com");
                    bin.Add("href=\"https://www." + sites[i] + ".com");
                }
                else
                {
                    if ((sites[i].Substring(0, 4)).Equals("http"))       // assume that exact link is provided as needed; can be either http or https
                    {
                        bin.Add("href=\"" + sites[i]);
                    }
                    else if ((sites[i].Substring(0, 4)).Equals("www."))     // part link provided; must add protocol
                    {
                        bin.Add("href=\"http://" + sites[i]);
                        bin.Add("href=\"https://" + sites[i]);
                    }
                    else {                                                  // just sitename.com provided
                        bin.Add("href=\"http://" + sites[i]);
                        bin.Add("href=\"https://" + sites[i]);
                        bin.Add("href=\"http://www." + sites[i]);
                        bin.Add("href=\"https://www." + sites[i]);
                    }
                }
            }
            return bin;
        } // FormatLink

        private bool blankSpot(string[] s) {
            bool b = false;
            for (int i = 0; i < s.Length; i++)
                if (s[i].Equals("") || s[i] == null) b = true;
            return b;
        } // blankSpot

        /**
         * Main Constructor for ProcessHub.
         * @param   param_query: search query to be used via Google
         *          param_searchString: phrase to be searched for
         *          param_website: website(s)
         **/
        public ProcessHub(string param_query, string param_searchString, string param_website, int param_numResults, string param_exclude, float param_delay)
        {
            // Parsing list of websites to target
            target_website = new List<string>();
            target_website_htmlview = param_website;
            results = new List<SearchResult>();
            results_excluded = new List<SearchResult>();
            if (param_website.Equals("") || param_website == null)
                target_website.Add("");
            else
            {

                target_website = FormatLinks(param_website.Split(' '));
                //foreach (string z in target_website)
                //    displayln(z);
            }

            // Setting target site
            client_site = param_website;

            // Setting Google search query
            query = "";
            if (param_query != null) query = param_query;

            // Setting phrase search query
            searchString = "";
            phraseSearch = true;
            if (param_searchString == null || param_searchString == "")
                phraseSearch = false;
            else searchString = param_searchString;

            // Setting limit on number of Google results pages
            limit = 1;
            if (param_numResults > 1) limit = param_numResults;

            // Setting config option on exclusion of positive results
            exclude = false;
            if (param_exclude.Equals("yes")) exclude = true;

            // Setting timer; input in seconds
            timer = 0;
            if (param_delay < 0)
            {
                timer = 0;
                delay_time_display = 0.0F;
            }
            else
            {
                timer = (int)(param_delay * 1000);
                delay_time_display = param_delay;
            }

            // Other important variables
            url = "https://ajax.googleapis.com/ajax/services/search/web?v=1.0&q=" + HttpUtility.UrlEncode(query);
        } // primary constructor

        /**
         * The driver method of ProcessHub. The run() method retrieves search results from Google in the form of JSON data,
         * and parses it via JSON.NET, a third-party addon. The Google Search API only allows for four search results at a time.
         * A delay timer is implemented between pages to simulate a human user.
         **/
        public void run()
        {
            watch = new Stopwatch();
            watch.Start();
            // Processing Google search results. Page is parsed from JSON string.
            bool complete = false;
            int count = 0;
            int furtherSearchValue = 0;
            int pages = limit / 4;
            if ((limit%4) > 0) pages++;

            for (int i = 0; i < pages; i++)
            {
                string temp = (new WebClient()).DownloadString(url);
                dynamic parsed = Newtonsoft.Json.JsonConvert.DeserializeObject(temp);
                max_googleResults = Convert.ToInt32(parsed["responseData"]["cursor"]["estimatedResultCount"]);
                for (int j = 0; j < 4; j++)
                {
                    string address = parsed["responseData"]["results"][j]["url"];
                    SearchResult r = scrapeURL(count+1, address, target_website, exclude, searchString);
                    results.Add(r);
                    count++;
                    if (count == limit)
                    {
                        complete = true;
                        break;
                    }
                }
                if (complete) break;
                furtherSearchValue += 4;
                url = "http://ajax.googleapis.com/ajax/services/search/web?v=1.0&start=" + furtherSearchValue + "&q=" + HttpUtility.UrlEncode(query);
                Thread.Sleep(timer);
            }

            TrimExclusions(results);
            watch.Stop();
            displayln(Convert.ToString(watch.ElapsedMilliseconds));
            total_time = (float)watch.ElapsedMilliseconds / 1000;
            //DiagnosticPrint(results);
        } // run

        /**
         * Results that are to be skipped over are referenced in a separate list.
         * Previous iteration completely removed the SearchResult object from the results
         * list, but this feature was removed to retain references to the omitted results
         * for future use.
         * @para results:   A list of SearchResult objects
         **/
        private void TrimExclusions(List<SearchResult> results)
        {
            SearchResult[] results_arr = results.ToArray();
            for (int i = 0; i < results_arr.Length; i++)
            {
                if (results_arr[i].skip)
                    results_excluded.Add(results_arr[i]);
            }
        } // TrimExclusions

        /**
         * Retrieves the page data from a given webpage in string form, and prepares it for computing
         * by escaping specific chars. The string is then examined for links that lead back to the target
         * website(s), and instances of the given phrase string.
         **/
        private SearchResult scrapeURL(int num, string link, List<string> target_website, bool exclude, string searchString)
        {
            SearchResult sr = new SearchResult(num,link);
            try
            {
                string pageData = (new WebClient()).DownloadString(link);
                pageData.Replace('"', '\"');
                foreach (string s in target_website)
                {
                    if (pageData.Contains(s))
                    {
                        sr.linksToTarget = true;
                        if (exclude) sr.skip = true;
                    }
                }

                bool contains = pageData.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0;
                if (contains)        // original condition: (pageData.Contains(searchString)
                    sr.containsPhrase = true;

                return sr;
            }
            catch (Exception e)
            {
                displayln(e.ToString());
                sr.exception = true;
                sr.errorMsg = e.ToString();
                return sr;
            }
        } // scrapeURL

        private void DiagnosticPrint(List<SearchResult> results)
        {
            foreach (SearchResult sr in results)
            {

                if (sr.skip) continue;

                displayln(sr.url);
                if (sr.exception)
                {
                    displayln(sr.errorMsg);
                    continue;
                }
                if (sr.linksToTarget) displayln("OKAY: Site features links that point to target(s).");
                else displayln("NOT: No links to target(s) found.");

                if (sr.containsPhrase) displayln("OKAY: Site contains instances of " + searchString);
                else displayln("NOT: No instances of " + searchString + " found.");

                displayln("");
            }
        } // DiagnosticPrint

        private void display(string s)
        {
            System.Diagnostics.Debug.Write(s);
        } // display

        private void displayln(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        } // displayln

    }



}