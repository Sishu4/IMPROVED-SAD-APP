using System;
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
    public partial class DoctorDefinitive : Form
    {
        DoctorAdd doctorAdd;
        public DoctorDefinitive(DoctorAdd doctorAdd)
        {
            InitializeComponent();
            this.doctorAdd = doctorAdd;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void DoctorDefinitive_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            doctorAdd.Show();
            this.Close();
        }
    }
}
