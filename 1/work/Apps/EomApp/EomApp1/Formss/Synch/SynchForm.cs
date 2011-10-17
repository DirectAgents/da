using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using DAgents.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
namespace EomApp1.Formss.Synch
{
    public partial class SynchForm : Form, ILogger
    {
        public SynchForm()
        {
            InitializeComponent();

            this.Shown += (s, e) =>
                {
                    toDayTextBox.Text = DateTime.DaysInMonth(Properties.Settings.Default.StatsYear, Properties.Settings.Default.StatsMonth).ToString();
                };
        }

        //private void campaignInitBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    Log("synching campaigns");
        //    DAgents.Synch.SynchUtility.SynchCampaignListFromDirectTrackToDatabase(Logger);

        //    Log("synching campaign groups");
        //    DAgents.Synch.SynchUtility.SynchCampaignGroups(Logger);

        //    Log("synching payouts");
        //    DAgents.Synch.SynchUtility.SynchPayoutsForAllCampaigns(Logger);

        //    Log("done");
        //}

        private void ExtractIntegerItemsFromSpaceAndLineDelimitedList(
            string s, out List<int> result)
        {
            result = new List<int>();
            var items = s.Split(' ', '\n');
            foreach (var item in items)
            {
                int pid = Convert.ToInt32(item);
                result.Add(pid);
            }
        }

        private void CampaignPrepareForStatsButton_Click(object sender, EventArgs e)
        {
            this.theTextInTheBox = richTextBox1.Text;
            campaignPrepareForStatsBackgroundWorker.RunWorkerAsync();
        }

        private void CampaignPrepareForStatsBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<int> pidList;
            ExtractIntegerItemsFromSpaceAndLineDelimitedList(theTextInTheBox, out pidList);

            pidList.AsParallel().ForAll(pid =>
            {
                // Payout
                if (checkBox1.Checked)
                {
                    Log("synching payouts");

                    if (theTextInTheBox.Length > 0)
                    {
                        // Restrict to affiliate
                        //if (textBox1.Text.Length > 0)
                        //{
                        //    int affid = Convert.ToInt32(textBox1.Text);
                        //    DAgents.Synch.SynchUtility.SynchPayoutsForCampaignPidForAffid(Logger, pid, affid);
                        //}
                        //else // All affiliates
                        //{
                        try
                        {
                            Logger.Log("calling SynchPayoutsForCampaignPid");
                            DAgents.Synch.SynchUtility.SynchPayoutsForCampaignPid(Logger, pid);
                            Logger.Log("returned from SynchPayoutsForCampaignPid");
                        }
                        catch (Exception e3)
                        {
                            Logger.Log(pid + ": " + e3.Message);
                        }
                        //}
                    }
                }

                // Stat
                if (checkBox2.Checked)
                {
                    Log("synching stats");

                    int sday = Convert.ToInt32(fromDayTextBox.Text);
                    int eday = Convert.ToInt32(toDayTextBox.Text);

                    try
                    {
                        DAgents.Synch.SynchUtility.SynchStatsForPid(
                            Logger,
                            pid,
                            Properties.Settings.Default.StatsYear,
                            Properties.Settings.Default.StatsMonth,
                            sday,
                            eday,
                            preDeleteCheckBox.Checked);
                    }
                    catch (Exception e2)
                    {
                        Logger.LogError(pid + ": " + e2.Message);
                    }
                }
            });

            Log("done");
        }

        private ILogger Logger { get { return this; } }

        #region ILogger Members
        //private delegate void SetPropertyThreadSafeDelegate<TResult>(Control @this, Expression<Func<TResult>> property, TResult value);
        //public static void SetPropertyThreadSafe<TResult>(this Control @this, Expression<Func<TResult>> property, TResult value)
        //{
        //    var propertyInfo = (property.Body as MemberExpression).Member as PropertyInfo;

        //    if (propertyInfo == null ||
        //        !@this.GetType().IsSubclassOf(propertyInfo.ReflectedType) ||
        //        @this.GetType().GetProperty(propertyInfo.Name, propertyInfo.PropertyType) == null)
        //    {
        //        throw new ArgumentException("The lambda expression 'property' must reference a valid property on this Control.");
        //    }

        //    if (@this.InvokeRequired)
        //    {
        //        @this.Invoke(new SetPropertyThreadSafeDelegate<TResult>(SetPropertyThreadSafe), new object[] { @this, property, value });
        //    }
        //    else
        //    {
        //        @this.GetType().InvokeMember(propertyInfo.Name, BindingFlags.SetProperty, null, @this, new object[] { value });
        //    }
        //}
        object logLock = new object();
        List<string> logMessages = new List<string>();
        List<string> logErrors = new List<string>();
        public void Log(string message)
        {
            lock (logLock)
            {
                if (message.StartsWith("!"))
                {
                    logBox1.Log(message);
                }
                else
                {
                    logBox1.Log(message);
                    //logMessages.Add(message);
                    //if (logMessages.Count % 10 == 0 || message == "EOF")
                    //{
                    //    logBox1.Log(string.Join("\n", logMessages));
                    //    logMessages.Clear();
                    //}
                }
            }
        }

        public void LogError(string message)
        {
            lock (logLock)
            {
                logBox1.LogError(message);
            }
        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
        }

        public string theTextInTheBox { get; set; }
    }
}
