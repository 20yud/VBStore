using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VBStore
{
    public partial class banhangForm : Form
    {
        private string connectionString;
        dbhelper dbHelper = new dbhelper();
        private string sdt;

        public banhangForm(string SDT)
        {
            InitializeComponent();
            connectionString = dbHelper.ConnectionString;
            this.sdt = SDT;

            // Gọi hàm để lấy thông tin mua hàng dựa trên số điện thoại
            DataTable purchaseInfo = GetPurchaseInfoByPhoneNumber(SDT);

            // Hiển thị dữ liệu lên DataGridView hoặc ListBox hoặc control tương tự
            if (purchaseInfo != null && purchaseInfo.Rows.Count > 0)
            {
                // Ví dụ DataGridView
                spKHmua.DataSource = purchaseInfo;

                // Nếu bạn muốn hiển thị tên sản phẩm thay vì mã sản phẩm, bạn có thể thực hiện một truy vấn SQL khác để lấy tên sản phẩm dựa trên mã sản phẩm.
                // Sau đó, bạn có thể ghép tên sản phẩm vào DataTable purchaseInfo trước khi hiển thị lên giao diện người dùng.
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin mua hàng cho số điện thoại này.");
            }
        }

        // Hàm lấy thông tin mua hàng dựa trên số điện thoại
        private DataTable GetPurchaseInfoByPhoneNumber(string phoneNumber)
        {
            DataTable purchaseInfo = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Tạo câu truy vấn SQL để lấy thông tin mua hàng
                    string query = @"SELECT CT.SOPHIEUBANHANG, PB.NGAYLAP, CT.MASANPHAM, SP.TENSP
                                     FROM CT_PHIEUBANHANG CT
                                     INNER JOIN PHIEUBANHANG PB ON CT.SOPHIEUBANHANG = PB.SOPHIEUBANHANG
                                     INNER JOIN SANPHAM SP ON CT.MASANPHAM = SP.MASANPHAM
                                     WHERE PB.MAKHACHHANG = (SELECT MAKHACHHANG FROM KHACHHANG WHERE SDT = @PhoneNumber)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(purchaseInfo);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy thông tin mua hàng: " + ex.Message);
                }
            }

            return purchaseInfo;
        }
    }
}
