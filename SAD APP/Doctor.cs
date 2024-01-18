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
    public partial class Doctor : Form
    {
        LoginPage loginPage;
        public Doctor(LoginPage loginpage)
        {
            InitializeComponent();
            this.loginPage = loginpage;
            refreshListOfReviewed();
        }

        public void refreshListOfReviewed()
        { 
            this.listOfReviewedPatient.DataSource = null;
            this.listOfReviewedPatient.Rows.Clear();
            this.listOfReviewedPatient.Columns.Clear();
            this.listOfReviewedPatient.DataSource = null;
            this.listOfReviewedPatient.DataSource = MySQLConn.listOfReviewedPatient();
            this.listOfReviewedPatient.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.listOfReviewedPatient.Columns["PatientID"].Visible = false;
        }

        private void Doctor_Load(object sender, EventArgs e)
        {
            refreshListOfReviewed();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listOfReviewedPatient.SelectedRows.Count > 0)
            {
                DataGridViewRow row = listOfReviewedPatient.SelectedRows[0];
                int patientID = int.Parse(row.Cells["PatientID"].Value.ToString());

                new DoctorAdd(this, patientID).Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Please select a row!");
            }   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loginPage.Show();
            this.Close();
        }
    }
}
