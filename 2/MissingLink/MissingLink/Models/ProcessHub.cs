using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Data.Services.Client;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using System.IO;

namespace MvcApplication1.Models
{
    /**
     * The model ProcessHub serves as the central point for all of the computing that takes place in the software.
     **/
    public class ProcessHub
    {

        private class MyWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest w = base.GetWebRequest(uri);
                w.Timeout = 8000;
                return w;
            }
        }

        /**
         * The container class SearchResult is integrated into ProcessHub to minimize clutter.
         **/
        public class SearchResult
        {
            public string url, errorMsg, title;
            public bool linksToTarget, containsPhrase, skip, exception, scraped;
            public int id;

            // For Bing
            public SearchResult(int num, string param_title, string param_url)
            {
                title = param_title;
                url = param_url;
                id = num;
                linksToTarget = false;
                containsPhrase = false;
                skip = false;
                exception = false;
                errorMsg = "";
                scraped = false;
            }

            public SearchResult() {
                linksToTarget = false;
                containsPhrase = false;
                skip = false;
                exception = false;
                errorMsg = "";
                scraped = false;
            }

        } // SearchResult class

        public bool search_error_encountered { get; set; }
        public string search_error_msg { get; set; }

        public float delay_time_display { get; set; }
        public float total_time { get; set; }
        public string query { get; set; }
        public string searchString { get; set; }
        public string client_site { get; set; }
        public List<string> target_website { get; set; }
        public string target_website_htmlview { get; set; }
        public int top { get; set; }
        public int timer { get; set; }
        public int max_googleResults { get; set; }
        public bool exclude { get; set; }
        public bool phraseSearch { get; set; }
        public List<SearchResult> results { get; set; }
        public int skip { get; set; }
        public int omit_count { get; set; }
        public bool display_all { get; set; }
        // Bing-related
        private const string AccountKey = "cU1u2NEkgkFY33IvcQxwngH38LyfutFTKY2tkQYq8ps";
        private int indices, threadsComplete, threadsRunning;
        private bool hubLock;

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
                    if ((sites[i].Substring(0, 7)).Equals("http://"))       // remove procotol prefix http
                        sites[i] = sites[i].Substring(7, sites[i].Length - 7);
                    else if ((sites[i].Substring(0, 8)).Equals("https://")) // remove protocol prefix https
                        sites[i] = sites[i].Substring(8, sites[i].Length - 8); 


                    if ((sites[i].Substring(0, 4)).Equals("www."))     // part link provided; must add protocol
                    {
                        bin.Add("href=\"http://" + sites[i]);
                        bin.Add("href=\"https://" + sites[i]);
                        
                        bin.Add("href=\"http://" + sites[i].Substring(4, sites[i].Length - 4));
                        bin.Add("href=\"https://" + sites[i].Substring(4, sites[i].Length - 4));

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
            for (int i = 0; i < bin.Count; i++)
                displayln(bin[i]);
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
        public ProcessHub (ParameterKeeper incoming)
        {
            // Parsing list of websites to target
            target_website = new List<string>();
            target_website_htmlview = incoming.website;
            results = new List<SearchResult>();
            if (incoming.website.Equals("") || incoming.website == null)
                target_website.Add("");
            else
                target_website = FormatLinks(incoming.website.Split(' '));

            // Setting target site
            client_site = incoming.website;

            // Setting Google search query
            query = "";
            if (incoming.query != null) query = incoming.query;

            // Setting phrase search query
            searchString = "";
            phraseSearch = true;
            if (incoming.searchString == null || incoming.searchString == "")
                phraseSearch = false;
            else searchString = incoming.searchString;

            // Setting limit on number of Google results pages
            top = 1;
            if (incoming.top > 1) top = incoming.top;
            indices = top;

            // Setting config option on exclusion of positive results
            exclude = false;
            if (incoming.exclude.Equals("yes")) exclude = true;

            // Display settings
            display_all = false;
            if (incoming.displayall.Equals("yes")) display_all = true;

            // Setting jump point
            skip = incoming.skip - 1;

            // Setting timer; input in seconds
            timer = 0;
            if (incoming.delay < 0)
            {
                timer = 0;
                delay_time_display = 0.0F;
            }
            else
            {
                timer = (int)(incoming.delay * 1000);
                delay_time_display = incoming.delay;
            }



            // Other important variables
            omit_count = 0;
            hubLock = true;
        } // primary constructor

        /**
         * The driver method of ProcessHub. The run() method retrieves search results from Google in the form of JSON data,
         * and parses it via JSON.NET, a third-party addon. The Google Search API only allows for four search results at a time.
         * A delay timer is implemented between pages to simulate a human user.
         **/
        public void run()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            search_error_encountered = false;
            bool complete = false;
            int count = 0;
            int pages = top / 50;
            if ((top % 50) > 0) pages++;

            string rootUrl = "https://api.datamarket.azure.com/Bing/Search";
            var bingContainer = new Bing.BingSearchContainer(new Uri(rootUrl));
            string market = "en-us";
            bingContainer.Credentials = new NetworkCredential(AccountKey, AccountKey);

            for (int i = 0; i < pages; i++)
            {
               var webQuery = bingContainer.Web(query, null, null, market, null, null, null, null);
                if (top < 50)
                    webQuery = webQuery.AddQueryOption("$top", top);
                else
                    webQuery = webQuery.AddQueryOption("$top", 50);
                webQuery = webQuery.AddQueryOption("$skip", skip);
                var webResults = webQuery.Execute();

                foreach (var result in webResults)
                {
                    results.Add(new SearchResult(skip + 1, result.Title, result.Url));
                    count++;
                    skip++;
                    if (count == top)
                    {
                        complete = true;
                        break;
                    }
                }

                if (complete) break;
            }
            /**NOTE: The beneath for-loop creates 1 thread per result, versus the 10 per result that is currently in place.**/

            //for (int i = 0; i < top; i++)
            //{
            //    Thread t = StartThread(i);
            //}

            threadsRunning = results.Count/10 ;
            if (results.Count % 10 > 0) threadsRunning++;
            threadsComplete = 0;

            int quota = results.Count;
            while (quota > 0)
            {
                int x, y;
                y = quota - 1;   // setting max index
                if (quota % 10 > 0)
                {
                    x = quota - (quota % 10);
                    quota -= quota % 10;
                }
                else
                {
                    quota -= 10;
                    x = quota;
                }
                Thread t = StartThread(x,y);
            }

            while (hubLock) { Thread.Sleep(100); }

            watch.Stop();
            displayln(Convert.ToString(watch.ElapsedMilliseconds));
            total_time = (float)watch.ElapsedMilliseconds / 1000;
            //DiagnosticPrint(results);
        }

        public Thread StartThread(int i)
        {
            var t = new Thread(() => scrapeURL(i));
            t.Start();
            return t;
        }

        public Thread StartThread(int x, int y)
        {
                var t = new Thread(() => ScrapeBatch(x,y));
                t.Start();
                return t;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void incrementOmitCount() {
            omit_count++;
        } // incrementOmitCount

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void checkLock() {
            threadsComplete++;
            if (threadsComplete >= threadsRunning) { hubLock = false; }
        }

        /**
         * Scrapes websites in multiple batches, as opposed to single pages.
         * Uses HttpWebRequest, along with specific settings that mimic a web browser.
         * The line
         *      if (response.StatusCode != HttpStatusCode.OK)
         * is questionable; w.GetResponse() can throw a WebException that bypasses the
         * if statement entirely.
         **/
        private void ScrapeBatch(int x, int y)
        {
            for (int i = x; i <= y; i++) {
                if (results[i].scraped) continue;
                try
                {
                    string pageData = "";
                    int status = 0;
                    HttpWebResponse response = null;
                    HttpWebRequest w = (HttpWebRequest)WebRequest.Create(results[i].url);
                    w.Timeout = 8000;
                    w.ContentLength = 0;
                    w.Method = WebRequestMethods.Http.Get;
                    CookieContainer cookieJar = new CookieContainer();
                    w.CookieContainer = cookieJar;
                    w.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:25.0) Gecko/20100101 Firefox/25.0";
                    response = (HttpWebResponse)w.GetResponse();
                    //displayln("Response " + response.StatusCode + ": " + response.StatusDescription);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        results[i].exception = true;
                        results[i].errorMsg = response.StatusDescription;
                        continue;
                    }
                    //Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default))
                    {
                        pageData = reader.ReadToEnd();
                    }
                    status = (int)response.StatusCode;

                    pageData.Replace('"', '\"');
                    foreach (string s in target_website)
                    {
                        if (pageData.Contains(s))
                        {
                            results[i].linksToTarget = true;
                            if (exclude)
                            {
                                results[i].skip = true;
                                incrementOmitCount();
                            }
                        }
                    }
                    bool contains = pageData.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0;
                    if (contains)
                    {
                        results[i].containsPhrase = true;
                    }
                }
                catch (System.Net.ProtocolViolationException e) {
                    results[i].exception = true;
                    results[i].errorMsg = "Protocol Violation: " + e.Message;
                }
                catch (System.Net.WebException e)
                {
                    HttpWebResponse res = (HttpWebResponse)e.Response;
                    results[i].exception = true;
                    if (res == null) results[i].errorMsg = e.Message;
                    else
                        results[i].errorMsg = "HTTP Status Code " + (int)res.StatusCode + ": " + res.StatusDescription;
                }
                results[i].scraped = true;
            }
            checkLock();
        } // ScrapeBatch

        /**
         * Retrieves the page data from a given webpage in string form, and prepares it for computing
         * by escaping specific chars. The string is then examined for links that lead back to the target
         * website(s), and instances of the given phrase string.
         * NOTE: Uses the old MyWebClient class to grab data; see the code used in ScrapeBatch().
         **/
        private void scrapeURL(int i)
        {
            try
            {
                MyWebClient w = new MyWebClient();
                w.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:25.0) Gecko/20100101 Firefox/25.0");
                string pageData = w.DownloadString(results[i].url);
                pageData.Replace('"', '\"');
                foreach (string s in target_website)
                {
                    if (pageData.Contains(s))
                    {
                        results[i].linksToTarget = true;
                        if (exclude)
                        {
                            results[i].skip = true;
                            incrementOmitCount();
                        }
                    }
                }
                bool contains = pageData.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0;
                if (contains)
                    results[i].containsPhrase = true;
            }
            catch (System.Net.WebException e)
            {
                HttpWebResponse res = (HttpWebResponse)e.Response;
                results[i].exception = true;
                if (res == null) results[i].errorMsg = e.Message;
                else
                    results[i].errorMsg = "HTTP Status Code " + (int)res.StatusCode + ": " + res.StatusDescription;
            }
            checkLock();
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