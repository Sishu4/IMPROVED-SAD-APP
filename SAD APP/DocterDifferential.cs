﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAD_APP
{
    public partial class DocterDifferential : Form
    {
        DoctorAdd doctorAdd;
        public DocterDifferential(DoctorAdd doctorAdd)
        {
            InitializeComponent();
            this.doctorAdd = doctorAdd;
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void DocterDifferential_Load(object sender, EventArgs e)
        {
            this.trackBar1.Minimum = 1;
            this.trackBar1.Maximum = 5;

            this.trackBar2.Minimum = 1;
            this.trackBar2.Maximum = 5;

            this.trackBar3.Minimum = 1;
            this.trackBar3.Maximum = 5;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            doctorAdd.Show();
            this.Close();
        }
    }
}
