using CRUD_Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_Final_Project
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Signup sup = new Signup();
            sup.Show();
            this.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            if (email_txt.Text == "" || password_txt.Text == "")
            {
                MessageBox.Show("Please fill all the fields");
            }
            else
            {

                if (checkEmail(email_txt.Text, password_txt.Text))
                {
                    int num = checkStatus();
                    if (num == 1)
                    {        
                        CustomerMenu sup = new CustomerMenu(getId());
                        sup.Show();
                        this.Hide();
                    }
                    else if (num == 2)
                    {
                        Admin sup = new Admin();
                        sup.Show();
                        this.Hide();
                    }
                    else if (num == 3)
                    {
                        AdminMenu sup = new AdminMenu(getId());
                        sup.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Email or Password");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Email or Password");
                }



            }
        }

        bool checkEmail(string email, string password)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id from UserId Where Email = @Email and Password = @Password", con);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                return true;
            }
            else
            {
                reader.Close();
                return false;
            }


        }
        int checkStatus()
        {
            int value = 0;
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select CustomerId from Customer join UserId on Customer.CustomerId = UserId.Id Where Email = @Email", con);
            cmd.Parameters.AddWithValue("@Email", email_txt.Text);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                value = 1;
                return value;
            }
            reader.Close();
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select [Admin Id] from Admin join UserId on Admin.[Admin Id] = UserId.Id Where Email = @Email", con1);
            cmd1.Parameters.AddWithValue("@Email", email_txt.Text);
            SqlDataReader reader1 = cmd1.ExecuteReader();
            if (reader1.Read())
            {
                reader1.Close();
                value = 2;
                return value;
            }
            reader1.Close();
            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Select ManagerId from Manager join UserId on Manager.ManagerId = UserId.Id Where Email = @Email", con2);
            cmd2.Parameters.AddWithValue("@Email", email_txt.Text);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            if (reader2.Read())
            {
                reader2.Close();
                value = 3;
                return value;
            }
            return value;


        }
        int getId()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id from UserId Where Email = @Email", con);
            cmd.Parameters.AddWithValue("@Email", email_txt.Text);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int id = reader.GetInt32(0);
                reader.Close();
                return id;
            }
            else
            {
                reader.Close();
                return 0;
            }
        }
    }
}
