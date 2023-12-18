using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace VBStore
{
    public partial class mainForm : Form
    {
        private string connectionString = "Data Source=DESKTOP-KRAFR0M\\MSSQLSERVER1;Initial Catalog=CNPM_DB;Integrated Security=True";
        public mainForm()
        {
            InitializeComponent();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
