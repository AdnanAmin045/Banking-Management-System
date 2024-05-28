using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_Final_Project
{
    public partial class Load : Form
    {
        int progress = 1;
        public Load()
        {
            InitializeComponent();
            timer1.Start();
        }


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            progressBar1.Value=progress;
            if (progressBar1.Value == 100)
            {
                timer1.Stop();
                Login l = new Login();
                l.Show();
                this.Hide();
            }
            label2.Text = progress + "%";
            progress += 1;
        }
    }
}
