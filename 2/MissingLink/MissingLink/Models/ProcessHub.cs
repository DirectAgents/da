﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public bool search_error_encountered { get; set; }
        public string search_error_msg { get; set; }

        public float delay_time_display { get; set; }
        public float total_time { get; set; }

        [Required]
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
        public int jump { get; set; }
        public int omit_count;

        private static string url;

        /**
         * Formats website entries as provided by the user. Method automatically
         * attaches protocols, prefixes, and suffixes where needed.
         * @para string[] sites: array of strings representing URLs
         **/
        private List<string> FormatLinks(string[] sites)
        {
            List<string> bin = new List<string>();
            for (int i = 0; i < sites.Length; i++)
            {

                string[] breakdown = sites[i].Split('.');

                if (breakdown.Length <= 1)
                {                            // a single name was provided, nothing more; very vague; default to http[s]://www.[address].com
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
                    else
                    {                                                  // just sitename.com provided
                        bin.Add("href=\"http://" + sites[i]);
                        bin.Add("href=\"https://" + sites[i]);
                        bin.Add("href=\"http://www." + sites[i]);
                        bin.Add("href=\"https://www." + sites[i]);
                    }
                }
            }
            return bin;
        } // FormatLink

        /**
         * Determines if there are any empty or null strings in an array of strings.
         **/
        private bool containsBlankSpot(string[] s)
        {
            bool b = false;
            for (int i = 0; i < s.Length; i++)
                if (s[i].Equals("") || s[i] == null) b = true;
            return b;
        } // containsBlankSpot

        /**
         * Main Constructor for ProcessHub.
         * @param   param_query: search query to be used via Google
         *          param_searchString: phrase to be searched for
         *          param_website: website(s)
         **/
        public ProcessHub(string param_query, string param_searchString, string param_website, int param_numResults, string param_exclude, int param_jump, float param_delay)
        {
            // Parsing list of websites to target
            target_website = new List<string>();
            target_website_htmlview = param_website;
            results = new List<SearchResult>();
            if (param_website.Equals("") || param_website == null)
                target_website.Add("");
            else
                target_website = FormatLinks(param_website.Split(' '));

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

            // Setting jump point
            jump = 0;
            if (param_jump < 1) jump = 0;
            else jump = param_jump - 1;

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
            omit_count = 0;
        } // primary constructor

        /**
         * The driver method of ProcessHub. The run() method retrieves search results from Google in the form of JSON data,
         * and parses it via JSON.NET, a third-party addon. The Google Search API only allows for four search results at a time.
         * A delay timer is implemented between pages to simulate a human user.
         **/
        public void run()
        {
            search_error_encountered = false;

            url = "http://ajax.googleapis.com/ajax/services/search/web?v=1.0&start=" + jump + "&q=" + HttpUtility.UrlEncode(query) + "&userip=" + "24.103.67.186";
            Stopwatch watch = new Stopwatch();
            watch.Start();
            // Processing Google search results. Page is parsed from JSON string.
            bool complete = false;
            int count = 0;
            int pages = limit / 4;
            if ((limit % 4) > 0) pages++;

            WebClient w = new WebClient();
            w.Headers.Add("Referer", "http://www.directagents.com");

            for (int i = 0; i < pages; i++)
            {
                string temp;
                dynamic parsed = "";
                try
                {
                    temp = w.DownloadString(url);
                    parsed = Newtonsoft.Json.JsonConvert.DeserializeObject(temp);
                    max_googleResults = Convert.ToInt32(parsed["responseData"]["cursor"]["estimatedResultCount"]);
                }
                catch (System.Net.WebException e1)
                {
                    HttpWebResponse res = (HttpWebResponse)e1.Response;
                    search_error_encountered = true;
                    search_error_msg = "HTTP Status Code " + (int)res.StatusCode + ": " + res.StatusDescription;
                    break;
                }
                catch (InvalidOperationException e2)
                {
                    search_error_encountered = true;
                    search_error_msg = String.Concat("HTTP Status Code ", parsed["responseStatus"], ": ", parsed["responseDetails"]);
                    break;
                }
                for (int j = 0; j < 4; j++)
                {
                    string address = parsed["responseData"]["results"][j]["url"];
                    SearchResult r = scrapeURL(jump + 1, address, target_website, exclude, searchString);
                    results.Add(r);
                    count++;
                    jump++;
                    if (count == limit)
                    {
                        complete = true;
                        break;
                    }
                }
                if (complete) break;
                url = "http://ajax.googleapis.com/ajax/services/search/web?v=1.0&start=" + jump + "&q=" + HttpUtility.UrlEncode(query) + "&userip=" + "24.103.67.186";
                Thread.Sleep(timer);
            }

            watch.Stop();
            displayln(Convert.ToString(watch.ElapsedMilliseconds));
            total_time = (float)watch.ElapsedMilliseconds / 1000;
            //DiagnosticPrint(results);
        } // run

        /**
         * Retrieves the page data from a given webpage in string form, and prepares it for computing
         * by escaping specific chars. The string is then examined for links that lead back to the target
         * website(s), and instances of the given phrase string.
         **/
        private SearchResult scrapeURL(int num, string link, List<string> target_website, bool exclude, string searchString)
        {
            SearchResult sr = new SearchResult(num, link);
            try
            {
                WebClient w = new WebClient();
                w.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:25.0) Gecko/20100101 Firefox/25.0");
                string pageData = w.DownloadString(link);
                pageData.Replace('"', '\"');
                foreach (string s in target_website)
                {
                    if (pageData.Contains(s))
                    {
                        sr.linksToTarget = true;
                        if (exclude)
                        {
                            sr.skip = true;
                            omit_count++;
                        }
                    }
                }
                bool contains = pageData.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0;
                if (contains)
                    sr.containsPhrase = true;
                return sr;
            }
            catch (System.Net.WebException e)
            {
                HttpWebResponse res = (HttpWebResponse)e.Response;
                sr.exception = true;
                sr.errorMsg = "HTTP Status Code " + (int)res.StatusCode + ": " + res.StatusDescription;
                return sr;
            }
        } // scrapeURL

        /**
         * Prints out all SearchResults information stored in a list
         * in the output console. Useful for testing purposes.
         * @para List<SearchResult> results:    list of SearchResults
         **/
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

        /**
         * Quick shortcut method for printing to the diagnostic console, sans new line.
         * @para string s:  the string to be printed
         **/
        private void display(string s)
        {
            System.Diagnostics.Debug.Write(s);
        } // display

        /**
         * Quick shortcut method for printing to the diagnostic console, with new line.
         * @para string s:  the string to be printed
         **/
        private void displayln(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        } // displayln

    } // public class ProcessHub
} // namespace