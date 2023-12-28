﻿using System;
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
    public partial class themCustomerForm : Form
    {
        private string connectionString;
        dbhelper dbHelper = new dbhelper();
        public themCustomerForm()
        {
            InitializeComponent();
            connectionString = dbHelper.ConnectionString;

        }

        private string sdt;
        public themCustomerForm(string sdt)
        {
            InitializeComponent();
            connectionString = dbHelper.ConnectionString;
            this.sdt = sdt;
        }

        private void customerForm_Load()
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
                        }
                    }
                }
            }

            nameTextBox.Tag = nameTextBox.Text;
            phoneNumberTextBox.Tag = phoneNumberTextBox.Text;
            emailTextBox.Tag = emailTextBox.Text;
            addressTextBox.Tag = addressTextBox.Text;
            
            WindowState = FormWindowState.Maximized;

        }

        private bool isEditing = false;

        private void editBtn_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem textbox nào đã được nhập dữ liệu mới
            bool isNameChanged = nameTextBox.Text != nameTextBox.Tag.ToString();
            bool isPhoneNumberChanged = phoneNumberTextBox.Text != phoneNumberTextBox.Tag.ToString();
            bool isEmailChanged = emailTextBox.Text != emailTextBox.Tag.ToString();
            bool isAddressChanged = addressTextBox.Text != addressTextBox.Tag.ToString();

            // Kiểm tra xem ít nhất một textbox đã được thay đổi
            if (!isNameChanged && !isPhoneNumberChanged && !isEmailChanged && !isAddressChanged)
            {
                MessageBox.Show("Vui lòng nhập dữ liệu để chỉnh sửa.", "Thông báo");
                return;
            }

            // Kết nối đến cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Kiểm tra xem dữ liệu đã nhập mới đã tồn tại trong cơ sở dữ liệu hay chưa
                if (isPhoneNumberChanged)
                {
                    string checkQuery = "SELECT COUNT(*) FROM KHACHHANG WHERE SDT = @PhoneNumber AND SDT != @OriginalPhoneNumber";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@PhoneNumber", phoneNumberTextBox.Text);
                        checkCommand.Parameters.AddWithValue("@OriginalPhoneNumber", sdt);

                        int existingCount = (int)checkCommand.ExecuteScalar();
                        if (existingCount > 0)
                        {
                            MessageBox.Show("Số điện thoại đã tồn tại trong cơ sở dữ liệu.", "Thông báo");
                            return;
                        }
                    }
                }

                // Tạo câu truy vấn SQL để cập nhật thông tin khách hàng
                string updateQuery = "UPDATE KHACHHANG SET TENKH = @Ten, DIACHI = @DiaChi, EMAIL = @Email, SDT = @PhoneNumber WHERE SDT = @OriginalPhoneNumber";

                using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                {
                    // Thêm tham số vào câu truy vấn
                    updateCommand.Parameters.AddWithValue("@Ten", nameTextBox.Text);
                    updateCommand.Parameters.AddWithValue("@DiaChi", addressTextBox.Text);
                    updateCommand.Parameters.AddWithValue("@Email", emailTextBox.Text);
                    updateCommand.Parameters.AddWithValue("@PhoneNumber", phoneNumberTextBox.Text);
                    updateCommand.Parameters.AddWithValue("@OriginalPhoneNumber", sdt);

                    // Thực thi câu truy vấn
                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thông tin khách hàng đã được cập nhật thành công.", "Thông báo");

                        // Cập nhật giá trị Tag cho các textbox
                        nameTextBox.Tag = nameTextBox.Text;
                        phoneNumberTextBox.Tag = phoneNumberTextBox.Text;
                        emailTextBox.Tag = emailTextBox.Text;
                        addressTextBox.Tag = addressTextBox.Text;

                        // Đặt lại trạng thái chỉnh sửa
                        isEditing = false;
                    }
                    else
                    {
                        MessageBox.Show("Không thể cập nhật thông tin khách hàng.", "Lỗi");
                    }
                }
            }
        }

        private void ReloadFormData()
        {
            // Kết nối đến cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Tạo câu truy vấn SQL để lấy dữ liệu khách hàng dựa trên số điện thoại
                string query = "SELECT TENKH, SDT, EMAIL, DIACHI FROM KHACHHANG WHERE SDT = @PhoneNumber";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thêm tham số vào câu truy vấn
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumberTextBox.Text);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Đặt giá trị cho các control trên Form từ dữ liệu khách hàng
                            nameTextBox.Text = reader["TENKH"].ToString();
                            nameTextBox.Tag = reader["TENKH"].ToString();
                            phoneNumberTextBox.Text = reader["SDT"].ToString();
                            phoneNumberTextBox.Tag = reader["SDT"].ToString();
                            emailTextBox.Text = reader["EMAIL"].ToString();
                            emailTextBox.Tag = reader["EMAIL"].ToString();
                            addressTextBox.Text = reader["DIACHI"].ToString();
                            addressTextBox.Tag = reader["DIACHI"].ToString();
                        }
                    }
                }
            }
        }
       

        private void delBtn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                // Kết nối đến cơ sở dữ liệu
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Tạo câu truy vấn SQL để xóa khách hàng
                    string query = "DELETE FROM KHACHHANG WHERE SDT = @PhoneNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm tham số vào câu truy vấn
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumberTextBox.Text);

                        // Thực thi câu truy vấn
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Khách hàng đã được xóa thành công.", "Thông báo");
                            // Đóng form khách hàng sau khi xóa thành công
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Không thể xóa khách hàng.", "Lỗi");
                        }
                    }
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) || string.IsNullOrWhiteSpace(phoneNumberTextBox.Text))
            {
                MessageBox.Show("Tên và số điện thoại là bắt buộc. Vui lòng nhập đầy đủ thông tin.", "Thông báo");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM KHACHHANG WHERE SDT = @PhoneNumber";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@PhoneNumber", phoneNumberTextBox.Text);

                    int existingCount = (int)checkCommand.ExecuteScalar();
                    if (existingCount > 0)
                    {
                        MessageBox.Show("Số điện thoại đã tồn tại trong cơ sở dữ liệu.", "Thông báo");
                        return;
                    }
                }

                // Insert new customer record
                string insertQuery = "EXEC SP_INSERT_KHACHHANG @Ten, @DiaChi, @PhoneNumber,@Email;";
                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@Ten", nameTextBox.Text);
                    insertCommand.Parameters.AddWithValue("@DiaChi", addressTextBox.Text);
                    insertCommand.Parameters.AddWithValue("@Email", emailTextBox.Text);
                    insertCommand.Parameters.AddWithValue("@PhoneNumber", phoneNumberTextBox.Text);

                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thông tin khách hàng đã được lưu thành công.", "Thông báo");
                    }
                    else
                    {
                        MessageBox.Show("Không thể lưu thông tin khách hàng.", "Lỗi");
                    }
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}