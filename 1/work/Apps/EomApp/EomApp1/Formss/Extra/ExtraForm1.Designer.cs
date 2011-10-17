namespace EomApp1.Formss.Extra
{
    partial class ExtraForm1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.accountManagerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.accountManagersForExtraItemsDataSet = new EomApp1.Formss.Extra.AccountManagersForExtraItemsDataSet();
            this.userControl11 = new EomApp1.Formss.Extra.ExtraItemsUserControl1();
            this.accountManagerTableAdapter = new EomApp1.Formss.Extra.AccountManagersForExtraItemsDataSetTableAdapters.AccountManagerTableAdapter();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.accountManagerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountManagersForExtraItemsDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DataSource = this.accountManagerBindingSource;
            this.comboBox1.DisplayMember = "name";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(287, 1);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(89, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // accountManagerBindingSource
            // 
            this.accountManagerBindingSource.DataMember = "AccountManager";
            this.accountManagerBindingSource.DataSource = this.accountManagersForExtraItemsDataSet;
            this.accountManagerBindingSource.Filter = "";
            // 
            // accountManagersForExtraItemsDataSet
            // 
            this.accountManagersForExtraItemsDataSet.DataSetName = "AccountManagersForExtraItemsDataSet";
            this.accountManagersForExtraItemsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // userControl11
            // 
            this.userControl11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControl11.Location = new System.Drawing.Point(0, 0);
            this.userControl11.Name = "userControl11";
            this.userControl11.Size = new System.Drawing.Size(920, 627);
            this.userControl11.TabIndex = 0;
            // 
            // accountManagerTableAdapter
            // 
            this.accountManagerTableAdapter.ClearBeforeFill = true;
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::EomApp1.Properties.Resources.Refresh1;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.Location = new System.Drawing.Point(382, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 21);
            this.button1.TabIndex = 2;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Refresh);
            // 
            // ExtraForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 627);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.userControl11);
            this.Name = "ExtraForm1";
            this.ShowIcon = false;
            this.Text = "Extra Items";
            this.Load += new System.EventHandler(this.ExtraForm1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.accountManagerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountManagersForExtraItemsDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtraItemsUserControl1 userControl11;
        private System.Windows.Forms.ComboBox comboBox1;
        private AccountManagersForExtraItemsDataSet accountManagersForExtraItemsDataSet;
        private System.Windows.Forms.BindingSource accountManagerBindingSource;
        private AccountManagersForExtraItemsDataSetTableAdapters.AccountManagerTableAdapter accountManagerTableAdapter;
        private System.Windows.Forms.Button button1;
    }
}