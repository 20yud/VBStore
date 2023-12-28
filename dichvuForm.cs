using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace VBStore
{
    public partial class dichvuForm : Form
    {
        private string connectionString;
        dbhelper dbHelper = new dbhelper();

        public dichvuForm()
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

                    // Thay đổi câu truy vấn và cách hiển thị dữ liệu
                    string query = "SELECT MALOAIDICHVU AS 'Mã dịch vụ', TENDICHVU AS 'Tên dịch vụ', DONGIA AS 'Đơn giá' FROM DICHVU";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            guna2DataGridView3.DataSource = dataTable;

                            // Đặt tên cho các cột
                            guna2DataGridView3.Columns[0].HeaderText = "Mã dịch vụ";
                            guna2DataGridView3.Columns[1].HeaderText = "Tên dịch vụ";
                            guna2DataGridView3.Columns[2].HeaderText = "Đơn giá";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void createBtn_Click(object sender, EventArgs e)
        {
            themDVForm themDV = new themDVForm();
            themDV.ShowDialog();
            dis();
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView3.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = guna2DataGridView3.SelectedRows[0];
                string maDichVu = selectedRow.Cells["Mã dịch vụ"].Value.ToString();

                // Truyền mã sản phẩm vào Form XoaTSForm khi mở Form này
                xoaDVForm xoaDV = new xoaDVForm(maDichVu);
                xoaDV.ShowDialog();
                dis();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void detailBtn_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView3.SelectedRows.Count > 0)
            {
                string maDichVu = guna2DataGridView3.SelectedRows[0].Cells["Mã dịch vụ"].Value.ToString();
                chitietDVForm chiTietForm = new chitietDVForm(maDichVu);
                chiTietForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dịch vụ để xem chi tiết.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void guna2DataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView3.SelectedRows.Count > 0)
            {
                string maSanPham = guna2DataGridView3.SelectedRows[0].Cells["Mã dịch vụ"].Value.ToString();
                suaDVForm suaDV = new suaDVForm(maSanPham);
                suaDV.ShowDialog();
                dis(); // Sau khi sửa thông tin, load lại dữ liệu
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để sửa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
