﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.Formss.Campaign
{
    public partial class CampaignsForm : Form
    {
        public CampaignsForm()
        {
            InitializeComponent();
        }

        private void CampaignsForm_Load(object sender, EventArgs e)
        {
            campaignsUC1.Fill();
        }
    }
}