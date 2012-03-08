using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Leads
{
    /// <summary>
    /// Summary description for LeadsContainer
    /// </summary>
    public class LeadsContainer
    {
        public LeadsContainer()
        {
        }

        public List<ILead> GetLeads(string sortParameter)
        {
            List<ILead> leads = new List<ILead>();
            int numLeads = 10;
            Random random = new Random();
            while (leads.Count < numLeads)
            {
                leads.Add(Lead.CreateRandom(random));
            }
            string[] sortExpressions = sortParameter.Split(',');
            if (sortExpressions.Length > 0)
            {
                string sortExpression = sortExpressions[0];
                string[] sortInfos = sortExpression.Split(' ');
                string sortField = sortInfos[0];
                string sortDirection = (sortInfos.Length == 1 ? "ASC" : "DESC");
                switch (sortField)
                {
                    case "Timestamp":
                        switch (sortDirection)
                        {
                            case "ASC":
                                leads = leads.OrderBy(c => c.Timestamp).ToList();
                                break;
                            case "DESC":
                                leads = leads.OrderByDescending(c => c.Timestamp).ToList();
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            return leads;
        }
    }

    public interface ILead
    {
        string AppID { get; set; }
        string Type { get; set; }
        DateTime Timestamp { get; set; }
        string CDNumber { get; set; }
        string IP { get; set; }
        string ESourceID { get; set; }
        string State { get; set; }
        DateTime DateOfBirth { get; set; }
        string Email { get; set; }
        bool IsVetran { get; set; }
        string Credit { get; set; }
    }

    public class Lead : ILead
    {
        public string AppID { get; set; }
        public string Type { get; set; }
        public DateTime Timestamp { get; set; }
        public string CDNumber { get; set; }
        public string IP { get; set; }
        public string ESourceID { get; set; }
        public string State { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public bool IsVetran { get; set; }
        public string Credit { get; set; }

        internal static ILead CreateRandom(Random random)
        {
            Lead lead = new Lead {
                AppID = Guid.NewGuid().ToString(),
                Timestamp = DateTime.Today.AddHours(random.Next(30 * 24) * -1),
            };
            return lead;
        }
    }
}