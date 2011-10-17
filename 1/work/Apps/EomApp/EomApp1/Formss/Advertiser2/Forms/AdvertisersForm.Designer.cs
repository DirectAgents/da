namespace EomApp1.Formss.Advertiser2.Forms
{
    partial class AdvertisersForm
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
            this.advertisers2Control1 = new EomApp1.Formss.Advertiser2.Controls.Advertisers2Control();
            this.SuspendLayout();
            // 
            // advertisers2Control1
            // 
            this.advertisers2Control1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advertisers2Control1.Location = new System.Drawing.Point(0, 0);
            this.advertisers2Control1.Name = "advertisers2Control1";
            this.advertisers2Control1.Size = new System.Drawing.Size(474, 662);
            this.advertisers2Control1.TabIndex = 0;
            // 
            // AdvertisersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 662);
            this.Controls.Add(this.advertisers2Control1);
            this.Name = "AdvertisersForm";
            this.ShowIcon = false;
            this.Text = "Advertisers";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Advertisers2Control advertisers2Control1;
    }
}