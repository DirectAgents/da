namespace EomApp1.Formss.Synch
{
    partial class SynchForm
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
            this.CampaignPrepareForStatsButton = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.campaignPrepareForStatsBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.logBox1 = new Mainn.Controls.Logging.LoggerBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.fromDayTextBox = new System.Windows.Forms.TextBox();
            this.toDayTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.preDeleteCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CampaignPrepareForStatsButton
            // 
            this.CampaignPrepareForStatsButton.Location = new System.Drawing.Point(254, 59);
            this.CampaignPrepareForStatsButton.Name = "CampaignPrepareForStatsButton";
            this.CampaignPrepareForStatsButton.Size = new System.Drawing.Size(140, 23);
            this.CampaignPrepareForStatsButton.TabIndex = 1;
            this.CampaignPrepareForStatsButton.Text = "Go";
            this.CampaignPrepareForStatsButton.UseVisualStyleBackColor = true;
            this.CampaignPrepareForStatsButton.Click += new System.EventHandler(this.CampaignPrepareForStatsButton_Click);
            // 
            // campaignPrepareForStatsBackgroundWorker
            // 
            this.campaignPrepareForStatsBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.CampaignPrepareForStatsBackgroundWorker_DoWork);
            // 
            // logBox1
            // 
            this.logBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.logBox1.Location = new System.Drawing.Point(0, 165);
            this.logBox1.Name = "logBox1";
            this.logBox1.ShowPrimary = true;
            this.logBox1.ShowSecondary = true;
            this.logBox1.Size = new System.Drawing.Size(491, 403);
            this.logBox1.TabIndex = 3;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(226, 70);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(254, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(64, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Payouts";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(254, 35);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(50, 17);
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "Stats";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // fromDayTextBox
            // 
            this.fromDayTextBox.Location = new System.Drawing.Point(310, 33);
            this.fromDayTextBox.Name = "fromDayTextBox";
            this.fromDayTextBox.Size = new System.Drawing.Size(27, 20);
            this.fromDayTextBox.TabIndex = 7;
            this.fromDayTextBox.Text = "1";
            // 
            // toDayTextBox
            // 
            this.toDayTextBox.Location = new System.Drawing.Point(366, 33);
            this.toDayTextBox.Name = "toDayTextBox";
            this.toDayTextBox.Size = new System.Drawing.Size(27, 20);
            this.toDayTextBox.TabIndex = 8;
            this.toDayTextBox.Text = "31";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(344, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "to";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(374, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Restrict to Affiliate ID (leave blank for all affiliates, use numbers only, ex: 3" +
    "586)";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(386, 25);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(85, 20);
            this.textBox1.TabIndex = 11;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.preDeleteCheckBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 88);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(479, 71);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Additional Options";
            // 
            // preDeleteCheckBox
            // 
            this.preDeleteCheckBox.AutoSize = true;
            this.preDeleteCheckBox.Location = new System.Drawing.Point(9, 44);
            this.preDeleteCheckBox.Name = "preDeleteCheckBox";
            this.preDeleteCheckBox.Size = new System.Drawing.Size(197, 17);
            this.preDeleteCheckBox.TabIndex = 13;
            this.preDeleteCheckBox.Text = "Delete all items for PID before synch";
            this.preDeleteCheckBox.UseVisualStyleBackColor = true;
            // 
            // SynchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 568);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toDayTextBox);
            this.Controls.Add(this.fromDayTextBox);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.logBox1);
            this.Controls.Add(this.CampaignPrepareForStatsButton);
            this.MaximizeBox = false;
            this.Name = "SynchForm";
            this.ShowIcon = false;
            this.Text = "Synch Stats";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CampaignPrepareForStatsButton;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.ComponentModel.BackgroundWorker campaignPrepareForStatsBackgroundWorker;
        private Mainn.Controls.Logging.LoggerBox logBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TextBox fromDayTextBox;
        private System.Windows.Forms.TextBox toDayTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox preDeleteCheckBox;
    }
}