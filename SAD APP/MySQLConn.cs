﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SAD_APP
{
    internal class MySQLConn
    {
        // The connections string for the database connection
        //Sishu added here
        //here again trials

        static readonly string connstring = "Data Source=DAGMAWI\\SQLEXPRESS01;" +
                                            "Integrated Security=True;" +
                                            "Connect Timeout=30;" +
                                            "Encrypt=False;";


        // Login page authentication 
        public static string loginPage(string username, string password)
        {
            try
            {
                string conQuery = "USE FinalHospital; SELECT Role FROM Users WHERE username = @Username AND password = @Password";

                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    using (SqlCommand cmd = new SqlCommand(conQuery, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string role = reader.GetString(0);
                                    return role;
                                }
                            }
                        }
                    }
                }

                return null;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                return null;
            }

        }

        // Fetch all the users for Admin
        public static DataTable listOfUsers() {
            try
            {
                string conQuery = "USE FinalHospital; SELECT Name, Username, Role FROM Users";

                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    using (SqlCommand cmd = new SqlCommand(conQuery, conn))
                    {
                        conn.Open();

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            conn.Close();
                            return dt;
                        }

                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred here : " + ex.Message);
                return null;
            }

        }

        // Create a user for Admin
        public static void createUser(string fullname, int phonenumber, string email, string specialization, string username, string role, string newpass)
        {
            try
            {
                string conQuery = "USE FinalHospital; INSERT INTO Users (Name, ContactNumber, Email, Username, Password, Role) VALUES(@Name, @ContactNumber, @Email, @Username, @Password, @Role)";

                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    using (SqlCommand cmd = new SqlCommand(conQuery, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@Name", fullname);
                        cmd.Parameters.AddWithValue("@ContactNumber", phonenumber);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", newpass);
                        cmd.Parameters.AddWithValue("@Role", role);

                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }

                if (role == "doctor")
                {
                    string fetchIDQuery = "USE FinalHospital; SELECT UserID FROM Users WHERE Name = @name";
                    string insertDoctorQuery = "USE FinalHospital; SET IDENTITY_INSERT Doctor ON; INSERT INTO Doctor(DoctorID, Specialization) VALUES(@UserID, @Specialization)";

                    using (SqlConnection conn = new SqlConnection(connstring))
                    {
                        using (SqlCommand cmd = new SqlCommand(fetchIDQuery, conn))
                        {
                            conn.Open();

                            cmd.Parameters.AddWithValue("@name", fullname);

                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                int userID = reader.GetInt32(0);
                                conn.Close();

                                using (SqlCommand cmd2 = new SqlCommand(insertDoctorQuery, conn))
                                {
                                    conn.Open();
                                    cmd2.Parameters.AddWithValue("@UserID", userID);
                                    cmd2.Parameters.AddWithValue("@Specialization", specialization);

                                    cmd2.ExecuteNonQuery();
                                    conn.Close();
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("Account created successfully");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        // Delete a User
        public static bool deleteUser(string username)
        {
            try
            {
                string conQuery = "USE FinalHospital; DELETE FROM Users WHERE username = @Username";

                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    using (SqlCommand cmd = new SqlCommand(conQuery, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@Username", username);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                return false;
            }
        }

        // To register Patients 
        public static void registerPatient(string fullName, int age, string gender, string city, int phoneNumber, string email)
        {
            using (SqlConnection connection = new SqlConnection(connstring))
            {
                connection.Open();
                string query = "USE FinalHospital; INSERT INTO Patient(Name, Age, Gender, City, ContactNumber, Email, Reviewed) VALUES(@fullName, @age, @gender, @city, @phoneNumber, @email, 0)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@fullName", fullName);
                    command.Parameters.AddWithValue("@age", age);
                    command.Parameters.AddWithValue("@gender", gender);
                    command.Parameters.AddWithValue("@city", city);
                    command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@email", email);
                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Record created successfully!");
        }

        // The list of unreviewed Patients
        public static DataTable listOfPatient()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();

                string query = "use FinalHospital; select PatientID, Name, Age, Gender from Patient Where Reviewed = 0 order by PatientID desc;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                }
            }

            return dt;
        }

        // To get the list of Doctors
        public static List<string> GetDoctorNames()
        {

            string query = "use FinalHospital; SELECT Name FROM Users WHERE Role = 'doctor'";

            List<string> doctorNames = new List<string>();

            using (SqlConnection connection = new SqlConnection(connstring))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        doctorNames.Add(reader["Name"].ToString());
                    }

                    reader.Close();
                }
            }

            return doctorNames;
        }

        // To get the list of Vacant Rooms
        public static List<string> GetVacantRooms()
        {
            string query = "use FinalHospital; SELECT RoomNumber FROM Room WHERE RoomStatus = 'vacant'";

            List<string> vacantRooms = new List<string>();

            using (SqlConnection connection = new SqlConnection(connstring))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        vacantRooms.Add(reader["RoomNumber"].ToString());
                    }

                    reader.Close();
                }
            }

            return vacantRooms;
        }

        // To register patient's Clinical Diagnosis
        public static bool registerClinical(int ID, string clinical, string history, string symptom, string assignedDoc, int assignedRoom)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string selectDoctorQuery = "use FinalHospital; SELECT UserID FROM Users WHERE Role = 'doctor' AND Name = @AssignedDoc;";
                    string insertQuery = "use FinalHospital; INSERT INTO Diagnosis(PatientID, ClinicalDiagnosis, History, Symptoms) VALUES (@ID, @Clinical, @History, @Symptom);";
                    string updateQuery = "use FinalHospital; UPDATE Patient SET Reviewed = 'true' WHERE PatientID = @ID;";
                    string insertAppointmentQuery = "use FinalHospital; INSERT INTO Appointment(PatientID, DoctorID) VALUES (@ID, @DoctorID);";
                    string insertAdmissionQuery = "use FinalHospital; INSERT INTO Admission(PatientID, RoomNumber) VALUES (@ID, @AssignedRoom);";

                    using (SqlCommand selectDoctorCmd = new SqlCommand(selectDoctorQuery, conn))
                    {
                        selectDoctorCmd.Parameters.AddWithValue("@AssignedDoc", assignedDoc);

                        using (SqlDataReader reader = selectDoctorCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int doctorID = reader.GetInt32(reader.GetOrdinal("UserID"));
                                reader.Close();
                                //MessageBox.Show("this is the iD: " + doctorID);
                                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                                {
                                    MessageBox.Show("InsertQuery");
                                    cmd.Parameters.AddWithValue("@ID", ID);
                                    cmd.Parameters.AddWithValue("@Clinical", clinical);
                                    cmd.Parameters.AddWithValue("@History", history);
                                    cmd.Parameters.AddWithValue("@Symptom", symptom);

                                    int rowsAffected = cmd.ExecuteNonQuery();
                                    //MessageBox.Show("Execute");

                                    if (rowsAffected > 0)
                                    {
                                        //MessageBox.Show("UpdateQuery");
                                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                                        {
                                            updateCmd.Parameters.AddWithValue("@ID", ID);
                                            rowsAffected = updateCmd.ExecuteNonQuery();
                                           // MessageBox.Show("Execute");
                                        }

                                        using (SqlCommand insertAppointmentCmd = new SqlCommand(insertAppointmentQuery, conn))
                                        {
                                           // MessageBox.Show("Appointment");
                                            insertAppointmentCmd.Parameters.AddWithValue("@ID", ID);
                                            insertAppointmentCmd.Parameters.AddWithValue("@DoctorID", doctorID);
                                            rowsAffected = insertAppointmentCmd.ExecuteNonQuery();
                                            //MessageBox.Show("Execute");
                                        }

                                        using (SqlCommand insertAdmissionCmd = new SqlCommand(insertAdmissionQuery, conn))
                                        {
                                            insertAdmissionCmd.Parameters.AddWithValue("@ID", ID);
                                            insertAdmissionCmd.Parameters.AddWithValue("@AssignedRoom", assignedRoom);
                                            rowsAffected = insertAdmissionCmd.ExecuteNonQuery();
                                        }
                                    }

                                    return rowsAffected > 0;
                                }
                            }
                            else
                            {
                                // No doctor found with the given name
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
              //  Log the exception details for debugging purposes

               Console.WriteLine(ex.ToString());

                return false;
            }
        }


        // The list of Reviewed Patients
        public static DataTable listOfReviewedPatient()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();

                string query = "use FinalHospital; select PatientID, Name, Age, Gender from Patient Where Reviewed = 1 AND (IsTreated = 0 OR IsTreated IS NULL) order by PatientID desc;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                }
                
                conn.Close();
            }

            return dt;
        }






    }
}

