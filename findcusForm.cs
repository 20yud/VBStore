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
    public partial class findcusForm : Form
    {
        private string sdt;
        private string tenKH;
        private string connectionString;
        dbhelper dbHelper = new dbhelper();
        private ChildFormUtility childFormUtility;
        public findcusForm()
        {
            InitializeComponent();
        }
        public findcusForm(string sdt)
        {
            InitializeComponent();
            this.sdt = sdt;
            connectionString = dbHelper.ConnectionString;
            childFormUtility = new ChildFormUtility(this);
        }

        private void btnMuahang_Click(object sender, EventArgs e)
        {

        }

        private void findcusForm_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT TENKH, DIACHI, SDT, EMAIL FROM KHACHHANG WHERE SDT = @PhoneNumber";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Update the parameter with the current phone number (sdt)
                    command.Parameters.AddWithValue("@PhoneNumber", sdt);  // Use the updated sdt value

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nameTextBox.Text = reader.GetString(reader.GetOrdinal("TENKH"));
                            phoneNumberTextBox.Text = reader.GetString(reader.GetOrdinal("SDT"));
                            emailTextBox.Text = reader.GetString(reader.GetOrdinal("EMAIL"));
                            addressTextBox.Text = reader.GetString(reader.GetOrdinal("DIACHI"));
                            tenKH=nameTextBox.Text;
                        }

                    }
                }
            }

            // Update the tag values with the current data
            tenKH = nameTextBox.Text;
            sdt = phoneNumberTextBox.Text;
            nameTextBox.Tag = nameTextBox.Text;
            phoneNumberTextBox.Tag = phoneNumberTextBox.Text;
            emailTextBox.Tag = emailTextBox.Text;
            addressTextBox.Tag = addressTextBox.Text;

            WindowState = FormWindowState.Maximized;
        }

        private void muahangBtn_Click(object sender, EventArgs e)
        {
            muahangForm muahangForm = new muahangForm(sdt,tenKH);
            childFormUtility.OpenChildForm(muahangForm);
        }

        private void capnhapBtn_Click(object sender, EventArgs e)
        {
            suaCustomerForm suaCustomerform = new suaCustomerForm(tenKH);
            suaCustomerform.ShowDialog();
            suaCustomerform.StartPosition = FormStartPosition.CenterParent;
        }
    }
}
