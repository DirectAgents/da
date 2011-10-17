using System;
using System.Windows.Forms;
using DGVColumnSelector;

namespace EomApp1.Formss.Extra
{
    public partial class ExtraItemsUserControl1 : UserControl
    {
        public ExtraItemsUserControl1()
        {
            InitializeComponent();

            // Prevent errors from popping up - need to look into why constraints get violated in first place...
            dataSet1.EnforceConstraints = false;
        }

        // called by ExtraForm1_Load
        public void Initialize()
        {
            // Fill all the table adapters
            currencyTableAdapter.Fill(dataSet1.Currency);
            unitTypeTableAdapter.Fill(dataSet1.UnitType);
            campaignTableAdapter.Fill(dataSet1.Campaign);
            affiliateTableAdapter.Fill(dataSet1.Affiliate);
            sourceTableAdapter.Fill(dataSet1.Source);
            itemReportingStatusTableAdapter.Fill(dataSet1.ItemReportingStatus);
            itemTableAdapter.Fill(dataSet1.Item);

            // Create a column selector and attach it to the main grid
            DataGridViewColumnSelector cs = new DataGridViewColumnSelector();
            cs.DataGridView = itemDataGridView;
            cs.MaxHeight = 800;
            cs.Width = 150;
        }

        private void itemBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.itemBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet1);

            //itemTableAdapter.Fill(dataSet1.Item);

            // scroll grid to bottom
            //this.itemDataGridView.CurrentCell = this.itemDataGridView[0, this.itemDataGridView.RowCount - 1];
        }

        public void RefreshByAccountManager(string accountManagerName)
        {
            this.Validate();
            this.itemBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet1);
            if (accountManagerName == "default")
            {
                itemTableAdapter.Fill(dataSet1.Item);
            }
            else
            {
                itemTableAdapter.FillBy(dataSet1.Item, accountManagerName);
            }
        }
    }
}
