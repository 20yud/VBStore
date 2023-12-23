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
using System.IO;

namespace VBStore
{
    public partial class ThemTSForm : Form
    {
        private string connectionString = "Data Source=DESKTOP-C753LHT\\SQLEXPRESS;Initial Catalog=vbstore;Integrated Security=True";

        public ThemTSForm()
        {
            InitializeComponent();
            LoadLoaiSanPham(); // Load danh sách loại sản phẩm vào ListBox
        }

        private void LoadLoaiSanPham()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT MALOAISANPHAM, TENLOAISANPHAM FROM LOAISANPHAM";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string maLoai = reader["MALOAISANPHAM"].ToString();
                                string tenLoai = reader["TENLOAISANPHAM"].ToString();
                                cmbLoaiSP.Items.Add(new LoaiSanPhamItem(maLoai, tenLoai));
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

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maSP = txtMaSP.Text;
            string tenSP = txtTenSP.Text;

            // Check if an item is selected in the ComboBox cmbLoaiSP
            if (cmbLoaiSP.SelectedItem != null)
            {
                string maLoaiSP = ((LoaiSanPhamItem)cmbLoaiSP.SelectedItem).MaLoai;
                decimal dongiaBan = decimal.Parse(txtDonGiaBan.Text);
                decimal dongiaMua = decimal.Parse(txtDonGiaMua.Text);
                int soLuongTon = int.Parse(txtSoLuongTon.Text);

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "INSERT INTO SANPHAM (MASANPHAM, TENSP, MALOAISANPHAM, DONGIABAN, DONGIAMUA, SOLUONGTON) " +
                                       "VALUES (@MaSP, @TenSP, @MaLoaiSP, @DonGiaBan, @DonGiaMua, @SoLuongTon)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@MaSP", maSP);
                            command.Parameters.AddWithValue("@TenSP", tenSP);
                            command.Parameters.AddWithValue("@MaLoaiSP", maLoaiSP);
                            command.Parameters.AddWithValue("@DonGiaBan", dongiaBan);
                            command.Parameters.AddWithValue("@DonGiaMua", dongiaMua);
                            command.Parameters.AddWithValue("@SoLuongTon", soLuongTon);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                // Đóng Form khi thêm thành công (tùy theo yêu cầu của bạn)
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Thêm sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public class LoaiSanPhamItem
        {
            public string MaLoai { get; set; }
            public string TenLoai { get; set; }

            public LoaiSanPhamItem(string maLoai, string tenLoai)
            {
                MaLoai = maLoai;
                TenLoai = tenLoai;
            }

            public override string ToString()
            {
                return TenLoai;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    // Class để lưu thông tin loại sản phẩm
   
}