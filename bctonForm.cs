using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace VBStore
{
    public partial class bctonForm : Form
    {
        private string connectionString;
        dbhelper dbHelper = new dbhelper();

        public bctonForm()
        {
            InitializeComponent();
            connectionString = dbHelper.ConnectionString;
            dis();
        }

        private void dis()
        {
           
        }
    }
}
