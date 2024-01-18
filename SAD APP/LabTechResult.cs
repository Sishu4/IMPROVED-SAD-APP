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
    public partial class LabTechResult : Form
    {
        LabTech labTech;
        public LabTechResult(LabTech labTech)
        {
            InitializeComponent();
            this.labTech = labTech;
        }

        private void LabTechResult_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            labTech.Show();
            this.Close();
        }
    }
}
