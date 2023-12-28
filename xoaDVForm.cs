using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VBStore
{
    public partial class xoaDVForm : Form
    {
        private string maLoaiDV;
        private string connectionString;
        dbhelper dbHelper = new dbhelper();
        public xoaDVForm(string maDV)
        {
            InitializeComponent();
            maLoaiDV = maDV;
            connectionString = dbHelper.ConnectionString;
            LoadCTDV();
        }

        private void LoadCTDV()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM DICHVU WHERE MALOAIDICHVU = @MaLoaiDV";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaLoaiDV", maLoaiDV);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtMaDV.Text = reader["MALOAIDICHVU"].ToString();
                                txtTenDV.Text = reader["TENDICHVU"].ToString();
                                txtDonGia.Text = reader["DONGIA"].ToString();
                                // Cập nhật các trường thông tin khác nếu cần
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy thông tin dịch vụ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close(); // Đóng form nếu không tìm thấy thông tin
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close(); // Đóng form nếu có lỗi
            }
        }

        private void createBtn_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn dịch vụ này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Thực hiện câu lệnh SQL để xóa sản phẩm
                if (DeleteProduct(maLoaiDV))
                {
                    MessageBox.Show("Xóa dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); // Đóng Form XoaTSForm sau khi xóa thành công
                }
                else
                {
                    MessageBox.Show("Lỗi khi xóa dịch vụ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool DeleteProduct(string maLoaiDV)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Thực hiện câu lệnh SQL để xóa dịch vụ
                    string query = "DELETE FROM DICHVU WHERE MALOAIDICHVU = @MaLoaiDV";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaLoaiDV", maLoaiDV);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0; // Trả về true nếu có ít nhất một dòng bị ảnh hưởng (xóa thành công)
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Trả về false nếu có lỗi xảy ra
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void xoaDVForm_Load(object sender, EventArgs e)
        {

        }
    }
}
