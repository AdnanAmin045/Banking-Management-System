using CRUD_Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace DB_Final_Project
{
    public partial class AdminMenu : Form
    {
        float count = 0;
        int id;
        int customerRequestId;
        public AdminMenu(int id)
        {
            InitializeComponent();
            this.id = id;

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            managertabcontrol.SelectedIndex = 1;
            totalcutomer_txt.Text = getCustomerCount().ToString();
            totalbalance_txt.Text = getTotalBalance().ToString();
            totalcutomer_txt.Enabled = false;
            totalbalance_txt.Enabled = false;
            showCustomerChnangeRequestInfo();
        }
        void  showCustomerChnangeRequestInfo()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("showCustomerInfoIntoGrid", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            customergridview.DataSource = dt;
        }
        decimal getTotalBalance()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Sum(CurrentBalance) from Account Where Account.BranchCode = @Code", con);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
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
    
       

        

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            managertabcontrol.SelectedIndex = 0;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            managertabcontrol.SelectedIndex = 2;
           // showCustomerRequestData();

            showRequestTextBox();
        }
        void showRequestTextBox()
        {
            totalRequest_txt.Text = getCustomerRequests().ToString();
            totalRequest_txt.Enabled = false;
            totalapproved_txt.Text = getCustomerApprovedRequests().ToString();
            totalapproved_txt.Enabled = false;
            pendingreq_txt.Text = getCustomerPendingRequests().ToString();
            pendingreq_txt.Enabled = false;
        }
        int getCustomerPendingRequests()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from ChangeRequest " +
                               "join Customer on ChangeRequest.CustomerId = Customer.CustomerId join Account on Account.CustomerId = " +
                                              "Customer.CustomerId Where Account.BranchCode = @Code and ChangeRequest.Status = '6'", con);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
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
        int getCustomerApprovedRequests()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from ChangeRequest " +
                               "join Customer on ChangeRequest.CustomerId = Customer.CustomerId join Account on Account.CustomerId = " +
                                              "Customer.CustomerId Where Account.BranchCode = @Code and ChangeRequest.Status = '5'", con);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
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
        int getCustomerRequests()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from ChangeRequest " +
                "join Customer on ChangeRequest.CustomerId = Customer.CustomerId join Account on Account.CustomerId = " +
                "Customer.CustomerId Where Account.BranchCode = @Code", con);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
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
        void showCustomerRequestData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select RequestId,RequestType,UpdatedInfo,Description,CASE WHEN Status = '5' THEN 'Approved' " +
                "WHEN Status = '6' THEN 'Pending' END as RequestStatus  from ChangeRequest " +
                "join Customer on ChangeRequest.CustomerId = Customer.CustomerId join Account on Account.CustomerId = " +
                "Customer.CustomerId Where Account.BranchCode = @Code", con);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            requestGridView.DataSource = dt;

        }



        private void guna2Button7_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            lg.Show();
            this.Hide();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            managertabcontrol.SelectedIndex = 4;
           // showLoanRequestData();
            showDataLoanTextBox();
           

        }
        void showDataLoanTextBox()
        {
            guna2TextBox3.Text = getTotalLoanRequest().ToString();
            guna2TextBox3.Enabled = false;
            guna2TextBox2.Text = getTotalLoanApproved().ToString();
            guna2TextBox2.Enabled = false;
            guna2TextBox1.Text = getTotalLoanPending().ToString();
            guna2TextBox1.Enabled = false;
        }
        int getTotalLoanPending()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from Loan join Account on Loan.AccountId = Account.AccountId" +
                " Where BranchCode = @Code and LoanStatus = '6'", con);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
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
        int getTotalLoanApproved()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from Loan join Account on Loan.AccountId = Account.AccountId" +
                " Where BranchCode = @Code and LoanStatus = '5'", con);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
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
        void showLoanRequestData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select LoanId,Account.AccountNo,Loan.Amount,Duration,Format(Date,'dd-MMMM-yyyy')" +
                " as Date,Lookup.Name as PaymentStatus,Case When LoanStatus = '5' Then 'Approved' " +
                "When LoanStatus = '6' Then 'Pending' End as LoanStatus from Loan join Account " +
                "on Loan.AccountId = Account.AccountId join LookUp on LookUp.Id = Loan.PaymentStatus Where BranchCode = @Code", con);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            loanrequestgridView.DataSource = dt;
        }
        int getTotalLoanRequest()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from Loan join Account on Loan.AccountId = Account.AccountId" +
                " Where BranchCode = @Code", con);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
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





        private void guna2Button10_Click(object sender, EventArgs e)
        {
            count += 1;
            if (count % 2 != 0)
                infopanel.Show();
            else
                infopanel.Hide();
        }

        private void guna2Button18_Click(object sender, EventArgs e)
        {
           
        }

        private void guna2Button19_Click(object sender, EventArgs e)
        {
           
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            managertabcontrol.SelectedIndex = 3;
            showDebitCardDatIntoGrid();
            loadDebitCardIntoTextBoxes();
        }
        void loadDebitCardIntoTextBoxes()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from ATMCard join Account on ATMCard.AccountId = Account.AccountId " +
                "Where Account.BranchCode = @Code", con);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                totalatmreq_txt.Text = result.ToString();
            }
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select Count(*) from ATMCard join Account on ATMCard.AccountId = Account.AccountId " +
                               "Where Account.BranchCode = @Code and ATMCard.Status = '5'", con1);
            cmd1.Parameters.AddWithValue("@Code", branchCode_txt.Text);
            object result1 = cmd1.ExecuteScalar();
            if (result1 != null && result1 != DBNull.Value)
            {
                totalApproveAtm.Text = result1.ToString();
            }
            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Select Count(*) from ATMCard join Account on ATMCard.AccountId = Account.AccountId " +
                                              "Where Account.BranchCode = @Code and ATMCard.Status = '6'", con2);
            cmd2.Parameters.AddWithValue("@Code", branchCode_txt.Text);
            object result2 = cmd2.ExecuteScalar();
            if (result2 != null && result2 != DBNull.Value)
            {
                pendingAtm_txt.Text = result2.ToString();
            }
            totalApproveAtm.Enabled = false;
            totalatmreq_txt.Enabled = false;
            pendingAtm_txt.Enabled = false;





        }


        void showDebitCardDatIntoGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select ATMCard.Id as RequestID,Account.AccountNo,ATMCard.Card,LookUp.Name as CardType," +
                " FORMAT(ATMCard.Date,'dd-MM-yyyy') as AppliedDate,Case When ATMCard.Status = '5' Then 'Approved' When ATMCard.Status = '6' Then 'Pending' END as Status" +
                " from ATMCard join Account on " +
                "ATMCard.AccountId = Account.AccountId join LookUp on ATMCard.CardType = LookUp.Id" +
                " Where Account.BranchCode =  @Code", con);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            atmgridview.DataSource = dt;
        }

        private void guna2Button23_Click(object sender, EventArgs e)
        {
            
        }

        private void guna2Button20_Click(object sender, EventArgs e)
        {
           
        }
        void showPersonInfo()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select FirstName + ' ' + LastName as Name,CNIC,FORMAT([Date of Birth],'dd-MMMM-yyyy') " +
                "as [Date of Birth],Contact,Address,Branch.BranchName,Branch.BranchCode,Branch.Location " +
                "from Manager join PersonInfo on Manager.ManagerId = PersonInfo.Id join Branch on Branch.BranchId = Manager.BranchId Where Manager.ManagerId = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read(); // Move to the first row
                    name_txt.Text = reader.GetString(0);
                    cnic_txt.Text = reader.GetString(1);
                    dateOfBirth.Text = reader.GetString(2);
                    contactInfo_txt.Text = reader.GetString(3);
                    addressInfo_txt.Text = reader.GetString(4);
                    branchName_txt.Text = reader.GetString(5);
                    branchCode_txt.Text = reader.GetString(6);
                    location_txt.Text = reader.GetString(7);


                }
                reader.Close();
            }
        }

        private void AdminMenu_Load(object sender, EventArgs e)
        {
            showPersonInfo();
            totalCustomer_txt.Text = getCustomerCount().ToString();
            name_txt.Enabled = false;
            cnic_txt.Enabled = false;
            dateOfBirth.Enabled = false;
            contactInfo_txt.Enabled = false;
            addressInfo_txt.Enabled = false;
            branchName_txt.Enabled = false;
            branchCode_txt.Enabled = false;
            location_txt.Enabled = false;
            totalCustomer_txt.Enabled = false;
            
        }
        int getCustomerCount()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from Account Where Account.BranchCode = @Code", con);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
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

        private void requestGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int requestId = 0;
            if (requestGridView.SelectedRows.Count > 0)
            {
                object cellValue = requestGridView.SelectedRows[0].Cells[0].Value;
                if (cellValue != null && int.TryParse(cellValue.ToString(), out int result))
                {
                    requestId = result;
                }

            }
            DialogResult confimation = MessageBox.Show("Do you want to Approve it?", "Confirmation", MessageBoxButtons.YesNo);
            if (confimation == DialogResult.Yes)
            {
                if(checkCustomerDuplicaton(requestId))
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("UpdateCustomerChangeRequest", con);
                    cmd.CommandType =CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", requestId);
                    cmd.ExecuteNonQuery();
                    showCustomerRequestData();
                    showRequestTextBox();
                }
                else
                {
                    MessageBox.Show("Request is already approved");
                }
            }
        }
        bool checkCustomerDuplicaton(int id)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from ChangeRequest join Customer on ChangeRequest.CustomerId = Customer.CustomerId " +
                "join Account on Account.CustomerId = Customer.CustomerId Where RequestId = @Id and Status = @Satus and BranchCode = @Code;", con);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Code",branchCode_txt.Text);
            cmd.Parameters.AddWithValue("@Satus", 5);
            if(Convert.ToInt32(cmd.ExecuteScalar())> 0)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }

        private void loanrequestgridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int requestId = 0;
            if (loanrequestgridView.SelectedRows.Count > 0)
            {
                object cellValue = loanrequestgridView.SelectedRows[0].Cells[0].Value;
                if (cellValue != null && int.TryParse(cellValue.ToString(), out int result))
                {
                    requestId = result;
                }

            }
            DialogResult confimation = MessageBox.Show("Do you want to Approve it?", "Confirmation", MessageBoxButtons.YesNo);
            if (confimation == DialogResult.Yes)
            {
                if (checkCustomerLaonDuplication(requestId))
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("UpdateCustomerLoanRequest", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", requestId);
                    cmd.ExecuteNonQuery();
                    showLoanRequestData();
                    showDataLoanTextBox();
                    
                }
                else
                {
                    MessageBox.Show("Request is already approved");
                }
            }
        }
        bool checkCustomerLaonDuplication(int id)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from Loan join Account on Loan.AccountId = Account.AccountId " +
                "Where Account.BranchCode = @Code and LoanId = @Id and LoanStatus = @Satus;", con);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Code",branchCode_txt.Text);
            cmd.Parameters.AddWithValue("@Satus", 5);
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private void customergridview_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void guna2Button12_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("UpdateCustomerInfo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", customerRequestId);
            cmd.Parameters.AddWithValue("@FirstName", firstname_txt.Text);
            cmd.Parameters.AddWithValue("@LastName", lastName_txt.Text);
            cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(dobpicker_txt.Value));
            cmd.Parameters.AddWithValue("@Contact", contact_txt.Text);
            cmd.Parameters.AddWithValue("@Address", addresschange.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Data Updated Successfully");
            showCustomerChnangeRequestInfo();
            firstname_txt.Text = "";
            lastName_txt.Text = "";
            dobpicker_txt.Text = "";
            contact_txt.Text = "";
            addresschange.Text = "";
            firstname_txt.Enabled = true;
            lastName_txt.Enabled = true;
            dobpicker_txt.Enabled = true;
            contact_txt.Enabled = true;
            addresschange.Enabled = true;



        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            managertabcontrol.SelectedIndex = 5;
            getDataForDepositTextBoxes();
           // getDataForDepositRequest();
           
        }
        void getDataForDepositRequest()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select TransactionId as RequestId,Account.AccountNo,Amount," +
                "Format(Date,'dd-MMM-yyyy  hh:mm') as RequestDate,Case When [Transaction].Status = '5' Then 'Approved' When [Transaction].Status = '6' Then 'Pending' END as Status" +
                "  from [Transaction] join Account on [Transaction].AccountId = Account.AccountId Where Account.BranchCode = @Code and[Transaction].Type = '3'", con);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            depositgridview.DataSource = dt;

        }
        void getDataForDepositTextBoxes()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from [Transaction] join Account on [Transaction].AccountId =" +
                " Account.AccountId  Where Account.BranchCode = @Code and [Transaction].Type = '3'", con);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                totaldeposit_txt.Text = result.ToString();
            }
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select Count(*) from [Transaction] join Account on [Transaction].AccountId = Account.AccountId " +
                                              "Where Account.BranchCode = @Code and [Transaction].Status = '5' and [Transaction].Type = '3'; ", con1);
            cmd1.Parameters.AddWithValue("@Code", branchCode_txt.Text);
            object result1 = cmd1.ExecuteScalar();
            if (result1 != null && result1 != DBNull.Value)
            {
                approveddeposit_txt.Text = result1.ToString();
            }
            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Select Count(*) from [Transaction] join Account on [Transaction].AccountId = Account.AccountId " +
                                                             "Where Account.BranchCode = @Code and [Transaction].Status = '6' and [Transaction].Type = '3' ", con2);
            cmd2.Parameters.AddWithValue("@Code", branchCode_txt.Text);
            object result2 = cmd2.ExecuteScalar();
            if (result2 != null && result2 != DBNull.Value)
            {
                pendingdeposit_txt.Text = result2.ToString();
            }
            totaldeposit_txt.Enabled = false;
            approveddeposit_txt.Enabled = false;
            pendingdeposit_txt.Enabled = false;
        }

        private void loanrequestgridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void atmgridview_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int ATMId = 0;
            if (atmgridview.SelectedRows.Count > 0)
            {
                object cellValue = atmgridview.SelectedRows[0].Cells[0].Value;
                if (cellValue != null && int.TryParse(cellValue.ToString(), out int result))
                {
                    ATMId = result;
                }

            }
            DialogResult confimation = MessageBox.Show("Do you want to Approve it?", "Confirmation", MessageBoxButtons.YesNo);
            if (confimation == DialogResult.Yes)
            {
                if (checkDuplictionForATMRequest(ATMId))
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("UpdateATMCardRequest", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", ATMId);
                    cmd.ExecuteNonQuery();
                    showDebitCardDatIntoGrid();
                    loadDebitCardIntoTextBoxes();
                    MessageBox.Show("Request Approved Successfully");
                    
                }
                else
                {
                    MessageBox.Show("Request is already approved");
                }
            }
        }
        bool checkDuplictionForATMRequest(int id)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from ATMCard join Account on ATMCard.AccountId = Account.AccountId " +
                               "Where Account.BranchCode = @Code and ATMCard.Id = @Id and ATMCard.Status = @Satus;", con);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
            cmd.Parameters.AddWithValue("@Satus", 5);
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void depositgridview_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int depositId = 0;
            
            if (depositgridview.SelectedRows.Count > 0)
            {
                object cellValue = depositgridview.SelectedRows[0].Cells[0].Value;
                if (cellValue != null && int.TryParse(cellValue.ToString(), out int result))
                {
                    depositId = result;
                }

            }
            DialogResult confimation = MessageBox.Show("Do you want to Approve it?", "Confirmation", MessageBoxButtons.YesNo);
            if (confimation == DialogResult.Yes)
            {
                if (checkDuplicationForDepositRequest(depositId))
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("UpdateDepositRequest", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", depositId);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Request Approved Successfully");
                    getDataForDepositTextBoxes();
                    getDataForDepositRequest();
                    UpdateBalance(depositId);

                }
                else
                {
                    MessageBox.Show("Request is already approved");
                }
            }
        }
        void UpdateBalance(int  depositId)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Account.AccountId from Account join [Transaction] on Account.AccountId = [Transaction].AccountId  where [Transaction].TransactionId = @Id1", con);
            cmd.Parameters.AddWithValue("@Id1", depositId);
            int id = (int)cmd.ExecuteScalar();
           

            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select CurrentBalance from Account where AccountId = @Id2", con1);
            cmd1.Parameters.AddWithValue("@Id2", id);
            decimal amountBalance = Convert.ToDecimal(cmd1.ExecuteScalar());
           
            

            var con3 = Configuration.getInstance().getConnection();
            SqlCommand cmd3 = new SqlCommand("Select Amount from [Transaction] Where [Transaction].TransactionId = @Id3", con3);
            cmd3.Parameters.AddWithValue("@Id3", depositId);
            decimal amount = Convert.ToDecimal(cmd3.ExecuteScalar());
            

            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Update Account set CurrentBalance = @Balance Where AccountId = @Id4", con2);
            cmd2.Parameters.AddWithValue("@Id4", id);
            cmd2.Parameters.AddWithValue("@Balance", amount + amountBalance);
            cmd2.ExecuteNonQuery();


        }
        bool checkDuplicationForDepositRequest(int id)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from [Transaction] join Account on [Transaction].AccountId = Account.AccountId " +
                               "Where Account.BranchCode = @Code and [Transaction].TransactionId = @Id and [Transaction].Status = @Satus;", con);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
            cmd.Parameters.AddWithValue("@Satus", 5);
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void customergridview_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void depositgridview_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(guna2ComboBox1.SelectedIndex == 0)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select TransactionId as RequestId,Account.AccountNo,Amount," +
                    "Format(Date,'dd-MMM-yyyy  hh:mm') as RequestDate,Case When [Transaction].Status = '5' Then 'Approved' When [Transaction].Status = '6' Then 'Pending' END as Status" +
                    "  from [Transaction] join Account on [Transaction].AccountId = Account.AccountId Where Account.BranchCode = @Code and[Transaction].Type = '3' and [Transaction].Status = '6'", con);
                cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                depositgridview.DataSource = dt;
            }
            else if(guna2ComboBox1.SelectedIndex == 1)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select TransactionId as RequestId,Account.AccountNo,Amount," +
                    "Format(Date,'dd-MMM-yyyy  hh:mm') as RequestDate,Case When [Transaction].Status = '5' Then 'Approved' When [Transaction].Status = '6' Then 'Pending' END as Status" +
                    "  from [Transaction] join Account on [Transaction].AccountId = Account.AccountId Where Account.BranchCode = @Code and[Transaction].Type = '3' and [Transaction].Status = '5'", con);
                cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                depositgridview.DataSource = dt;
            }
            else if (guna2ComboBox1.SelectedIndex == 2)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select TransactionId as RequestId,Account.AccountNo,Amount," +
                    "Format(Date,'dd-MMM-yyyy  hh:mm') as RequestDate,Case When [Transaction].Status = '5' Then 'Approved' When [Transaction].Status = '6' Then 'Pending' END as Status" +
                    "  from [Transaction] join Account on [Transaction].AccountId = Account.AccountId Where Account.BranchCode = @Code and[Transaction].Type = '3'", con);
                cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                depositgridview.DataSource = dt;
            }
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox2.SelectedIndex == 0)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select RequestId,RequestType,UpdatedInfo,Description,CASE WHEN Status = '5' THEN 'Approved' " +
                    "WHEN Status = '6' THEN 'Pending' END as RequestStatus  from ChangeRequest " +
                    "join Customer on ChangeRequest.CustomerId = Customer.CustomerId join Account on Account.CustomerId = " +
                    "Customer.CustomerId Where Account.BranchCode = @Code and Status = '6'", con);
                cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                requestGridView.DataSource = dt;
            }
            else if (guna2ComboBox2.SelectedIndex == 1)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select RequestId,RequestType,UpdatedInfo,Description,CASE WHEN Status = '5' THEN 'Approved' " +
                    "WHEN Status = '6' THEN 'Pending' END as RequestStatus  from ChangeRequest " +
                    "join Customer on ChangeRequest.CustomerId = Customer.CustomerId join Account on Account.CustomerId = " +
                    "Customer.CustomerId Where Account.BranchCode = @Code and Status = '5'", con);
                cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                requestGridView.DataSource = dt;
            }
            else if (guna2ComboBox2.SelectedIndex == 2)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select RequestId,RequestType,UpdatedInfo,Description,CASE WHEN Status = '5' THEN 'Approved' " +
                    "WHEN Status = '6' THEN 'Pending' END as RequestStatus  from ChangeRequest " +
                    "join Customer on ChangeRequest.CustomerId = Customer.CustomerId join Account on Account.CustomerId = " +
                    "Customer.CustomerId Where Account.BranchCode = @Code", con);
                cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                requestGridView.DataSource = dt;
            }
        }

        private void guna2ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox3.SelectedIndex == 0)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select ATMCard.Id as RequestID,Account.AccountNo,ATMCard.Card,LookUp.Name as CardType," +
                    " FORMAT(ATMCard.Date,'dd-MM-yyyy') as AppliedDate,Case When ATMCard.Status = '5' Then 'Approved' When ATMCard.Status = '6' Then 'Pending' END as Status" +
                    " from ATMCard join Account on " +
                    "ATMCard.AccountId = Account.AccountId join LookUp on ATMCard.CardType = LookUp.Id" +
                    " Where Account.BranchCode =  @Code and ATMCard.Status = '6'", con);
                cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                atmgridview.DataSource = dt;
            }
            else if (guna2ComboBox3.SelectedIndex == 1)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select ATMCard.Id as RequestID,Account.AccountNo,ATMCard.Card,LookUp.Name as CardType," +
                    " FORMAT(ATMCard.Date,'dd-MM-yyyy') as AppliedDate,Case When ATMCard.Status = '5' Then 'Approved' When ATMCard.Status = '6' Then 'Pending' END as Status" +
                    " from ATMCard join Account on " +
                    "ATMCard.AccountId = Account.AccountId join LookUp on ATMCard.CardType = LookUp.Id" +
                    " Where Account.BranchCode =  @Code and ATMCard.Status = '5'", con);
                cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                atmgridview.DataSource = dt;
            }
            else if (guna2ComboBox3.SelectedIndex == 2)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select ATMCard.Id as RequestID,Account.AccountNo,ATMCard.Card,LookUp.Name as CardType," +
                    " FORMAT(ATMCard.Date,'dd-MM-yyyy') as AppliedDate,Case When ATMCard.Status = '5' Then 'Approved' When ATMCard.Status = '6' Then 'Pending' END as Status" +
                    " from ATMCard join Account on " +
                    "ATMCard.AccountId = Account.AccountId join LookUp on ATMCard.CardType = LookUp.Id" +
                    " Where Account.BranchCode =  @Code", con);
                cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                atmgridview.DataSource = dt;
            }
        }

        private void guna2ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(guna2ComboBox4.SelectedIndex == 0)
            {

                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select LoanId,Account.AccountNo,Loan.Amount,Duration,Format(Date,'dd-MMMM-yyyy')" +
                    " as Date,Lookup.Name as PaymentStatus,Case When LoanStatus = '5' Then 'Approved' " +
                    "When LoanStatus = '6' Then 'Pending' End as LoanStatus from Loan join Account " +
                    "on Loan.AccountId = Account.AccountId join LookUp on LookUp.Id = Loan.PaymentStatus Where BranchCode = @Code and LoanStatus = '5'", con);
                cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                loanrequestgridView.DataSource = dt;
            }
            else if(guna2ComboBox4.SelectedIndex == 1)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select LoanId,Account.AccountNo,Loan.Amount,Duration,Format(Date,'dd-MMMM-yyyy')" +
                    " as Date,Lookup.Name as PaymentStatus,Case When LoanStatus = '5' Then 'Approved' " +
                    "When LoanStatus = '6' Then 'Pending' End as LoanStatus from Loan join Account " +
                    "on Loan.AccountId = Account.AccountId join LookUp on LookUp.Id = Loan.PaymentStatus Where BranchCode = @Code and LoanStatus = '5'", con);
                cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                loanrequestgridView.DataSource = dt;
            }
            else if( guna2ComboBox4.SelectedIndex == 2)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select LoanId,Account.AccountNo,Loan.Amount,Duration,Format(Date,'dd-MMMM-yyyy')" +
                    " as Date,Lookup.Name as PaymentStatus,Case When LoanStatus = '5' Then 'Approved' " +
                    "When LoanStatus = '6' Then 'Pending' End as LoanStatus from Loan join Account " +
                    "on Loan.AccountId = Account.AccountId join LookUp on LookUp.Id = Loan.PaymentStatus Where BranchCode = @Code", con);
                cmd.Parameters.AddWithValue("@Code", branchCode_txt.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                loanrequestgridView.DataSource = dt;

            }
        }
    }
}
