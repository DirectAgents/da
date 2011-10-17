using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using DAgents.Common;
namespace DirectTrack.Rest
{
    public class ResourceGetter
    {
        public static int MaxResources { get; set; }
        private int NumResources = 0;

        static ResourceGetter()
        {
            MaxResources = Int32.MaxValue;
        }

        public ResourceGetter(ILogger logger, string url)
        {
            this._logger = logger;
            this._rootURL = url;
        }

        public List<XDocument> GetResources()
        {
            GetResources(_rootURL);
            while (NumTasks() > 0) RemoveTask().Wait();
            return resources;
        }

        void GetResources(string url)
        {
            if (++NumResources > MaxResources) return; // Observe hard limit

            _logger.Log("Getting resources at " + url);

            AddTask(Task.Factory.StartNew(new Action(() =>
            {
                var doc = XDocument.Parse(DirectTrackRestCall.GetXml(url));

                if (_resourceListDeserializer.CanDeserialize(doc.CreateReader()))
                {
                    var rl = (resourceList)_resourceListDeserializer.Deserialize(doc.CreateReader());
                    foreach (var item in rl.resourceURL)
                    {
                        GetResources(url + item.location);
                    }
                }
                else
                {
                    _logger.Log("Got resource for " + url);

                    AddResrouce(url, doc);
                }
            })));
        }

        object resourceLock = new object();

        Queue<Task> tasks = new Queue<Task>();

        void AddResrouce(string url, XDocument doc)
        {
            lock (resourceLock)
            {
                _logger.Log("add resource");

                resources.Add(doc);
                OnGotResource(url, doc);
            }
        }

        object taskLock = new object();

        List<XDocument> resources = new List<XDocument>();

        void AddTask(Task task)
        {
            lock (taskLock)
            {
                tasks.Enqueue(task);
            }
        }

        Task RemoveTask()
        {
            lock (taskLock)
            {
                return tasks.Dequeue();
            }
        }

        int NumTasks()
        {
            lock (taskLock)
            {
                _logger.Log(tasks.Count + " tasks left");
                return tasks.Count;
            }
        }

        ILogger _logger;

        XmlSerializer _resourceListDeserializer = new XmlSerializer(typeof(resourceList));

        readonly string _rootURL;

        public delegate void GotResourceEventHandler(ResourceGetter sender, string url, XDocument doc);
        public event GotResourceEventHandler GotResource;

        void OnGotResource(string url, XDocument doc)
        {
            if (GotResource != null)
            {
                GotResource(this, url, doc);
            }
        }
    }
}
