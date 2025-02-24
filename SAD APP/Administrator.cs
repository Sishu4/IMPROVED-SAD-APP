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
    public partial class Administrator : Form
    {
        LoginPage loginPage;

        public Administrator(LoginPage loginpage)
        {
            InitializeComponent();
            this.loginPage = loginpage;
        }

        public void refreshUsersList()
        {
            listOfUsers.DataSource = null;
            listOfUsers.Rows.Clear();
            listOfUsers.Columns.Clear();

            listOfUsers.DataSource = MySQLConn.listOfUsers();
        }

        private void Administrator_Load(object sender, EventArgs e)
        {
            this.listOfUsers.DataSource = MySQLConn.listOfUsers();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new AdministratorAdd(this).Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            loginPage.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this account?", "Confirm Account Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (listOfUsers.SelectedRows.Count > 0)
                {
                    DataGridViewRow row = listOfUsers.SelectedRows[0];
                    string username = row.Cells["Username"].Value.ToString();

                    if (MySQLConn.deleteUser(username))
                    {
                        MessageBox.Show("Accound deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Accound couldn't be deleted!");
                    } 
                }
            refreshUsersList();
            }
        }

        private void listOfUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
