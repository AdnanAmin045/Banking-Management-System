using CRUD_Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;


namespace DB_Final_Project
{
    public partial class Signup : Form
    {
        public Signup()
        {
            InitializeComponent();
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            lg.Show();
            this.Hide();

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (email_txt.Text == "" || password_txt.Text == "" || confirmPass_txt.Text == "")
            {
                MessageBox.Show("Please fill all the fields");
            }
            else
            {
                if (checkEmail(email_txt.Text))
                {
                    if (password_txt.Text == confirmPass_txt.Text)
                    {
                        if (checkDuplication())
                        {
                            MessageBox.Show("Email already exists");
                            clearBoxes();
                        }
                        else
                        {
                            MessageBox.Show("SignUp Successfully");
                            account ac = new account(email_txt.Text, password_txt.Text);
                            ac.Show();
                            this.Hide();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Password does not match");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Email");
                }
            }

        }
        bool checkEmail(string email)
        {
            string trimmedText = email.Replace(" ", "");
            string pattern = @"^[a-zA-Z0-9._%+-]+@(?:gmail\.com|student\.uet\.edu\.pk)$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(trimmedText);
        }
        bool checkDuplication()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from UserId Where UserId.Email = @Email", con);
            cmd.Parameters.AddWithValue("@Email", email_txt.Text);
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        void clearBoxes()
        {
            email_txt.Text = "";
            password_txt.Text = "";
            confirmPass_txt.Text = "";
        }
    }
}
