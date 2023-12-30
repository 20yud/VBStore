using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace VBStore
{
    public partial class dichvubookedForm : Form
    {
        private string connectionString;
        dbhelper dbHelper = new dbhelper();

        public dichvubookedForm()
        {
            InitializeComponent();
            connectionString = dbHelper.ConnectionString;
            dis();
        }

        private void dis()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Câu truy vấn SQL để lấy các cột cụ thể từ hai bảng
                    string query = "SELECT " +
                                   "PHIEUDICHVU.SOPHIEUDICHVU, PHIEUDICHVU.NGAYLAP, PHIEUDICHVU.TINHTRANG, " +
                                   "CT_PHIEUDICHVU.MALOAIDICHVU, CT_PHIEUDICHVU.DONGIADICHVU, " +
                                   "CT_PHIEUDICHVU.DONGIADUOCTINH, CT_PHIEUDICHVU.SOLUONG, " +
                                   "CT_PHIEUDICHVU.THANHTIEN, CT_PHIEUDICHVU.TRATRUOC, " +
                                   "CT_PHIEUDICHVU.CONLAI, CT_PHIEUDICHVU.NGAYGIAO " +
                                   "FROM PHIEUDICHVU " +
                                   "INNER JOIN CT_PHIEUDICHVU ON PHIEUDICHVU.SOPHIEUDICHVU = CT_PHIEUDICHVU.SOPHIEUDICHVU";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Đặt dữ liệu từ DataTable cho guna2DataGridView1
                        guna2DataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void updateBtn_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                string maSanPham = guna2DataGridView1.SelectedRows[0].Cells["SOPHIEUDICHVU"].Value.ToString();
                capnhapdvForm suaDV = new capnhapdvForm(maSanPham);

                // Check if the suaDVForm is already closed or disposed before showing it
                if (!suaDV.IsDisposed)
                {
                    suaDV.ShowDialog();
                    dis();
                }
                else
                {
                    MessageBox.Show("The suaDVForm has been closed.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để sửa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void findTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Dynamically adjust the SQL query based on the entered text
                    string query = "SELECT " +
                                   "PHIEUDICHVU.SOPHIEUDICHVU, PHIEUDICHVU.NGAYLAP, PHIEUDICHVU.TINHTRANG, " +
                                   "CT_PHIEUDICHVU.MALOAIDICHVU, CT_PHIEUDICHVU.DONGIADICHVU, " +
                                   "CT_PHIEUDICHVU.DONGIADUOCTINH, CT_PHIEUDICHVU.SOLUONG, " +
                                   "CT_PHIEUDICHVU.THANHTIEN, CT_PHIEUDICHVU.TRATRUOC, " +
                                   "CT_PHIEUDICHVU.CONLAI, CT_PHIEUDICHVU.NGAYGIAO" +
                                   "FROM PHIEUDICHVU " +
                                   "INNER JOIN CT_PHIEUDICHVU ON PHIEUDICHVU.SOPHIEUDICHVU = CT_PHIEUDICHVU.SOPHIEUDICHVU";

                    if (!string.IsNullOrEmpty(findTextBox.Text))
                    {
                        string searchText = findTextBox.Text.Trim();

                        // Add a condition to filter based on the SOPHIEUDICHVU
                        query += $@" WHERE PHIEUDICHVU.SOPHIEUDICHVU LIKE '%{searchText}%'";
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Set the data source for guna2DataGridView1
                        guna2DataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
