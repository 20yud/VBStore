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
    public partial class themDVForm : Form
    {
        private string connectionString;
        dbhelper dbHelper = new dbhelper();
        public themDVForm()
        {
            InitializeComponent();
            connectionString = dbHelper.ConnectionString;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtMaDV_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTenDV_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDonGia_TextChanged(object sender, EventArgs e)
        {

        }

        private void createBtn_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ các controls trên form
            string maDV = txtMaDV.Text;
            string tenDV = txtTenDV.Text;
            decimal donGia = decimal.Parse(txtDonGia.Text);

            // Thực hiện kiểm tra dữ liệu đầu vào nếu cần

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Câu lệnh SQL để thêm dịch vụ
                    string query = "INSERT INTO DICHVU (MALOAIDICHVU, TENDICHVU, DONGIA) " +
                                   "VALUES (@MaDV, @TenDV, @DonGia)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaDV", maDV);
                        command.Parameters.AddWithValue("@TenDV", tenDV);
                        command.Parameters.AddWithValue("@DonGia", donGia);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Đóng Form khi thêm thành công (tùy theo yêu cầu của bạn)
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Thêm dịch vụ thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
