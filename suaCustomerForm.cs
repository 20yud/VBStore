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
    public partial class suaCustomerForm : Form
    {
        private string maKhachHang;
        private string connectionString = "Data Source=DESKTOP-C753LHT\\SQLEXPRESS;Initial Catalog=vbstore;Integrated Security=True";
        public suaCustomerForm(string maKH)
        {
            InitializeComponent();
            maKhachHang = maKH;
            LoadCustomerDetails();
        }
        private void LoadCustomerDetails()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * " +
                                   "FROM KHACHHANG " +
                                   "WHERE KHACHHANG.MAKHACHHANG = @MaKhachHang";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaKhachHang", maKhachHang);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Hiển thị chi tiết sản phẩm và loại sản phẩm trên Form
                                // txtMaKH.Text = reader["MAKHACHHANG"].ToString();
                                txtTenKH.Text = reader["TENKH"].ToString();
                                txtDiaChi.Text = reader["DIACHI"].ToString();
                                txtSDT.Text = reader["SDT"].ToString();
                                txtEmail.Text = reader["EMAIL"].ToString();
                                // Thêm các control khác tương ứng
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            if (IsDataValid()) // Kiểm tra xem đã nhập đủ dữ liệu hay không
            {
                // Thực hiện cập nhật dữ liệu vào cơ sở dữ liệu
                if (UpdateCustomerData())
                {
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); // Đóng Form sau khi cập nhật thành công
                }
                else
                {
                    MessageBox.Show("Lỗi khi cập nhật dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin cần cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private bool UpdateCustomerData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE KHACHHANG " +
                                   "SET TENKH = @TenKH, DIACHI = @DiaChi, SDT = @SDT, EMAIL = @Email " +
                                   "WHERE MAKHACHHANG = @MaKhachHang";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Đặt giá trị cho các tham số
                        command.Parameters.AddWithValue("@TenKH", txtTenKH.Text);
                        command.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                        command.Parameters.AddWithValue("@SDT", txtSDT.Text);
                        command.Parameters.AddWithValue("@Email", txtEmail.Text);
                        command.Parameters.AddWithValue("@MaKhachHang", maKhachHang);

                        // Thực hiện câu lệnh SQL
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0; // Trả về true nếu có ít nhất một dòng bị ảnh hưởng (cập nhật thành công)
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Trả về false nếu có lỗi xảy ra
            }
        }
        private bool IsDataValid()
        {
            // Kiểm tra xem đã nhập đủ thông tin hay không
            return !string.IsNullOrEmpty(txtTenKH.Text) && !string.IsNullOrEmpty(txtDiaChi.Text) &&
                   !string.IsNullOrEmpty(txtEmail.Text) && !string.IsNullOrEmpty(txtSDT.Text);
            // Thêm kiểm tra cho các control khác nếu cần
        }
    }
}
