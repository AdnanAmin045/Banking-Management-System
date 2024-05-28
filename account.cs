using CRUD_Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DB_Final_Project
{
    public partial class account : Form
    {
       
        string email;
        string password;

        public account(string email, string password)
        {
            InitializeComponent();
            this.email = email;
            this.password = password;
            email_txt.Text = email;
            email_txt.Enabled = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                long accountNo = 0;
                if (checkEmptyBox())
                {
                    if (IsNumeric(contact_txt.Text) && checkContactNo(contact_txt.Text))
                    {
                        if (checkCNIC(cnic_txt.Text) && IsNumeric(cnic_txt.Text))
                        {
                            if (radioBtn.Checked)
                            {
                                if (!checkCNICDuplication(cnic_txt.Text))
                                {
                                    var con = Configuration.getInstance().getConnection();
                                   
                                    SqlCommand cmd = new SqlCommand("INSERTNEWACCOUNTINFO", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@Email", email);
                                    cmd.Parameters.AddWithValue("@Password", password);
                                  
                                  
                                   
                                    cmd.Parameters.AddWithValue("@CNIC", cnic_txt.Text);
                                 
                                    cmd.Parameters.AddWithValue("@FirstName", firstName_txt.Text);
                                    cmd.Parameters.AddWithValue("@LastName", lastName_txt.Text);
                                    cmd.Parameters.AddWithValue("@Contact", contact_txt.Text);
                                    cmd.Parameters.AddWithValue("@Date", dob.Value);
                                    cmd.Parameters.AddWithValue("@Address", address_txt.Text);
                                   

                                    
                                  
                                    do
                                    {
                                        Random rand = new Random();
                                        accountNo = createRandAccountNo(rand);
                                    } while (checkDuplicaitonofAccountNo(accountNo));

                                    cmd.Parameters.AddWithValue("@AccountNo", accountNo.ToString());
                                    if (accountType_txt.Text == "Current")
                                    {
                                        cmd.Parameters.AddWithValue("@Type", 1);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@Type", 2);
                                    }
                                    cmd.Parameters.AddWithValue("@Code", branch_txt.Text);
                                    cmd.Parameters.AddWithValue("@Balance", 0);
                                    cmd.Parameters.AddWithValue("@AccountDate", DateTime.Now);
                                    cmd.ExecuteNonQuery();

                                   
                                    MessageBox.Show("Account Created Successfully");
                                    Login lg = new Login();
                                    lg.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    MessageBox.Show("CNIC already exists");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Accept the terms and conditions to proceed");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid CNIC number");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid contact number");
                    }
                }
                else
                {
                    MessageBox.Show("Please fill all the fields");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        bool checkCNICDuplication(string input)
        {
            bool result = false;
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Customer Where CNIC = @cnic", con);
            cmd.Parameters.AddWithValue("@cnic", input);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                dr.Close();
                result = true;
            }
            else
            {
                dr.Close();
            }
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select * from Manager Where CNIC = @cnic", con1);
            cmd1.Parameters.AddWithValue("@cnic", input);
            SqlDataReader dr1 = cmd1.ExecuteReader();
            if (dr1.Read())
            {
                dr1.Close();
                result = true;
            }
            else { dr1.Close(); }
            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Select * from Admin Where CNIC = @cnic", con2);
            cmd2.Parameters.AddWithValue("@cnic", input);
            SqlDataReader dr2 = cmd2.ExecuteReader();
            if (dr2.Read())
            {
                dr2.Close();
                result = true;
            }
            else { dr2.Close(); }
            return result;
        }
        bool checkEmptyBox()
        {
            if (firstName_txt.Text == "" || lastName_txt.Text == "" || contact_txt.Text == "" || branch_txt.Text == "" || accountType_txt.Text == ""
                || cnic_txt.Text == "" || address_txt.Text == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        bool IsNumeric(string input)
        {       
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
        bool checkContactNo(string text)
        {
            if (text.Length == 11)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool checkCNIC(string text)
        {
            if (text.Length == 13)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        int getId(string email)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id from UserId Where Email = @Email", con);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.ExecuteNonQuery();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        long createRandAccountNo(Random rand)
        {
            long min = 10000000000000;
            long max = 99999999999999;
            return (long)(rand.NextDouble() * (max - min) + min);
        }
        bool checkDuplicaitonofAccountNo(long accountNo)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select AccountNo from Account Where AccountNo = @AccountNo", con);
            cmd.Parameters.AddWithValue("@AccountNo", accountNo);
            cmd.ExecuteNonQuery();
            if (cmd.ExecuteScalar() == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void account_Load(object sender, EventArgs e)
        {
            loadDataIntoCombo();
        }

        private void branch_txt_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        void loadDataIntoCombo()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from BranchCode;", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                branch_txt.Items.Add(reader["BranchCode"].ToString());
            }
            reader.Close();
        }

        private void branch_txt_SelectedValueChanged(object sender, EventArgs e)
        {

        }
    }
}
