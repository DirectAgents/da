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
using System.Text.RegularExpressions;
using BingWebOnly;
using HtmlAgilityPack;
using MissingLinkPro.Helpers;
using System.Threading.Tasks;
namespace MissingLinkPro.Models
{
    /**
    * The model ProcessHub serves as the central engine for all of the computing that takes place in the software.
    **/
    public class ProcessHub
    {
        public class GZipWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.Timeout = 8000;
                request.Proxy = null;
                return request;
            }
        }
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
            public SearchResult()
            {
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
        public int MaxResultRange { get; set; }
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
        public int InitialSkip { get; set; }
        // Parsed informational variables
        public List<string> ClientWebsiteParsed { get; set; }
        public bool PhraseSearchEnabled { get; set; }
        public bool ExcludeEnabled { get; set; }
        public List<SearchResult> ParsedResults { get; set; }
        public int OmitCount { get; set; }
        public string RegexString { get; set; }
        // Bing-related
        private string AccountKey = ConfigurationManager.AppSettings["BingAPIKey"];
        // Multi-threading vars
        private int ThreadsComplete, ThreadsRunning;
        private bool HubLock;
        // Systematic vars
        public int PingTime { get; set; }
        public int LoadTime { get; set; }
        /**
        * Formats website entries as provided by the user.
        * Forms a regex string based on the provided URL string.
        * @para string[] sites: array of strings representing URLs
        **/
        private List<string> FormatLinks(string[] websites)
        {
            List<string> temp = new List<string>();
            for (int i = 0; i < websites.Length; i++)
            {
                if (websites[i].Length >= 7 && websites[i].Substring(0, 7).Equals("http://"))
                    websites[i] = websites[i].Substring(7, websites[i].Length - 7);
                else if (websites[i].Length >= 8 && websites[i].Substring(0, 8).Equals("https://"))
                    websites[i] = websites[i].Substring(8, websites[i].Length - 8);
                string[] breakdown = websites[i].Split('.');
                if (breakdown.Length <= 1)   // a single name was provided, nothing more; very vague.
                {
                    
                    // old string: "\b(href=)(""|')((http|https)(://))?((([0-9]*[a-zA-Z]*))*\.)?"
                    RegexString = @"\b(href=)(""|')((http|https)(://))?(([a-zA-Z]|[0-9])*\.)?" + @breakdown[0] + @"\.[a-zA-Z]{2,4}";
                    temp.Add(RegexString);
                }
                else if (breakdown.Length >= 3) // Example: "www.capcom.com" or "www.capcom.co.jp"
                {
                    RegexString = @"\b(href=)(""|')((http|https)(://))?(([a-zA-Z]|[0-9])*\.)?";
                    RegexString += @"(";
                    for (int j = 0; j < breakdown.Length; j++)
                    {
                        if (breakdown[j].Equals("www") && (j == 0)) continue;      // We'll ignore "www" prefix in order to widen regex match range; remove this line for a more precise match.
                        RegexString = RegexString + breakdown[j];
                        if (j < breakdown.Length - 1) RegexString += @"\.";
                    }
                    RegexString += @")";
                    temp.Add(RegexString);
                }
                else // breakdown length is 2, which fits format "example.com"
                {
                    RegexString = @"\b(href=)(""|')((http|https)(://))?(([a-zA-Z]|[0-9])*\.)?(" + @breakdown[0] + @"\." + @breakdown[1] + ")";
                    temp.Add(RegexString);
                }
            }
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
        public ProcessHub() { } // Empty constructor
        /**
        * Main Constructor for ProcessHub.
        * @param param_query: search query to be used via Google
        * param_searchString: phrase to be searched for
        * param_website: website(s)
        **/
        public ProcessHub(ParameterKeeper incoming)
        {
            PingTime = incoming.LoadTime;
            LoadTime = incoming.PingTime;
            ClientWebsiteParsed = new List<string>(); // Parsing list of websites to target
            ClientWebsite = incoming.ClientWebsite;
            ParsedResults = new List<SearchResult>();
            if (incoming.ClientWebsite.Equals("") || incoming.ClientWebsite == null)
                ClientWebsiteParsed.Add("");
            else
                ClientWebsiteParsed = FormatLinks(incoming.ClientWebsite.Split(' '));
            BingSearchQuery = ""; // Setting Google search query
            if (incoming.BingSearchQuery != null) BingSearchQuery = incoming.BingSearchQuery;
            PhraseSearchString = ""; // Setting phrase search query
            PhraseSearchEnabled = true;
            if (incoming.PhraseSearchString == null || incoming.PhraseSearchString == "")
                PhraseSearchEnabled = false;
            else PhraseSearchString = incoming.PhraseSearchString;
            ExcludeString = ""; // Setting exclude string
            ExcludeEnabled = true;
            if (incoming.ExcludeString == null || incoming.ExcludeString == "")
                ExcludeEnabled = false;
            else ExcludeString = incoming.ExcludeString;
            ResultType = incoming.ResultType; // Setting result type.
            top = 1; // Setting limit on number of results per query
            if (incoming.top > top) top = incoming.top;
            ExcludeLinkbackResults = false; // Setting config option on exclusion of positive results
            if (incoming.ExcludeLinkbackResults == true) ExcludeLinkbackResults = true;
            DisplayAllResults = false; // Display settings
            if (incoming.DisplayAllResults == true) DisplayAllResults = true;
            skip = incoming.skip - 1; // Setting jump point
            MaxResultRange = incoming.MaxResultRange; // Setting maximum allowable results remaining
            OmitCount = 0; // Other important variables
            InitialSkip = incoming.InitialSkip - 1;
            HubLock = true; // While-loop lock
        } // primary constructor
        /**
        * The driver method of ProcessHub. Performs search, and parses results.
        **/
        public void run()
        {
            DebugHelper.displayln("Begin run");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            SearchErrorEncountered = false;
            string market = "en-us";
            string QueryAttachment = "";
            if (ExcludeEnabled) // if user chose to exclude search terms, add their terms to search query
            {
                var ExcludeSplits = ExcludeString.Split(new string[] { "\" \"", "\"" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var split in ExcludeSplits)
                    QueryAttachment += (" -" + split);
            }
            try
            {
                if (skip >= 1000) // Check to see if next set of results will go beyond 1000.
                {
                    if (MaxResultRange <= 0) // Maxed out all possible results that Bing can retrieve.
                    {
                        SearchErrorEncountered = true; // Flag prevents Process() from incrementing user's QueriesPerformed field.
                        SearchErrorMsg = "You've retrieved all of the possible results that the search engine was able to produce.";
                        return;
                    }
                    top = MaxResultRange;
                    skip = 1000 - MaxResultRange;
                }
                if (ResultType.Equals("news"))
                {
                    string rootUrl = "https://api.datamarket.azure.com/Bing/Search";
                    var bingContainer = new Bing.BingSearchContainer(new Uri(rootUrl));
                    bingContainer.Credentials = new NetworkCredential(AccountKey, AccountKey);
                    int pages = top / 15;
                    if ((top % 15) > 0) pages++;
                    processNews(bingContainer, pages, BingSearchQuery + QueryAttachment, market);
                }
                else if (ResultType.Equals("web"))
                {
                    string rootUrl = "https://api.datamarket.azure.com/Bing/SearchWeb/";
                    var bingContainerWebOnly = new BingWebOnly.BingSearchContainer(new Uri(rootUrl));
                    bingContainerWebOnly.Credentials = new NetworkCredential(AccountKey, AccountKey);
                    int pages = top / 50;
                    if ((top % 50) > 0) pages++;
                    processWeb(bingContainerWebOnly, pages, BingSearchQuery + QueryAttachment, market);
                }
            }
            catch (System.Data.Services.Client.DataServiceQueryException e)
            {
                SearchErrorEncountered = true;
                SearchErrorMsg = e.Message;
            }
            //for (int i = 0; i < top; i++) /**NOTE: This for-loop creates 1 thread per result.**/
            // Thread t = StartThread(i);
            int NumResultsPerThread = 10; // FELLOW DEVELOPERS: Set the number of results per thread here.
            ThreadsRunning = ParsedResults.Count / NumResultsPerThread;
            if (ParsedResults.Count % NumResultsPerThread > 0) ThreadsRunning++;
            ThreadsComplete = 0;
            int quota = ParsedResults.Count;
            while (quota > 0)
            {
                int x, y;
                y = quota - 1; // setting max index
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
                Thread t = StartThread(x, y);
            }
            while (HubLock) { Thread.Sleep(1000); }
            watch.Stop();
            DebugHelper.displayln(Convert.ToString(watch.ElapsedMilliseconds));
            TotalRunTime = (float)watch.ElapsedMilliseconds / 1000;
            //DiagnosticPrint(results);
        }
        /**
         * Driver method designed specifically with Web Search parameters in mind.
         **/
        private void processWeb(BingWebOnly.BingSearchContainer bingContainer, int pages, string query, string market)
        {
            try
            {
                int count = 0;
                bool complete = false;
                for (int i = 0; i < pages; i++)
                {
                    var webQuery = bingContainer.Web(query, market, null, null, null, null, null, null);
                    if (top < 50)
                        webQuery = webQuery.AddQueryOption("$top", top);
                    else
                        webQuery = webQuery.AddQueryOption("$top", 50);
                    webQuery = webQuery.AddQueryOption("$skip", skip);
                    var webResults = webQuery.Execute();
                    foreach (var result in webResults)
                    {
                        if (ParsedResults.Count > 0 && result.Url.Equals(ParsedResults[ParsedResults.Count - 1].Url)) continue; // checks for consecutive duplicates
                        ParsedResults.Add(new SearchResult(ParsedResults.Count + 1 + InitialSkip, result.Title, result.Url));
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
            } // try
            catch (System.Data.Services.Client.DataServiceQueryException e)
            {
                DebugHelper.displayln(e.Message);
            }
        } // processWeb
        /**
         * Driver method designed specifically with News Search in mind.
         **/
        private void processNews(Bing.BingSearchContainer bingContainer, int pages, string query, string market)
        {
            try
            {
                int count = 0;
                bool complete = false;
                for (int i = 0; i < pages; i++)
                {
                    var webQuery = bingContainer.News(query, null, market, null, null, null, null, null, null);
                    if (top < 15)
                        webQuery = webQuery.AddQueryOption("$top", top);
                    else
                        webQuery = webQuery.AddQueryOption("$top", 15);
                    webQuery = webQuery.AddQueryOption("$skip", skip);
                    var webResults = webQuery.Execute();
                    foreach (var result in webResults)
                    {
                        if (ParsedResults.Count > 0 && result.Url.Equals(ParsedResults[ParsedResults.Count - 1].Url)) continue; // checks for consecutive duplicates
                        ParsedResults.Add(new SearchResult(ParsedResults.Count + 1 + InitialSkip, result.Title, result.Url, result.Source, result.Date));
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
            } // try
            catch (System.Data.Services.Client.DataServiceQueryException e)
            {
                DebugHelper.displayln(e.Message);
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
            var t = new Thread(() => ScrapeBatch(x, y));
            t.Start();
            return t;
        }
        private readonly object SyncLock = new object();
        private void incrementOmitCount()
        {
            lock (SyncLock)
            {
                OmitCount++;
            }
        } // incrementOmitCount
        private void checkLock()
        {
            lock (SyncLock)
            {
                ThreadsComplete++;
                DebugHelper.displayln(ThreadsComplete + " / " + ThreadsRunning);
                if (ThreadsComplete >= ThreadsRunning) { HubLock = false; }
            }
        } // checkLock
        /**
        * Scrapes websites in multiple batches, as opposed to single pages.
        * Uses HttpWebRequest, along with specific settings that mimic a web browser.
        * The line
        * if (response.StatusCode != HttpStatusCode.OK)
        * is questionable; w.GetResponse() can throw a WebException that bypasses the
        * if statement entirely.
        **/
        private void ScrapeBatch(int x, int y)
        {
            for (int i = x; i <= y; i++)
            {
                if (ParsedResults[i].Scraped) continue;
                try
                {
                    string pageData = "";
                    int status = 0;
                    HttpWebResponse response = null;
                    HttpWebRequest w = (HttpWebRequest)WebRequest.Create(ParsedResults[i].Url);
                    w.Proxy = null;
                    w.Timeout = PingTime;
                    w.ContentLength = 0;
                    w.Method = WebRequestMethods.Http.Get;
                    CookieContainer cookieJar = new CookieContainer();
                    w.CookieContainer = cookieJar;
                    w.Accept = "text/html, image/png, image/jpeg, image/gif, */*;q=0.1";
                    w.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:25.0) Gecko/20100101 Firefox/33.0";
                    w.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                    w.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    response = (HttpWebResponse)w.GetResponse();    // Test the site for an OK response before proceeding
                    if (response.StatusCode != HttpStatusCode.OK)   // If not OK
                    {
                        ParsedResults[i].ExceptionFound = true;
                        ParsedResults[i].ErrorMsg = response.StatusDescription;
                        continue;
                    }

                    HtmlWeb web = new HtmlWeb();
                    web.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:25.0) Gecko/20100101 Firefox/33.0";
                    web.UseCookies = true;
                    try
                    {
                        bool Done = false;
                        CancellationTokenSource tokenSource = new CancellationTokenSource(LoadTime);       // Set your timeout for token here.
                        Task.Factory.StartNew(() => {LoadToStringAsync(ParsedResults[i].Url, ref pageData, i, ref Done, tokenSource); }, tokenSource.Token);
                        while (!tokenSource.IsCancellationRequested && !Done)       // while token is valid && not done scraping
                        {
                            Thread.Sleep(150);
                            if (Done) break;
                            if (tokenSource.IsCancellationRequested)
                            {
                                tokenSource.Dispose();
                                if (Done) break;                                    // Last-ditch effort to check for (Done == true).
                                throw new TaskCanceledException();
                            }
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        ParsedResults[i].ExceptionFound = true;
                        ParsedResults[i].ErrorMsg = "TimeOut: page took an excessive amount of time to load.";
                        DebugHelper.displayln("[" + i + "] TimeOut Exception: " + ParsedResults[i].Url);
                        continue;
                    }
                    status = (int)response.StatusCode;
                    if (!ParsedResults[i].ExceptionFound)
                    {
                        //pageData.Replace('"', '\"');
                        Regex regex;
                        Match match = null;
                        foreach (string s in ClientWebsiteParsed)
                        {
                            regex = new Regex(s, RegexOptions.IgnoreCase, new TimeSpan(0, 0, 10));
                            match = regex.Match(pageData);
                            if (match.Success)
                            {
                                ParsedResults[i].LinksToClientWebsite = true;
                                if (ExcludeLinkbackResults)
                                {
                                    ParsedResults[i].SkipThisResult = true;
                                    incrementOmitCount();
                                }
                            }
                        }
                        bool containsPhrase = pageData.IndexOf(PhraseSearchString, StringComparison.OrdinalIgnoreCase) >= 0;
                        if (containsPhrase)
                        {
                            ParsedResults[i].ContainsSearchPhrase = true;
                        }
                    }
                }
                catch (RegexMatchTimeoutException)
                {
                    ParsedResults[i].ExceptionFound = true;
                    ParsedResults[i].ErrorMsg = "Took too long to find a link on webpage.";
                }
                catch (TaskCanceledException e)
                {
                    ParsedResults[i].ExceptionFound = true;
                    ParsedResults[i].ErrorMsg = "TimeOut Exception: " + e.Message;
                    DebugHelper.displayln("[" + i + "] TimeOut Exception: " + ParsedResults[i].Url);
                    continue;
                }
                catch (System.Net.WebException e)
                {
                    DebugHelper.displayln("[" + i + "] Web Exception: " + ParsedResults[i].Url + " >> " + e.Message);
                    HttpWebResponse res = (HttpWebResponse)e.Response;
                    ParsedResults[i].ExceptionFound = true;
                    if (res == null) ParsedResults[i].ErrorMsg = e.Message;
                    else
                        ParsedResults[i].ErrorMsg = "HTTP Status Code " + (int)res.StatusCode + ": " + res.StatusDescription;
                }
                catch (InvalidOperationException e)
                {
                    DebugHelper.displayln("[" + i + "] Invalid Operation Exception: " + ParsedResults[i].Url + " >> " + e.Message);
                }
                ParsedResults[i].Scraped = true;
            }
            DebugHelper.display("Index Range: [" + x + ", " + y + "], ");
            checkLock();
        } // ScrapeBatch
        public void LoadToStringAsync(string Url, ref string pageData, int i, ref bool Done, CancellationTokenSource tokenSource)
        {
            HtmlWeb web = new HtmlWeb();
            web.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:25.0) Gecko/20100101 Firefox/33.0";
            web.UseCookies = true;
            try
            {
                HtmlDocument doc = web.Load(Url);
                pageData = doc.DocumentNode.OuterHtml;
            }
            catch (ArgumentException)
            { // Catching GZIP encoding issue
                string html;
                using (var wc = new GZipWebClient())
                {
                    html = wc.DownloadString(Url);
                }
                var htmldocObject = new HtmlDocument();
                htmldocObject.LoadHtml(html);
                pageData = htmldocObject.DocumentNode.OuterHtml;
            }
            catch (NullReferenceException e) // This will happen as the result of program trying to load a non-HTML page
            {
                ParsedResults[i].ExceptionFound = true;
                ParsedResults[i].ErrorMsg = "NullRef Exception: Page doesn't appear to be in a valid format.";
                DebugHelper.displayln("[" + i + "] NullRef Exception: " + ParsedResults[i].Url);
            }
            catch (System.IO.IOException e)
            {
                ParsedResults[i].ExceptionFound = true;
                ParsedResults[i].ErrorMsg = "IO Exception: " + e.Message;
                DebugHelper.displayln("[" + i + "] IO Exception: " + ParsedResults[i].Url);
            }
            catch (System.Net.ProtocolViolationException e)
            {
                ParsedResults[i].ExceptionFound = true;
                ParsedResults[i].ErrorMsg = "Protocol Violation: " + e.Message;
            }
            catch (System.Net.WebException e)
            {
                DebugHelper.displayln("[" + i + "] Web Exception: " + ParsedResults[i].Url + " >> " + e.Message);
                HttpWebResponse res = (HttpWebResponse)e.Response;
                ParsedResults[i].ExceptionFound = true;
                if (res == null) ParsedResults[i].ErrorMsg = e.Message;
                else
                    ParsedResults[i].ErrorMsg = "HTTP Status Code " + (int)res.StatusCode + ": " + res.StatusDescription;
            }
            Done = true;
            DebugHelper.displayln("Task complete.");
        } // LoadToStringAsync
        /**
        * A shortcut method for scraping a single index as opposed to batches.
        **/
        private void scrapeURL(int i)
        {
            ScrapeBatch(i, i);
        } // scrapeURL
        /**
        * Prints out all SearchResults information stored in a list
        * in the output console. Useful for testing purposes.
        * @para List<SearchResult> results: list of SearchResults
        **/
        private void DiagnosticPrint(List<SearchResult> results)
        {
            foreach (SearchResult sr in results)
            {
                if (sr.SkipThisResult) continue;
                DebugHelper.displayln(sr.Url);
                if (sr.ExceptionFound)
                {
                    DebugHelper.displayln(sr.ErrorMsg);
                    continue;
                }
                if (sr.LinksToClientWebsite) DebugHelper.displayln("OKAY: Site features links that point to target(s).");
                else DebugHelper.displayln("NOT: No links to target(s) found.");
                if (sr.ContainsSearchPhrase) DebugHelper.displayln("OKAY: Site contains instances of " + PhraseSearchString);
                else DebugHelper.displayln("NOT: No instances of " + PhraseSearchString + " found.");
                DebugHelper.displayln("");
            }
        } // DiagnosticPrint
    } // public class ProcessHub
} // namespace