using CRUD_Operations;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_Final_Project
{
    public partial class CustomerMenu : Form
    {
        decimal balance;
        string accounNo;
        private int count = 0;
        private int id;

        public CustomerMenu(int id)
        {
            InitializeComponent();
            this.id = id;

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }



        private void guna2Button14_Click(object sender, EventArgs e)
        {
            if (count % 2 == 0)
                infopanel.Visible = true;
            else
                infopanel.Visible = false;
            count = count + 1;
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            cutomermenu.SelectedIndex = 0;
            getDataOfPersonInfo();
            accountno_txt.Enabled = false;
            balance_txt.Enabled = false;
            name_txt.Enabled = false;
            cnic_txt.Enabled = false;
            contact_txt.Enabled = false;
            dob_txt.Enabled = false;
            Address_txt.Enabled = false;
            accountNoCash_txt.Enabled = false;
            balanceCash_txt.Enabled = false;
            accountNoLoan_txt.Enabled = false;
            balanceLoan_txt.Enabled = false;
            accountNotransfer_txt.Enabled = false;
            balanceTransfer_txt.Enabled = false;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            cutomermenu.SelectedIndex = 1;
            guna2ComboBox1.SelectedIndex = -1;
            getDataOfPersonInfo();
            transactionDataIntoGrid();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            cutomermenu.SelectedIndex = 2;
            details_txt.SelectedIndex = -1;
            getDataOfPersonInfo();
            
            loadTransferCashDataIntoGrid();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            cutomermenu.SelectedIndex = 3;
            getDataOfPersonInfo();      
            loadDataIntoGridForLoan();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            cutomermenu.SelectedIndex = 4;
            getDataOfPersonInfo();
            showDataofRequestIntoGrid();
            guna2ComboBox2.SelectedIndex = -1;


        }
        void getDataOfPersonInfo()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select AccountNo,CurrentBalance,FirstName + ' ' + LastName as UserName,CNIC,Contact,Format([Date of Birth],'dd-MMMM-yyyy'),Address from PersonInfo join Account on PersonInfo.Id = Account.CustomerId join Customer on Customer.CustomerId = PersonInfo.Id Where Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read(); 
                    accountno_txt.Text = reader.GetString(0);
                    balance_txt.Text = reader.GetDecimal(1).ToString();
                    name_txt.Text = reader.GetString(2);
                    cnic_txt.Text = reader.GetString(3);
                    contact_txt.Text = reader.GetString(4);
                    dob_txt.Text = reader.GetString(5);
                    Address_txt.Text = reader.GetString(6);
                    balance = decimal.Parse(balance_txt.Text);
                    accounNo = accountno_txt.Text;
                }
                reader.Close();
            }
            accountNoCash_txt.Text = accounNo;
            balanceCash_txt.Text = balance.ToString();
            accountNotransfer_txt.Text = accounNo;
            balanceTransfer_txt.Text = balance.ToString();
            sender_txt.Text = accounNo;
            accountNoLoan_txt.Text = accounNo;
            balanceLoan_txt.Text = balance.ToString();
            accountLoan_txt.Text = accounNo;
            guna2TextBox6.Text = accounNo;
            guna2TextBox7.Text = balance.ToString();
            laonpayaccount_txt.Text = accounNo;
            balancepayloan_txt.Text = balance.ToString();

            laonpayaccount_txt.Enabled = false;
            balancepayloan_txt.Enabled = false;
            guna2TextBox6.Enabled = false;
            guna2TextBox7.Enabled = false;
            accountLoan_txt.Enabled = false;
            sender_txt.Enabled = false;
            accountno_txt.Enabled = false;
            balance_txt.Enabled = false;
            name_txt.Enabled = false;
            cnic_txt.Enabled = false;
            contact_txt.Enabled = false;
            dob_txt.Enabled = false;
            Address_txt.Enabled = false;
            accountNoCash_txt.Enabled = false;
            balanceCash_txt.Enabled = false;
            accountNoLoan_txt.Enabled = false;
            balanceLoan_txt.Enabled = false;
            accountNotransfer_txt.Enabled = false;
            balanceTransfer_txt.Enabled = false;





        }





        private void CustomerMenu_Load(object sender, EventArgs e)
        {

            getDataOfPersonInfo();
            

        }

        private void guna2Button16_Click(object sender, EventArgs e)
        {
           
            try
            {


                if (amount_txt.Text == "" || description_txt.Text == "")
                {
                    MessageBox.Show("Please fill all the fields");
                }
                else
                {
                    if(IsNumeric(amount_txt.Text))
                    { 
                        if(guna2ComboBox1.SelectedIndex==0)
                        {
                            var con = Configuration.getInstance().getConnection();
                       
                            SqlCommand cmd = new SqlCommand("InsertTransactionData", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@AccountId", accountId());
                            cmd.Parameters.AddWithValue("@Amount", decimal.Parse(amount_txt.Text));
                            cmd.Parameters.AddWithValue("@Description", description_txt.Text);
                            cmd.Parameters.AddWithValue("@Type", getTransationTypeId());
                            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Status", 6);
                            cmd.ExecuteNonQuery();
                       
                           
                            MessageBox.Show("Deposit Request Submitted");
                            transactionDataIntoGrid();
                            getDataOfPersonInfo();
                            amount_txt.Text = "";
                            description_txt.Text = "";
                            guna2ComboBox1.SelectedIndex = -1;
                        }
                        else
                        {
                            if((checkBalance() - decimal.Parse(amount_txt.Text)) >= 0  && guna2ComboBox1.SelectedIndex == 1)
                            {
                                var con = Configuration.getInstance().getConnection();
                                
                                SqlCommand cmd = new SqlCommand("InsertTransactionDataWithdraw", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@AccountId", accountId());
                                cmd.Parameters.AddWithValue("@Amount", decimal.Parse(amount_txt.Text));
                                cmd.Parameters.AddWithValue("@Description", description_txt.Text);
                                cmd.Parameters.AddWithValue("@Type", getTransationTypeId());
                                cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                                cmd.Parameters.AddWithValue("@Status", 5);
                            
                              
                                cmd.Parameters.AddWithValue("@Balance", checkBalance() - decimal.Parse(amount_txt.Text));
                                cmd.Parameters.AddWithValue("@No", accounNo);
                                cmd.ExecuteNonQuery();
                               
                                MessageBox.Show("Amount has Withdraw");
                                transactionDataIntoGrid();
                                getDataOfPersonInfo();
                                amount_txt.Text = "";
                                description_txt.Text = "";
                                guna2ComboBox1.SelectedIndex = -1;

                            }
                            else
                            {
                                MessageBox.Show("Insufficient Balance");
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

        
        int getTransationTypeId()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id from LookUp Where Name = @Name", con);
            cmd.Parameters.AddWithValue("@Name", guna2ComboBox1.Text);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        int accountId()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select AccountId from Account Where CustomerId = @CustomerId", con);
            cmd.Parameters.AddWithValue("@CustomerId", id);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        void transactionDataIntoGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Amount, LookUp.Name AS [Transaction Type],Format([Transaction].Date,'dd-MMM-yyyy hh:mm') " +
                "as Date,CASE WHEN Status = '6' THEN 'Pending' WHEN Status = '5' THEN 'Approved' END AS [Application Status] " +
                "FROM [Transaction] JOIN LookUp ON [Transaction].Type = LookUp.Id JOIN Account ON [Transaction].AccountId =" +
                " Account.AccountId Where Account.AccountId = @Id", con);
            cmd.Parameters.AddWithValue("@Id", accountId());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cashdeposithistorygrid.DataSource = dt;
        }
        decimal getAmountForWithdraw()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select CurrentBalance from Account Where CustomerId = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            if(cmd.ExecuteScalar() == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
            
        }

        private void guna2Button17_Click(object sender, EventArgs e)
        {
            var connection = Configuration.getInstance().getConnection();
            
          
           
                if(amounttransfer_txt.Text == "" || reciver_txt.Text == "" || details_txt.Text == "" || password_txt.Text == "")
                {
                    MessageBox.Show("Please fill all the fields");
                }
                else
                {
                    if(IsNumeric(amounttransfer_txt.Text))
                    {

                        if(checkReceiverAccount())
                        {
                            if(checkLoginId(password_txt.Text))
                            {
                                if (balance < decimal.Parse(amounttransfer_txt.Text))
                                {
                                    MessageBox.Show("Insufficient Balance");
                                }
                                else
                                {

              
                                    SqlCommand cmd = new SqlCommand("InsertIntoTransferAmount", connection);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@Acc1", accountId());
                                    cmd.Parameters.AddWithValue("@Acc2", getAccountIdForTransfer());
                                    decimal amount;
                                    if (decimal.TryParse(amounttransfer_txt.Text, out amount))
                                    {
                                        cmd.Parameters.AddWithValue("@Amount", amount);
                                    }
                                    cmd.Parameters.AddWithValue("@Detail", details_txt.Text);
                                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);


                                    cmd.Parameters.AddWithValue("@SenderBalance", balance - decimal.Parse(amounttransfer_txt.Text));
                                    cmd.Parameters.AddWithValue("@SenderNo", accounNo);



                                    cmd.Parameters.AddWithValue("@ReceiverBalance", getReceiverAmount() + decimal.Parse(amounttransfer_txt.Text));
                                    cmd.Parameters.AddWithValue("@ReceiverNo", reciver_txt.Text);






                                    cmd.ExecuteNonQuery();
                                   
                                   
                                    MessageBox.Show("Amount Transfered Successfully");
                                    loadTransferCashDataIntoGrid();
                                    clearTransferBoxes();
                                    getDataOfPersonInfo();
                                    balanceTransfer_txt.Text = balance.ToString();
                                    amounttransfer_txt.Text = "";
                                    details_txt.SelectedIndex = -1;
                                    reciver_txt.Text = "";
                                    password_txt.Text = "";
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid Password");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Receiver Account No");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Amount");
                    }
                }
            
            
           
        }
        string getAccountIdForTransfer()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select AccountId from Account Where AccountNo = @AccountNo", con);
            cmd.Parameters.AddWithValue("@AccountNo", reciver_txt.Text);
            return cmd.ExecuteScalar().ToString();
        }
        void insetTransferData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("UpdateBalance", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Balance", balance - decimal.Parse(amounttransfer_txt.Text));
            cmd.Parameters.AddWithValue("@No", accounNo);
            cmd.ExecuteNonQuery();
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("UpdateBalance", con1);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@Balance", getReceiverAmount() + decimal.Parse(amounttransfer_txt.Text));
            cmd1.Parameters.AddWithValue("@No", reciver_txt.Text);
            cmd1.ExecuteNonQuery();
        }
        decimal getReceiverAmount()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select CurrentBalance from Account Where AccountNo = @AccountNo", con);
            cmd.Parameters.AddWithValue("@AccountNo", reciver_txt.Text);
            if(cmd.ExecuteScalar() == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
 
        }
        void clearTransferBoxes()
        {
            amounttransfer_txt.Text = "";
            reciver_txt.Text = "";
            details_txt.Text = "";
            password_txt.Text = "";
        }
        bool checkLoginId(string password)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id from UserId Where Id = @Id and Password = @Password", con);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Password", password);
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool checkReceiverAccount()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from Account Where AccountNo = @AccountNo", con);
            cmd.Parameters.AddWithValue("@AccountNo", reciver_txt.Text);
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        void loadTransferCashDataIntoGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Account.AccountNo as [Account No], Amount, Detail as Reason," +
                "FORMAT(Date,'dd-MM-yyyy  hh:mm') as Date from TransferAmount " +
                "join Account on TransferAmount.AccountTo=Account.AccountId Where AccountFrom = @Id", con);
            cmd.Parameters.AddWithValue("@Id", accountId());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            transferhistorygrid.DataSource = dt;
        }

        private void guna2Button19_Click(object sender, EventArgs e)
        {
            try
            {

                if (amountLoan_txt.Text == "" || PasswordLoan_txt.Text == "" || reasonCombo.Text == "" || durationLoan.Text == "")
                {
                    MessageBox.Show("Please fill all the fields");
                }
                else
                {
                    if(IsNumeric(amountLoan_txt.Text))
                    {
                        if(checkBalance()>=20000)
                        {

                            int days = daysForLoan();
                            if (true)
                            {
                                if (checkLoginId(PasswordLoan_txt.Text))
                                {
                                    if(checkLoanRequestAmount(amountLoan_txt.Text,durationLoan.SelectedIndex))
                                    {
                                        if(radioBtn.Checked)
                                        { 
                                            if(!checkDuplicationOfLoan())
                                            {
                                                var con = Configuration.getInstance().getConnection();
                                                SqlCommand cmd = new SqlCommand("InsertLoan", con);
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.AddWithValue("@Id", getLoanAccountForId());
                                                cmd.Parameters.AddWithValue("@Amount", decimal.Parse(amountLoan_txt.Text));
                                                cmd.Parameters.AddWithValue("@Reason", reasonCombo.Text);
                                                cmd.Parameters.AddWithValue("@Duration", durationLoan.Text);
                                                cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                                                cmd.Parameters.AddWithValue("@Status", 6);
                                                cmd.Parameters.AddWithValue("@PaymentStatus", 8);
                                                cmd.ExecuteNonQuery();
                                                MessageBox.Show("Loan Requested Successfully");
                                                clearLoanBoxes();
                                                loadDataIntoGridForLoan();
                                                amountLoan_txt.Text = "";
                                                reasonCombo.SelectedIndex = -1;
                                                durationLoan.SelectedIndex = -1;
                                                PasswordLoan_txt.Text = "";
                                                radioBtn.Checked = false;

                                            }
                                            else
                                            {
                                                MessageBox.Show("You have Already take the Loan");
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Accept the Terms and Condition");
                                        }
                            
                                    }
                                    else
                                    {
                                        MessageBox.Show("Amount is not according to the duration");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Invalid Password");
                                }
                            }
                            else
                            {
                                MessageBox.Show($"You can get a loan after {15 - days} days");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Insufficient Balance to Take Laon");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Amount");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        int getLoanAccountForId()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select AccountId from Account Where CustomerId = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        void clearLoanBoxes()
        {
            amountLoan_txt.Text = "";
            reasonCombo.Text = "";
            durationLoan.Text = "";
            PasswordLoan_txt.Text = "";
        }
        int daysForLoan()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT DATEDIFF(DAY, CreatedDate, GETDATE()) AS DaysSinceCreation FROM Account WHERE AccountId = @Id;", con);
            cmd.Parameters.AddWithValue("@Id", accountId());
            if(cmd.ExecuteScalar() == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        bool checkLoanRequestAmount(string amount, int duration)
        {
            if ((int.Parse(amount) > 50000) && duration == 0)
            {
                return false;
            }
            else if ((int.Parse(amount) > 100000) && duration == 1)
            {
                return false;
            }
            else if ((int.Parse(amount) > 200000) && duration == 2)
            {
                return false;
            }
            else if ((int.Parse(amount) > 200000) && duration == 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        void loadDataIntoGridForLoan()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Amount,Reason,Duration," +
                "CASE WHEN Loan.LoanStatus = '6' THEN 'Pending' When Loan.LoanStatus = '5' Then 'Approved' END AS Status," +
                "CASE WHEN Loan.PaymentStatus = '8' THEN 'Unpaid' When Loan.PaymentStatus = '7' Then 'Paid' END as [Payment Status] from Loan " +
                "join Account on Loan.AccountId = Account.AccountId Where Account.AccountId = @Id", con);
            cmd.Parameters.AddWithValue("@Id", accountId());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            loangridview.DataSource = dt;
        }

        private void guna2Button21_Click(object sender, EventArgs e)
        {
            if (guna2ComboBox2.Text == "" || richTextBox1.Text == "" || guna2TextBox5.Text == "")
            {
                MessageBox.Show("Please fill all the fields");
            }
            else
            {
                if (!checkDuplicationForRequest())
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("InsertIntoRequestChange", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@RequestType", guna2ComboBox2.Text);
                    cmd.Parameters.AddWithValue("@Description", richTextBox1.Text);
                    cmd.Parameters.AddWithValue("@UpdatedInfo", guna2TextBox5.Text);
                    cmd.Parameters.AddWithValue("@Status",6);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Request Sent Successfully");
                    showDataofRequestIntoGrid();
                    guna2ComboBox2.Text = "";
                    richTextBox1.Text = "";
                    guna2ComboBox2.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("You have already sent the request");
                }
            }
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            Login form = new Login();
            form.Show();
            this.Hide();
        }
        bool checkDuplicationOfLoan()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from Loan Where AccountId=@Id", con);
            cmd.Parameters.AddWithValue("@Id", accountId());
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        void showDataofRequestIntoGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select RequestType,Description,UpdatedInfo from ChangeRequest Where CustomerId = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            requestsgridview.DataSource = dt;

        }
        bool checkDuplicationForRequest()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from ChangeRequest Where CustomerId = @Id and RequestType =@Type", con);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Type", guna2ComboBox2.Text);
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
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
            cutomermenu.SelectedIndex = 5 ;
            laontotal_txt.Text = gettotalLoanAmount().ToString();
            loanunpaid_txt.Text = (gettotalLoanAmount() - getPayableLoanAmount()).ToString();
            DateTime dateTime = getRemainingDays();
            duedate_txt.Text = dateTime.ToString("dd-MMM-yyyy");

            laontotal_txt.Enabled = false;
            loanunpaid_txt.Enabled = false;
            duedate_txt.Enabled = false;
            loadLoanPaymentData();

        }
        void loadLoanPaymentData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select PayableAmount as Amount, Format(InstallmentOfLoan.Date,'dd-MMM-yyyy  hh:mm') as Date from InstallmentOfLoan join Loan on InstallmentOfLoan.LoanId = Loan.LoanId  Where Loan.AccountId = @Id", con);
            cmd.Parameters.AddWithValue("@Id", accountId());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            payLoanGridview.DataSource = dt;
        }
        DateTime getRemainingDays()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Date from Loan join Account on Loan.AccountId = Account.AccountId Where Account.CustomerId = @Id;", con);
            cmd.Parameters.AddWithValue("@Id", id);
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
               var con1 = Configuration.getInstance().getConnection();
                SqlCommand cmd1 = new SqlCommand("Select Duration from Loan join Account on Loan.AccountId = Account.AccountId" +
                    " Where Account.CustomerId = @Id;", con1);
                cmd1.Parameters.AddWithValue("@Id", id);
                object durationtype = cmd1.ExecuteScalar();
                if (durationtype != null)
                {
                    string durationString = durationtype.ToString();
                    if (durationString == "Six Month")
                    {
                        return Convert.ToDateTime(result).AddMonths(1);
                    }
                    else if (durationString == "One Year")
                    {
                        return Convert.ToDateTime(result).AddYears(1);
                    }
                    else if (durationString == "Two Year")
                    {
                        return Convert.ToDateTime(result).AddYears(2);
                    }
                    else
                    {
                        return Convert.ToDateTime(result).AddYears(5);
                    }
                }
                else
                {
                    return DateTime.Now;
                }
            }
            else
            {
                return DateTime.Now;
            }
        }
        decimal getPayableLoanAmount()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT SUM(PayableAmount) AS TotalAmount FROM InstallmentOfLoan " +
                "JOIN Loan ON Loan.LoanId = InstallmentOfLoan.LoanId join Account on Account.AccountId = " +
                "Loan.AccountId WHERE Account.AccountId = @Id GROUP BY Loan.LoanId; ", con);
            cmd.Parameters.AddWithValue("@Id", accountId());
            if (cmd.ExecuteScalar() == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }
        decimal gettotalLoanAmount()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Sum(Amount) from Loan Where AccountId = @Id", con);
            cmd.Parameters.AddWithValue("@Id", accountId());
            if(cmd.ExecuteScalar() == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }
        private void guna2Button10_Click(object sender, EventArgs e)
        {
            atmpanel.Show();
            atmtype_combobox.SelectedIndex = -1;
            guna2ComboBox3.SelectedIndex = -1;
            
        }

        private void guna2Button12_Click(object sender, EventArgs e)
        {
            changepasspanel.Show();
        }

        private void guna2Button26_Click(object sender, EventArgs e)
        {
            atmpanel.Hide();
            atmtype_combobox.Items.Clear();
        }

        private void guna2Button29_Click(object sender, EventArgs e)
        {
            changepasspanel.Hide();
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            feedbackpanel.Show();
        }

        private void guna2Button30_Click(object sender, EventArgs e)
        {
            feedbackpanel.Hide();
        }

        private void guna2Button24_Click(object sender, EventArgs e)
        {
            if(balancepayloan_txt.Text =="" || guna2TextBox1.Text == "")
            {
                MessageBox.Show("Please fill all the fields");
            }
            else
            {
                if(IsNumeric(guna2TextBox1.Text))
                {

                    if(checkExisttenceOfLoan())
                    {

                        if(checkLoanStatus())
                        {
                            if(checkLoanPaymentStatus())
                            {
                                if((checkBalance() - decimal.Parse(guna2TextBox1.Text)) > 0)
                                {
                                    decimal amount = gettotalLoanAmount() - (getInstallmentAmount() + decimal.Parse(guna2TextBox1.Text));
                                    if (amount >= 0)
                                    {
                                    
                                        var con = Configuration.getInstance().getConnection();
                                        SqlCommand cmd = new SqlCommand("InsertIntoInstallment", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@LoanId", getLoanId());
                                        cmd.Parameters.AddWithValue("@Amount", decimal.Parse(guna2TextBox1.Text));
                                        cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                                        cmd.ExecuteNonQuery();
                                        if(amount == 0)
                                        {
                                            var con1 = Configuration.getInstance().getConnection();
                                            SqlCommand cmd1 = new SqlCommand("Update Loan Set PaymentStatus = '7' Where LoanId = @Id", con1);
                                            cmd1.Parameters.AddWithValue("@Id", getLoanId());
                                            cmd1.ExecuteNonQuery();
                                        }
                                        var con2 = Configuration.getInstance().getConnection();
                                        SqlCommand cmd2 = new SqlCommand("UpdateBalance", con2);
                                        cmd2.CommandType = CommandType.StoredProcedure;
                                        cmd2.Parameters.AddWithValue("@Balance", checkBalance() - decimal.Parse(guna2TextBox1.Text));
                                        cmd2.Parameters.AddWithValue("@No", accounNo);
                                        cmd2.ExecuteNonQuery();
                                        MessageBox.Show("Loan Payment Successfully");
                                        loanunpaid_txt.Text = (gettotalLoanAmount() - getPayableLoanAmount()).ToString();
                                        loadLoanPaymentData();
                                        getDataOfPersonInfo();
                                        guna2TextBox1.Text = "";
                                    }
                                    else
                                    {
                                        MessageBox.Show("You have to pay the amount less than the payable amount. Remaining Amount:  " +amount );
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Insufficient Balance");
                                }
                            }
                            else
                            {
                                MessageBox.Show("You have already paid the loan");
                            }
                    
                        }
                        else
                        {
                            MessageBox.Show("Yours Loan Applicaiton is Still Pending");
                        }
                    }
                    else
                    {
                        MessageBox.Show("You have not taken the loan");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Amount");
                }
            }
        }
        decimal checkBalance()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select CurrentBalance from Account Where CustomerId = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            return Convert.ToDecimal(cmd.ExecuteScalar());
        }
        decimal getInstallmentAmount()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Sum(PayableAmount) as Amount from InstallmentOfLoan" +
                " Group By LoanId Having LoanId = @Id", con);
            cmd.Parameters.AddWithValue("@Id", getLoanId());
            if (cmd.ExecuteScalar() == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }

        }
        bool checkExisttenceOfLoan()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from Loan Where AccountId = @Id", con);
            cmd.Parameters.AddWithValue("@Id", accountId());
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        int getLoanId()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select LoanId from Loan Where AccountId = @Id", con);
            cmd.Parameters.AddWithValue("@Id", accountId());
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        bool checkLoanStatus()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from Loan Where AccountId = @Id and LoanStatus = '5'", con);
            cmd.Parameters.AddWithValue("@Id", accountId());
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool checkLoanPaymentStatus()
        { 
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from Loan Where AccountId = @Id and PaymentStatus = '8'", con);
            cmd.Parameters.AddWithValue("@Id", accountId());
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void guna2ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            atmtype_combobox.Items.Clear(); 
            if(guna2ComboBox3.SelectedIndex == 0)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select Name from LookUp Where Category = 'Debit Card';", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    atmtype_combobox.Items.Add(reader["Name"].ToString());
                }
                reader.Close();
            }
            else
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select Name from LookUp Where Category = 'Credit Card';", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    atmtype_combobox.Items.Add(reader["Name"].ToString());
                }
                reader.Close(); 
            }
        }

        private void guna2Button27_Click(object sender, EventArgs e)
        {
            if(guna2ComboBox3.Text == "" || atmtype_combobox.Text == "")
            {
                MessageBox.Show("Please fill all the fields");
            }
            else
            {
                if(checkExistenceOfCard())
                {
                    MessageBox.Show("You have already taken the card");
                }
                else
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("InsertIntoATMCard", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", accountId());
                    cmd.Parameters.AddWithValue("@Type", guna2ComboBox3.Text);
                    cmd.Parameters.AddWithValue("@CardType", getCardType());
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Status", 6);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Card Requested Successfully");
                    guna2ComboBox3.SelectedIndex = -1;
                    atmtype_combobox.SelectedIndex = -1;
                    atmpanel.Hide();
                    atmtype_combobox.Items.Clear();
                }
            }
        }
        bool checkExistenceOfCard()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Count(*) from ATMCard Where AccountId = @Id", con);
            cmd.Parameters.AddWithValue("@Id", accountId());
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        int getCardType()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id from LookUp Where Name = @Name", con);
            cmd.Parameters.AddWithValue("@Name", atmtype_combobox.Text);
            return Convert.ToInt32(cmd.ExecuteScalar());   
        }

        private void guna2Button28_Click(object sender, EventArgs e)
        {
            if(oldpass_txt.Text == "" || newPass_txt.Text == "")
            {
               MessageBox.Show("Please fill all the fields");
            }
            else
            {
                if(checkOldPassword() == oldpass_txt.Text)
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("UpdatePassword", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Password", newPass_txt.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Password Changed Successfully");
                    newPass_txt.Text = "";
                    oldpass_txt.Text = "";
                    changepasspanel.Hide();

                }
                else
                {
                    MessageBox.Show("Invalid Old Password");
                }
            }
        }
        string checkOldPassword()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Password from Customer Join UserId on Customer.CustomerId = UserId.Id Where CustomerId = @Id",con);
            cmd.Parameters.AddWithValue("@Id", id);
            return cmd.ExecuteScalar().ToString();
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            if(richTextBox2.Text == "")
            {
                MessageBox.Show("Plesse Write Something");
            }
            else
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("InsertIntoFeedback", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Description", richTextBox2.Text);
                cmd.ExecuteNonQuery();
                richTextBox2.Text = "";
                MessageBox.Show("Feedback Sent Successfully");
                feedbackpanel.Hide();

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
    }
}
