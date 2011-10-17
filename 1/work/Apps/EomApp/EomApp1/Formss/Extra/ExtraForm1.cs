using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Extra
{
    public partial class ExtraForm1 : Form
    {
        public ExtraForm1()
        {
            InitializeComponent();
        }

        private void ExtraForm1_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                this.accountManagerTableAdapter.Fill(this.accountManagersForExtraItemsDataSet.AccountManager);
                userControl11.Initialize();
            }
        }

        private void button1_Refresh(object sender, EventArgs e)
        {
            userControl11.RefreshByAccountManager(comboBox1.Text);
        }
    }
}
