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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DB_Final_Project
{
    public partial class Admin : Form
    {
        string branchName;
        string branchCode;
        string location;
        string firstName;
        string lastName;
        string email;
        string cnic;
        string contact;
        string dob;
        string address;
        public Admin()
        {
            InitializeComponent();
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            guna2TabControl1.SelectedIndex = 0;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            guna2TabControl1.SelectedIndex = 1;
            loadDatIntoCombo();

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            guna2TabControl1.SelectedIndex = 2;
            loadDataOfCustomer();
            totalCustomer_txt.Text = getTotalCustomer().ToString();
            total_balance_txt.Text = getTotalBalance().ToString();
            totalCustomer_txt.Enabled = false;
            total_balance_txt.Enabled = false;
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            guna2TabControl1.SelectedIndex = 3;

        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            lg.Show();
            this.Hide();
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {

        }
        void clearBoxes()
        {
            branchName_txt.Text = "";
            branchCode_txt.Text = "";
            location_txt.Text = "";
        }
        bool checkDuplication()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Branch where BranchCode = @Code OR BranchName =@Name", con);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
            cmd.Parameters.AddWithValue("@Name", branchName_txt.Text);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                dr.Close();
                return true;
            }
            else
            {
                dr.Close();
                return false;
            }
        }
        bool checkBranchCode()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Branch where BranchCode = @Code", con);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                dr.Close();
                return true;
            }
            else
            {
                dr.Close();
                return false;
            }
        }
        bool checkDuplicationForUpdate()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from Branch where BranchCode = @Code And BranchName =@Name and Location = @Location", con);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
            cmd.Parameters.AddWithValue("@Name", branchName_txt.Text);
            cmd.Parameters.AddWithValue("@Location", location_txt.Text);
           if(Convert.ToInt32(cmd.ExecuteScalar())>3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        void loadBranchDataIntoGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from BranchData", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            branchgridview.DataSource = dt;
        }

        private void guna2Button9_Click_1(object sender, EventArgs e)
        {
            if (branchCode_txt.Text == "" || branchName_txt.Text == "" || location_txt.Text == "")
            {
                MessageBox.Show("Please fill all the boxes");
            }
            else
            {
                if(CheckStringFormat(branchCode_txt.Text))
                {
                    if (checkDuplication())
                    {
                        MessageBox.Show("Branch already exists");
                    }
                    else
                    {
                        var con = Configuration.getInstance().getConnection();
                        SqlCommand cmd = new SqlCommand("Insert into Branch (BranchName,BranchCode,Location) values (@Name,@Code,@Location)", con);
                        cmd.Parameters.AddWithValue("@Name", branchName_txt.Text);
                        cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
                        cmd.Parameters.AddWithValue("@Location", location_txt.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Branch Added Successfully");
                        clearBoxes();
                        loadBranchDataIntoGrid();

                    }
                }
                else
                {
                    MessageBox.Show("Invalid Branch Code");
                }
            }
        }
        bool CheckStringFormat(string input)
        {
           
            string pattern = @"^MBL-\d{3}$";

            
            Regex regex = new Regex(pattern);

           
            return regex.IsMatch(input);
        }

        private void Admin_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            loadBranchDataIntoGrid();
        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            if (branchCode_txt.Text == "" || branchName_txt.Text == "" || location_txt.Text == "")
            {
                MessageBox.Show("Please fill all the boxes");
            }
            else
            {
                if (checkDuplication())
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("Delete Branch Where BranchName = @Name and BranchCode = @Code", con);
                    cmd.Parameters.AddWithValue("@Name", branchName_txt.Text);
                    cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Branch Delete Successfully");
                    clearBoxes();
                    loadBranchDataIntoGrid();
                }
                else
                {
                    MessageBox.Show("Branch does not exist");
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (branchgridview.SelectedRows.Count > 0)
            {
                branchName_txt.Text = branchgridview.SelectedRows[0].Cells[0].Value?.ToString();
                branchCode_txt.Text = branchgridview.SelectedRows[0].Cells[1].Value?.ToString();
                location_txt.Text = branchgridview.SelectedRows[0].Cells[2].Value?.ToString();
                branchName = branchName_txt.Text;
                branchCode = branchCode_txt.Text;
                location = location_txt.Text;
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            if (branchCode_txt.Text == "" || branchName_txt.Text == "" || location_txt.Text == "")
            {
                MessageBox.Show("Please fill all the boxes");
            }
            else
            {
                if(CheckStringFormat(branchCode_txt.Text))
                {
                    if (checkDuplicationForUpdate())
                    {
                        MessageBox.Show("Branch already exists");
                    }
                    else
                    {
                        if(checkBranchCode() && branchCode_txt.Text != branchCode)
                        {
                            MessageBox.Show("BranchCode Already Exists");
                        }
                        else
                        {
                            var con = Configuration.getInstance().getConnection();
                            SqlCommand cmd = new SqlCommand("Update Branch set BranchName = @Name, BranchCode = @Code, Location = @Location where BranchName = @OldName and BranchCode = @OldCode", con);
                            cmd.Parameters.AddWithValue("@Name", branchName_txt.Text);
                            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
                            cmd.Parameters.AddWithValue("@Location", location_txt.Text);
                            cmd.Parameters.AddWithValue("@OldName", branchName);
                            cmd.Parameters.AddWithValue("@OldCode", branchCode);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Branch Updated Successfully");
                            clearBoxes();
                            loadBranchDataIntoGrid();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Branch Code");
                }
            }
        }
        void loadDataOfCustomer()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from CustomerData", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            customergridview.DataSource = dt;
        }
        int getTotalCustomer()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from Customer", con);
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                return int.Parse(result.ToString());
            }
            else
            {
                return 0;
            }
        }
        decimal getTotalBalance()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Sum(CurrentBalance) from Account", con);
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                return decimal.Parse(result.ToString());
            }
            else
            {
                return 0;
            }
        }

        private void guna2Button13_Click(object sender, EventArgs e)
        {
            loadManagerDataIntoGrid();
        }
        void loadManagerDataIntoGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from ManagerData", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void guna2Button15_Click(object sender, EventArgs e)
        {
           
            try
            {

                if (checkEmptyBoxes())
                {
                    MessageBox.Show("Please fill all the boxes");
                }
                else
                {
                    if (checkEmailDuplicaitonForManager())
                    {
                        MessageBox.Show("Email already exists");
                    }
                    else
                    {
                        if (checkDuplicationForManager())
                        {
                            MessageBox.Show("Manager already exists");
                        }
                        else
                        {
                            if(checkEmail(email_txt.Text))
                            {
                                if (checkCNIC(cnic_txt.Text))
                                {
                                    if (checkContactNo(contact_txt.Text) && IsNumeric(contact_txt.Text))
                                    {
                                        string password;
                                        var con = Configuration.getInstance().getConnection();
                                       
                                        SqlCommand cmd = new SqlCommand("INSERTMANAGERINFO", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@Email", email_txt.Text);
                                        do
                                        {
                                            password = generatePassword();
                                        } while (checkPassword(password));
                                        cmd.Parameters.AddWithValue("@Password", password);
                                       
                                       
                                       
                                       
                                        cmd.Parameters.AddWithValue("@CNIC", cnic_txt.Text);
                                        cmd.Parameters.AddWithValue("@BranchId", getBranchId());
                                       

                                        cmd.Parameters.AddWithValue("@FirstName", firstname_txt.Text);
                                        cmd.Parameters.AddWithValue("@LastName", lastName_txt.Text);
                                        cmd.Parameters.AddWithValue("@Contact", contact_txt.Text);
                                        cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(dob_txt.Text));
                                        cmd.Parameters.AddWithValue("@Address", address_txt.Text);
                                        cmd.ExecuteNonQuery();
                                        MessageBox.Show("LogIn Email " + email_txt.Text + "   Password " + password);
                                       
                                        MessageBox.Show("Manager Added Successfully");
                                        clearBoxesOfManager();
                                        loadManagerDataIntoGrid();
                                        loadDatIntoCombo();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Invalid Contact Number");
                                    }
                                }
                                else
                                {
                           
                                    MessageBox.Show("Invalid CNIC");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid Email");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
               
                
                MessageBox.Show(ex.Message);
            }

        }
        string generatePassword()
        {
            string chars = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$%^&*()_+";
            Random rand = new Random();

            char[] password = new char[5];
            for (int i = 0; i < 5; i++)
            {
                int index = rand.Next(chars.Length);
                password[i] = chars[index];
            }
            string passwordString = new string(password);
            return passwordString;
        }
        bool checkPassword(string password)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from UserId where Password = @Password", con);
            cmd.Parameters.AddWithValue("@Password", password);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                dr.Close();
                return true;
            }
            else
            {
                dr.Close();
                return false;
            }
        }
        bool checkEmail(string email)
        {
            string trimmedText = email.Replace(" ", "");
            string pattern = @"^[a-zA-Z0-9._%+-]+@(?:gmail\.com|student\.uet\.edu\.pk)$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(trimmedText);
        }
        int getBranchId()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select BranchId from Branch where BranchCode = @Code", con);
            cmd.Parameters.AddWithValue("@Code", branchcode_combobox.Text);
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                return int.Parse(result.ToString());
            }
            else
            {
                return 0;
            }
        }
        void clearBoxesOfManager()
        {
            firstname_txt.Text = "";
            lastName_txt.Text = "";
            email_txt.Text = "";
            cnic_txt.Text = "";
            contact_txt.Text = "";
            dob_txt.Text = "";
            address_txt.Text = "";

        }
        void loadDatIntoCombo()
        {
            branchcode_combobox.Items.Clear();
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Branch.BranchCode FROM Branch LEFT JOIN Manager ON Branch.BranchId = Manager.BranchId WHERE Manager.ManagerId IS NULL;", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                branchcode_combobox.Items.Add(reader["BranchCode"].ToString());
            }
            reader.Close();
        }

        private void guna2Button14_Click(object sender, EventArgs e)
        {
            int id = checkExistingDataForManager();
            if (checkEmptyBoxes())
            {
                MessageBox.Show("Please fill all the boxes");
            }
            else
            {

                if (id > 0)
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("Delete from Manager where ManagerId = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                    var con1 = Configuration.getInstance().getConnection();
                    SqlCommand cmd1 = new SqlCommand("Delete from PersonInfo where Id = @Id", con1);
                    cmd1.Parameters.AddWithValue("@Id", id);
                    cmd1.ExecuteNonQuery();
                    var con2 = Configuration.getInstance().getConnection();
                    SqlCommand cmd2 = new SqlCommand("Delete from UserId where Id = @Id", con2);
                    cmd2.Parameters.AddWithValue("@Id", id);
                    cmd2.ExecuteNonQuery();
                    MessageBox.Show("Manager Deleted Successfully");
                    clearBoxesOfManager();
                    loadManagerDataIntoGrid();
                    loadDatIntoCombo();
                }
                else
                {
                    MessageBox.Show("Manager does not exist");
                }
            }
        }
        int checkExistingDataForManager()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id from Manager join PersonInfo on Manager.ManagerId = PersonInfo.Id Where FirstName = @fName and LastName = @lName and Contact = @Contact and [Date of Birth] = @dob", con);
            cmd.Parameters.AddWithValue("@fName", firstname_txt.Text);
            cmd.Parameters.AddWithValue("@lName", lastName_txt.Text);
            cmd.Parameters.AddWithValue("@Contact", contact_txt.Text);
            cmd.Parameters.AddWithValue("@dob", Convert.ToDateTime(dob_txt.Text));
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                return int.Parse(result.ToString());
            }
            else
            {
                return 0;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                firstname_txt.Text = dataGridView2.SelectedRows[0].Cells[0].Value?.ToString();
                lastName_txt.Text = dataGridView2.SelectedRows[0].Cells[1].Value?.ToString();
                branchcode_combobox.Items.Add(dataGridView2.SelectedRows[0].Cells[2].Value?.ToString());
                branchcode_combobox.Text = branchcode_combobox.Items[branchcode_combobox.Items.Count - 1].ToString();
                email_txt.Text = dataGridView2.SelectedRows[0].Cells[3].Value?.ToString();
                cnic_txt.Text = dataGridView2.SelectedRows[0].Cells[5].Value?.ToString();
                address_txt.Text = dataGridView2.SelectedRows[0].Cells[6].Value?.ToString();
                contact_txt.Text = dataGridView2.SelectedRows[0].Cells[7].Value?.ToString();
                dob_txt.Text = dataGridView2.SelectedRows[0].Cells[8].Value?.ToString();


                firstName = firstname_txt.Text;
                lastName = lastName_txt.Text;
                email = email_txt.Text;
                cnic = cnic_txt.Text;
                contact = contact_txt.Text;
                dob = dob_txt.Text;
                address = address_txt.Text;
                branchCode = branchcode_combobox.Text;
            }
        }

        private void guna2Button12_Click(object sender, EventArgs e)
        {
                SqlTransaction transaction = null;
            try
            {

                if (checkEmptyBoxes())
                {
                    MessageBox.Show("Please Fill All the Boxes");
                }
                else
                {
                    int id = getManagerIdForUpdate();
                    if (id > 0)
                    {
                        if (checkEmail(email_txt.Text))
                        {
                            if (checkCNIC(cnic_txt.Text))
                            {
                                if (checkContactNo(contact_txt.Text) && IsNumeric(contact_txt.Text))
                                {

                                    if (checkEmailDuplicaitonForManager() && (email != email_txt.Text))
                                    {
                                        MessageBox.Show("Email already exists");
                                    }
                                    else
                                    {
                                        if (checkCNICDuplication(cnic_txt.Text) && (cnic != cnic_txt.Text))
                                        {
                                            MessageBox.Show("CNIC already exists");
                                        }
                                        else
                                        {
                                            
                                            var con = Configuration.getInstance().getConnection();
                                            
                                            SqlCommand cmd = new SqlCommand("UPDATEMANAGERINFO", con);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@Email", email_txt.Text);
                                            cmd.Parameters.AddWithValue("@Id", id);
                                            
                                           
                                            
                                            cmd.Parameters.AddWithValue("@cnic", cnic_txt.Text);
                                            cmd.Parameters.AddWithValue("@BranchId", getBranchId());
                                           
                                         
                                            cmd.Parameters.AddWithValue("@fName", firstname_txt.Text);
                                            cmd.Parameters.AddWithValue("@lName", lastName_txt.Text);
                                            cmd.Parameters.AddWithValue("@Contact", contact_txt.Text);
                                            cmd.Parameters.AddWithValue("@dob", Convert.ToDateTime(dob_txt.Text));
                                            cmd.Parameters.AddWithValue("@Address", address_txt.Text);
                                            cmd.ExecuteNonQuery();
                                           
                                            MessageBox.Show("Manager Updated Successfully");
                                            clearBoxesOfManager();
                                            loadManagerDataIntoGrid();
                                            loadDatIntoCombo();
                                        }
                                    }


                                }
                                else
                                {
                                    MessageBox.Show("Invalid Contact Number");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid CNIC");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Email");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Manager does not exist");
                    }
                }
            }
            catch (Exception ex)
            {
                if(transaction != null)
                {
                    transaction.Rollback();
                }
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
        int getManagerIdForUpdate()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select ManagerId from Manager join PersonInfo on Manager.ManagerId = PersonInfo.Id Where FirstName = @fName and LastName = @lName and Contact = @Contact and [Date of Birth] = @dob", con);
            cmd.Parameters.AddWithValue("@fName", firstName);
            cmd.Parameters.AddWithValue("@lName", lastName);
            cmd.Parameters.AddWithValue("@Contact", contact);
            cmd.Parameters.AddWithValue("@dob", Convert.ToDateTime(dob));
            cmd.Parameters.AddWithValue("@cnic", cnic);
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                return int.Parse(result.ToString());
            }
            else
            {
                return 0;
            }

        }
        bool checkEmptyBoxes()
        {
            if (firstname_txt.Text == "" || lastName_txt.Text == "" || branchcode_combobox.Text == "" || email_txt.Text == "" || cnic_txt.Text == ""
                 || contact_txt.Text == "" || dob_txt.Text == "" || address_txt.Text == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool checkEmailDuplicaitonForManager()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from UserId where Email = @Email", con);
            cmd.Parameters.AddWithValue("@Email", email_txt.Text);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                dr.Close();
                return true;
            }
            else
            {
                dr.Close();
                return false;
            }
        }
        bool checkDuplicationForManager()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from PersonInfo Join Manager on Manager.ManagerId = PersonInfo.Id Where FirstName =@fname and LastName = @lname and Contact = @contact and CNIC =@cnic", con);
            cmd.Parameters.AddWithValue("@fname", firstname_txt.Text);
            cmd.Parameters.AddWithValue("@lname", lastName_txt.Text);
            cmd.Parameters.AddWithValue("@contact", contact_txt.Text);
            cmd.Parameters.AddWithValue("@cnic", cnic_txt.Text);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                dr.Close();
                return true;
            }
            else
            {
                dr.Close();
                return false;
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

        private void guna2Button23_Click(object sender, EventArgs e)
        {
            guna2TabControl1.SelectedIndex = 4;
            getDataOfFeedback();
            feedback_txt.Text = getTotalFeedback().ToString(); 
            feedback_txt.Enabled = false;
        }
        void getDataOfFeedback()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from FeedbackData;", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            feedbackgridview.DataSource = dt;
        }
        int getTotalFeedback()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from Feedback", con);
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                return int.Parse(result.ToString());
            }
            else
            {
                return 0;
            }
        }

        private void guna2Button18_Click(object sender, EventArgs e)
        {
            Reports rp = new Reports();
            rp.Show();
            
        }

        private void guna2Button20_Click(object sender, EventArgs e)
        {
            Reports2 rp = new Reports2();
            rp.Show();
        }

        private void guna2Button22_Click(object sender, EventArgs e)
        {
            Reports3 rp = new Reports3();
            rp.Show();
        }

        private void branchcode_combobox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
