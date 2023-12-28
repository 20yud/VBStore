using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using QRCoder;

namespace VBStore
{
    public partial class ThemTSForm : Form
    {
        private string connectionString;
        dbhelper dbHelper = new dbhelper();

        public ThemTSForm()
        {
            InitializeComponent();
            connectionString = dbHelper.ConnectionString;
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

                        string query = "INSERT INTO SANPHAM (MASANPHAM, TENSP, MALOAISANPHAM, DONGIABAN, DONGIAMUA, SOLUONGTON, MAQR) " +
                                       "VALUES (@MaSP, @TenSP, @MaLoaiSP, @DonGiaBan, @DonGiaMua, @SoLuongTon, @MaQR)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@MaSP", maSP);
                            command.Parameters.AddWithValue("@TenSP", tenSP);
                            command.Parameters.AddWithValue("@MaLoaiSP", maLoaiSP);
                            command.Parameters.AddWithValue("@DonGiaBan", dongiaBan);
                            command.Parameters.AddWithValue("@DonGiaMua", dongiaMua);
                            command.Parameters.AddWithValue("@SoLuongTon", soLuongTon);

                            // Tạo mã QR
                            string qrCodect = maSP; // Mã SP dùng làm nội dung mã QR
                            string qrFileName = qrCodect + ".png"; // Tên file mã QR

                            // Tạo đường dẫn lưu tạm file mã QR
                            string qrFilePath = Path.Combine(Path.GetTempPath(), qrFileName);

                            // Tạo mã QR và lưu thành file ảnh
                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodect, QRCodeGenerator.ECCLevel.Q);
                            QRCode qrCode = new QRCode(qrCodeData);
                            Bitmap qrImage = qrCode.GetGraphic(10);
                            qrImage.Save(qrFilePath, System.Drawing.Imaging.ImageFormat.Png);

                            // Tải ảnh lên Cloudinary
                            Account cloudinaryAccount = new Account("deayc8fkw", "587612639861316", "x7CCykyQrvK58ZI9J9-67J_4A8E"); // Thay thế bằng thông tin tài khoản Cloudinary của bạn
                            Cloudinary cloudinary = new Cloudinary(cloudinaryAccount);

                            var uploadParams = new ImageUploadParams()
                            {
                                File = new FileDescription(qrFilePath),
                                PublicId = "VBStore/" + qrCodect, // Tên public ID của ảnh trên Cloudinary
                            };

                            var uploadResult = cloudinary.Upload(uploadParams);

                            if (uploadResult != null)
                            {
                                string qrImageUrl = uploadResult.SecureUri.ToString();

                                command.Parameters.AddWithValue("@MaQR", qrImageUrl);

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
}