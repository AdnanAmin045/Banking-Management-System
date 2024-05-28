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
    public partial class Reports : Form
    {
        public Reports()
        {
            InitializeComponent();
        }

        private void Reports_Load(object sender, EventArgs e)
        {
            
            SqlConnection con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(@"SELECT  CONCAT(P.FirstName,' ',P.LastName ) AS Name,
C.CNIC, A.AccountNo, A.AccountType, A.BranchCode, A.CreatedDate
FROM Account A JOIN Customer C
ON A.CustomerId = C.CustomerId
JOIN PersonInfo P ON P.Id = C.CustomerId
WHERE DATEDIFF(DAY, A.CreatedDate, GETDATE()) <= 7", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "RecentAccount");
            report1 report = new report1();
            report.SetDataSource(ds);
            report.SummaryInfo.ReportTitle = "Recently Opened Accounts Report";
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = report;
            crystalReportViewer1.Refresh();
        
    }
    }
}
