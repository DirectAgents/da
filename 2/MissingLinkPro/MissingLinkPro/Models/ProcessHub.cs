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
using System.Configuration;

namespace MissingLinkPro.Models
{
    /**
     * The model ProcessHub serves as the central point for all of the computing that takes place in the software.
     **/
    public class ProcessHub
    {
        /**
         * Class established specifically to allow overriding of HTML request timeout duration.
         **/
        private class MyWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest w = base.GetWebRequest(uri);
                w.Timeout = 8000;
                return w;
            }
        } // class MyWebClient

        /**
         * The container class SearchResult is integrated into ProcessHub to minimize clutter.
         **/
        public class SearchResult
        {
            public string Url, ErrorMsg, Title;
            public bool LinksToClientWebsite, ContainsSearchPhrase, SkipThisResult, ExceptionFound, Scraped;
            public int Id;

            public string Source;
            public DateTime? Date;

            // For Bing
            public SearchResult(int num, string param_title, string param_url)
            {
                Title = param_title;
                Url = param_url;
                Id = num;
                LinksToClientWebsite = false;
                ContainsSearchPhrase = false;
                SkipThisResult = false;
                ExceptionFound = false;
                ErrorMsg = "";
                Scraped = false;
            }

            // For News Results specifically
            public SearchResult(int num, string param_title, string param_url, string param_source, DateTime? param_date)
            {
                Title = param_title;
                Url = param_url;
                Id = num;
                LinksToClientWebsite = false;
                ContainsSearchPhrase = false;
                SkipThisResult = false;
                ExceptionFound = false;
                ErrorMsg = "";
                Scraped = false;
                Source = param_source;
                Date = param_date;
            }

            public SearchResult() {
                LinksToClientWebsite = false;
                ContainsSearchPhrase = false;
                SkipThisResult = false;
                ExceptionFound = false;
                ErrorMsg = "";
                Scraped = false;
            }

        } // SearchResult class

        public bool SearchErrorEncountered { get; set; }
        public string SearchErrorMsg { get; set; }
        public float TotalRunTime { get; set; }

        // Form Field Parameters
        public string BingSearchQuery { get; set; }
        public string PhraseSearchString { get; set; }
        public string ExcludeString { get; set; }
        public string ClientWebsite { get; set; }
        public bool ExcludeLinkbackResults { get; set; }
        public bool DisplayAllResults { get; set; }
        public string ResultType { get; set; }
        public int top { get; set; }
        public int skip { get; set; }

        // Parsed informational variables
        public List<string> ClientWebsiteParsed { get; set; }
        public bool PhraseSearchEnabled { get; set; }
        public bool ExcludeEnabled { get; set; }
        public List<SearchResult> ParsedResults { get; set; }
        public int OmitCount { get; set; }

        // Bing-related
        private string AccountKey = ConfigurationManager.AppSettings["BingAPIKey"];

        // Multi-threading vars
        private int ThreadsComplete, ThreadsRunning;
        private bool HubLock;

        /**
         * Formats website entries as provided by the user. Method automatically
         * attaches protocols, prefixes, and suffixes where needed.
         * @para string[] sites: array of strings representing URLs
         **/
        private List<string> FormatLinks(string[] websites)
        {
            List<string> temp = new List<string>();
            for (int i = 0; i < websites.Length; i++)
            {

                string[] breakdown = websites[i].Split('.');

                if (breakdown.Length <= 1)
                {                            // a single name was provided, nothing more; very vague; default to http[s]://www.[address].com
                    temp.Add("href=\"http://" + websites[i] + ".com");
                    temp.Add("href=\"https://" + websites[i] + ".com");
                    temp.Add("href=\"http://www." + websites[i] + ".com");
                    temp.Add("href=\"https://www." + websites[i] + ".com");
                }
                else
                {
                    if ((websites[i].Substring(0, 7)).Equals("http://"))       // remove procotol prefix http
                        websites[i] = websites[i].Substring(7, websites[i].Length - 7);
                    else if ((websites[i].Substring(0, 8)).Equals("https://")) // remove protocol prefix https
                        websites[i] = websites[i].Substring(8, websites[i].Length - 8); 


                    if ((websites[i].Substring(0, 4)).Equals("www."))     // part link provided; must add protocol
                    {
                        temp.Add("href=\"http://" + websites[i]);
                        temp.Add("href=\"https://" + websites[i]);
                        
                        temp.Add("href=\"http://" + websites[i].Substring(4, websites[i].Length - 4));
                        temp.Add("href=\"https://" + websites[i].Substring(4, websites[i].Length - 4));

                    }
                    else
                    {                                                  // just sitename.com provided
                        temp.Add("href=\"http://" + websites[i]);
                        temp.Add("href=\"https://" + websites[i]);
                        temp.Add("href=\"http://www." + websites[i]);
                        temp.Add("href=\"https://www." + websites[i]);
                    }
                }
            }
            for (int i = 0; i < temp.Count; i++)
                displayln(temp[i]);
            return temp;
        } // FormatLink

        /**
         * Determines if there are any empty or null strings in an array of strings.
         **/
        private bool ContainsBlankSpot(string[] s)
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
            ClientWebsiteParsed = new List<string>();
            ClientWebsite = incoming.ClientWebsite;
            ParsedResults = new List<SearchResult>();
            if (incoming.ClientWebsite.Equals("") || incoming.ClientWebsite == null)
                ClientWebsiteParsed.Add("");
            else
                ClientWebsiteParsed = FormatLinks(incoming.ClientWebsite.Split(' '));

            // Setting Google search query
            BingSearchQuery = "";
            if (incoming.BingSearchQuery != null) BingSearchQuery = incoming.BingSearchQuery;

            // Setting phrase search query
            PhraseSearchString = "";
            PhraseSearchEnabled = true;
            if (incoming.PhraseSearchString == null || incoming.PhraseSearchString == "")
                PhraseSearchEnabled = false;
            else PhraseSearchString = incoming.PhraseSearchString;

            // Setting exclude string
            ExcludeString = "";
            ExcludeEnabled = true;
            if (incoming.ExcludeString == null || incoming.ExcludeString == "")
                ExcludeEnabled = false;
            else ExcludeString = incoming.ExcludeString;

            // Setting result type.
            ResultType = incoming.ResultType;

            // Setting limit on number of results per query
            top = 1;
            if (incoming.top > top) top = incoming.top;

            // Setting config option on exclusion of positive results
            ExcludeLinkbackResults = false;
            if (incoming.ExcludeLinkbackResults == true) ExcludeLinkbackResults = true;

            // Display settings
            DisplayAllResults = false;
            if (incoming.DisplayAllResults == true) DisplayAllResults = true;

            // Setting jump point
            skip = incoming.skip - 1;

            // Other important variables
            OmitCount = 0;
            HubLock = true;
        } // primary constructor

        /**
         * The driver method of ProcessHub. Performs search, and parses results.
         **/
        public void run()
        {

            Stopwatch watch = new Stopwatch();
            watch.Start();
            SearchErrorEncountered = false;

            string rootUrl = "https://api.datamarket.azure.com/Bing/Search";
            var bingContainer = new Bing.BingSearchContainer(new Uri(rootUrl));
            string market = "en-us";
            bingContainer.Credentials = new NetworkCredential(AccountKey, AccountKey);

            string QueryAttachment = "";
            if (ExcludeEnabled)
            {
                var ExcludeSplits = ExcludeString.Split(new string[] { "\" \"", "\"" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var split in ExcludeSplits)
                    QueryAttachment += (" -" + split);
            }
            try
            {
                if (ResultType.Equals("news"))
                {
                    int pages = top / 15;
                    if ((top % 15) > 0) pages++;
                    processNews(bingContainer, pages, BingSearchQuery + QueryAttachment, market);
                }
                else
                {
                    int pages = top / 50;
                    if ((top % 50) > 0) pages++;
                    processWeb(bingContainer, pages, BingSearchQuery + QueryAttachment, market);
                }
            }
                // This IOException typically occurs when the Bing request comes back with problems.
                catch (IOException e) {
                    SearchErrorEncountered = true;
                    SearchErrorMsg = e.Message;
                }
            /**NOTE: The beneath for-loop creates 1 thread per result, versus the 10 per result that is currently in place.**/

            //for (int i = 0; i < top; i++)
            //{
            //    Thread t = StartThread(i);
            //}

            int NumResultsPerThread = 5; // set number of threads here
            ThreadsRunning = ParsedResults.Count/NumResultsPerThread;
            if (ParsedResults.Count % NumResultsPerThread > 0) ThreadsRunning++;
            ThreadsComplete = 0;

            int quota = ParsedResults.Count;
            while (quota > 0)
            {
                int x, y;
                y = quota - 1;   // setting max index
                if (quota % NumResultsPerThread > 0)
                {
                    x = quota - (quota % NumResultsPerThread);
                    quota -= quota % NumResultsPerThread;
                }
                else
                {
                    quota -= NumResultsPerThread;
                    x = quota;
                }
                Thread t = StartThread(x,y);
            }

            while (HubLock) { Thread.Sleep(100); }

            watch.Stop();
            displayln(Convert.ToString(watch.ElapsedMilliseconds));
            TotalRunTime = (float)watch.ElapsedMilliseconds / 1000;
            //DiagnosticPrint(results);
        }

        private Uri AttachUriParameter(Uri link, string parameter)
        {
            UriBuilder builder = new UriBuilder(link);
            builder.Host += parameter;
            Uri result = builder.Uri;
            return result;
        } // AttachUriParameter

        private void processWeb(Bing.BingSearchContainer bingContainer, int pages, string query, string market)
        {
            int count = 0;
            bool complete = false;
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
                    ParsedResults.Add(new SearchResult(skip + 1, result.Title, result.Url));
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
        } // processWeb

        private void processNews(Bing.BingSearchContainer bingContainer, int pages, string query, string market)
        {
            int count = 0;
            bool complete = false;
            for (int i = 0; i < pages; i++)
            {
                var webQuery = bingContainer.News(query,null,market,null,null,null,null,null,null);
                if (top < 15)
                    webQuery = webQuery.AddQueryOption("$top", top);
                else
                    webQuery = webQuery.AddQueryOption("$top", 15);
                webQuery = webQuery.AddQueryOption("$skip", skip);

                var webResults = webQuery.Execute();

                foreach (var result in webResults)
                {
                    ParsedResults.Add(new SearchResult(skip + 1, result.Title, result.Url, result.Source, result.Date));
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
        } // processNews

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
            OmitCount++;
        } // incrementOmitCount

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void checkLock() {
            ThreadsComplete++;
            if (ThreadsComplete >= ThreadsRunning) { HubLock = false; }
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
                if (ParsedResults[i].Scraped) continue;
                try
                {
                    string pageData = "";
                    int status = 0;
                    HttpWebResponse response = null;
                    HttpWebRequest w = (HttpWebRequest)WebRequest.Create(ParsedResults[i].Url);
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
                        ParsedResults[i].ExceptionFound = true;
                        ParsedResults[i].ErrorMsg = response.StatusDescription;
                        continue;
                    }
                    //Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default))
                    {
                        pageData = reader.ReadToEnd();
                    }
                    status = (int)response.StatusCode;

                    pageData.Replace('"', '\"');
                    foreach (string s in ClientWebsiteParsed)
                    {
                        if (pageData.Contains(s))
                        {
                            ParsedResults[i].LinksToClientWebsite = true;
                            if (ExcludeLinkbackResults)
                            {
                                ParsedResults[i].SkipThisResult = true;
                                incrementOmitCount();
                            }
                        }
                    }
                    bool contains = pageData.IndexOf(PhraseSearchString, StringComparison.OrdinalIgnoreCase) >= 0;
                    if (contains)
                    {
                        ParsedResults[i].ContainsSearchPhrase = true;
                    }
                }
                catch (System.IO.IOException e) {
                    ParsedResults[i].ExceptionFound = true;
                    ParsedResults[i].ErrorMsg = "IO Exception: " + e.Message;
                }
                catch (System.Net.ProtocolViolationException e)
                {
                    ParsedResults[i].ExceptionFound = true;
                    ParsedResults[i].ErrorMsg = "Protocol Violation: " + e.Message;
                }
                catch (System.Net.WebException e)
                {
                    HttpWebResponse res = (HttpWebResponse)e.Response;
                    ParsedResults[i].ExceptionFound = true;
                    if (res == null) ParsedResults[i].ErrorMsg = e.Message;
                    else
                        ParsedResults[i].ErrorMsg = "HTTP Status Code " + (int)res.StatusCode + ": " + res.StatusDescription;
                }
                ParsedResults[i].Scraped = true;
            }
            checkLock();
        } // ScrapeBatch

        /**
         * Retrieves the page data from a given webpage in string form, and prepares it for computing
         * by escaping specific chars. The string is then examined for links that lead back to the target
         * website(s), and instances of the given phrase string.
         * NOTE: This method is outdated; it uses the old MyWebClient class to grab data; see the code used in ScrapeBatch().
         **/
        private void scrapeURL(int i)
        {
            try
            {
                MyWebClient w = new MyWebClient();
                w.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:25.0) Gecko/20100101 Firefox/25.0");
                string pageData = w.DownloadString(ParsedResults[i].Url);
                pageData.Replace('"', '\"');
                foreach (string s in ClientWebsiteParsed)
                {
                    if (pageData.Contains(s))
                    {
                        ParsedResults[i].LinksToClientWebsite = true;
                        if (ExcludeLinkbackResults)
                        {
                            ParsedResults[i].SkipThisResult = true;
                            incrementOmitCount();
                        }
                    }
                }
                bool contains = pageData.IndexOf(PhraseSearchString, StringComparison.OrdinalIgnoreCase) >= 0;
                if (contains)
                    ParsedResults[i].ContainsSearchPhrase = true;
            }
            catch (System.Net.WebException e)
            {
                HttpWebResponse res = (HttpWebResponse)e.Response;
                ParsedResults[i].ExceptionFound = true;
                if (res == null) ParsedResults[i].ErrorMsg = e.Message;
                else
                    ParsedResults[i].ErrorMsg = "HTTP Status Code " + (int)res.StatusCode + ": " + res.StatusDescription;
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

                if (sr.SkipThisResult) continue;

                displayln(sr.Url);
                if (sr.ExceptionFound)
                {
                    displayln(sr.ErrorMsg);
                    continue;
                }
                if (sr.LinksToClientWebsite) displayln("OKAY: Site features links that point to target(s).");
                else displayln("NOT: No links to target(s) found.");

                if (sr.ContainsSearchPhrase) displayln("OKAY: Site contains instances of " + PhraseSearchString);
                else displayln("NOT: No instances of " + PhraseSearchString + " found.");

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