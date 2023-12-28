using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace VBStore
{
    public partial class customerForm : Form
    {
        private string sdt;
        private string connectionString;
        dbhelper dbHelper = new dbhelper();
        public customerForm()
        {
            InitializeComponent();
            connectionString = dbHelper.ConnectionString;

        }

        private void customerForm_Load(object sender, EventArgs e)
        {
            loadCustomer();
        }

        private void loadCustomer()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT MAKHACHHANG AS 'Mã khách hàng', " +
                                   "TENKH AS 'Tên khách hàng', " +
                                   "DIACHI AS 'Địa chỉ', " +
                                   "SDT AS 'Số điện thoại', " +
                                   "EMAIL AS 'Email' " +
                                   "FROM KHACHHANG ";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            guna2DataGridView1.DataSource = dataTable;
                            // Đặt tên cho các cột
                            if (guna2DataGridView1.Columns.Count >= 5)
                            {
                                guna2DataGridView1.Columns[0].HeaderText = "Mã khách hàng";
                                guna2DataGridView1.Columns[1].HeaderText = "Tên tên khách hàng";
                                guna2DataGridView1.Columns[2].HeaderText = "Địa chỉ";
                                guna2DataGridView1.Columns[3].HeaderText = "Số điện thoại";
                                guna2DataGridView1.Columns[4].HeaderText = "Email";
                            }

                            // Đổ dữ liệu vào guna2DataGridView1

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}