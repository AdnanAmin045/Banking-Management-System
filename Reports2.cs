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
    public partial class Reports2 : Form
    {
        public Reports2()
        {
            InitializeComponent();
        }

        private void Reports2_Load(object sender, EventArgs e)
        {
            SqlConnection con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(@"select A.AccountNo,T.Amount,L.Name AS TransactionType ,T.Date,A.BranchCode,G.Name AS Status 
from [Transaction] T JOIN LookUp L ON T.Type=L.Id 
JOIN Account A ON A.AccountId=T.AccountId 
JOIN LookUp G ON G.ID=T.Status  
where datediff(DAY,GETDATE(),T.Date)<30", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "TRANSACTION");
            report2 report = new report2();
            report.SetDataSource(ds);
            report.SummaryInfo.ReportTitle = "Monthly Transaction History Report";
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = report;
            crystalReportViewer1.Refresh();
        }
    }
}
