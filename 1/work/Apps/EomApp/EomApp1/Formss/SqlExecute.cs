using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EomApp1.Formss
{
    public partial class SqlExecute : Form
    {
        public SqlExecute()
        {
            InitializeComponent();
        }

        private void SqlExecute_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dAMain1DataSetForSqlExecute.QueriesForSqlExecuteDialog' table. You can move, or remove it, as needed.
            this.queriesForSqlExecuteDialogTableAdapter.Fill(this.dAMain1DataSetForSqlExecute.QueriesForSqlExecuteDialog);

        }

        private void queriesForSqlExecuteDialogBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            richTextBox1.Text =
            ((DAMain1DataSetForSqlExecute.QueriesForSqlExecuteDialogRow)
            (((DataRowView)(queriesForSqlExecuteDialogBindingSource.Current)).Row)).query_text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var sqlConnection = new SqlConnection(DAgents.Common.Properties.Settings.Default.ConnStr))
            using (var sqlCommand = new SqlCommand(richTextBox1.Text, sqlConnection))
            {
                if (textBox1.Text == "da123")
                {
                    sqlConnection.Open();
                    int i = sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                    MessageBox.Show(string.Format("{0} rows affected", i));
                }
                else
                {
                    MessageBox.Show("Enter the correct password");
                }
            }
        }
    }
}
