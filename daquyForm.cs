using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace VBStore
{
    public partial class daquyForm : Form
    {
        private string sdt;
        private string connectionString = "Data Source=DESKTOP-KRAFR0M\\MSSQLSERVER1;Initial Catalog=vbstore;Integrated Security=True";

        public daquyForm()
        {
            InitializeComponent();
        }

        private void daquyForm_Load(object sender, EventArgs e)
        {
            LoadProducts(); // Gọi phương thức để load dữ liệu sản phẩm
        }

        private void LoadProducts()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Thay đổi điều kiện để chỉ lấy sản phẩm có mã LSP01 hoặc LSP02
                    string query = "SELECT SANPHAM.MASANPHAM AS 'Mã sản phẩm', " +
                                   "TENSP AS 'Tên sản phẩm', " +
                                   "DONGIABAN AS 'Đơn giá bán', " +
                                   "DONGIAMUA AS 'Đơn giá mua', " +
                                   "SOLUONGTON AS 'Số lượng tồn' " +
                                   "FROM SANPHAM INNER JOIN LOAISANPHAM ON SANPHAM.MALOAISANPHAM = LOAISANPHAM.MALOAISANPHAM " +
                                   "WHERE SANPHAM.MALOAISANPHAM IN ('LSP03')";

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
                                guna2DataGridView1.Columns[0].HeaderText = "Mã sản phẩm";
                                guna2DataGridView1.Columns[1].HeaderText = "Tên sản phẩm";
                                guna2DataGridView1.Columns[2].HeaderText = "Đơn giá bán";
                                guna2DataGridView1.Columns[3].HeaderText = "Đơn giá mua";
                                guna2DataGridView1.Columns[4].HeaderText = "Số lượng tồn";
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

        private void createBtn_Click(object sender, EventArgs e)
        {
            themDQFrom themDQ = new themDQFrom();
            themDQ.ShowDialog();
        }
        private void detailBtn_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                string maSanPham = guna2DataGridView1.SelectedRows[0].Cells["Mã sản phẩm"].Value.ToString();
                chitietDQForm chiTietForm = new chitietDQForm(maSanPham);
                chiTietForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xem chi tiết.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void editBtn_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                string maSanPham = guna2DataGridView1.SelectedRows[0].Cells["Mã sản phẩm"].Value.ToString();
                suaDQForm suaTSForm = new suaDQForm(maSanPham);
                suaTSForm.ShowDialog();
                LoadProducts(); // Sau khi sửa thông tin, load lại dữ liệu
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để sửa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];
                string maSanPham = selectedRow.Cells["Mã sản phẩm"].Value.ToString();

                // Truyền mã sản phẩm vào Form XoaTSForm khi mở Form này
                xoaDQForm xoaTSForm = new xoaDQForm(maSanPham);
                xoaTSForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}