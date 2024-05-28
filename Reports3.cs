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
    public partial class Reports3 : Form
    {
        public Reports3()
        {
            InitializeComponent();
        }

        private void Reports3_Load(object sender, EventArgs e)
        {
            SqlConnection con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(@"SELECT CONCAT(P.FirstName,' ',P.LastName)AS PersonNAME,CR.RequestType,CR.Description,CR.UpdatedInfo,L.Name AS STATUS FROM ChangeRequest CR
JOIN CUSTOMER C ON CR.CustomerId=C.CustomerId
JOIN Account A ON A.CustomerId=C.CustomerId
JOIN PersonInfo P ON P.Id=C.CustomerId
JOIN LookUp L ON L.Id=CR.Status", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "AccountActivity");
            report1 report = new report1();
            report.SetDataSource(ds);
            report.SummaryInfo.ReportTitle = "Account Activity Report";
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = report;
            crystalReportViewer1.Refresh();
        }
    }
}
