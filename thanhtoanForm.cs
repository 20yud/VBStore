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
    public partial class thanhtoanForm : Form
    {
        private string connectionString;
        dbhelper dbHelper = new dbhelper();
        private string sdt;
        private string tenKH;
        private DataTable dtGioHangThanhToan;

        public DataTable GioHangThanhToan
        {
            get { return dtGioHangThanhToan; }
            set { dtGioHangThanhToan = value; }
        }
        public thanhtoanForm()
        {
            InitializeComponent();
        }
        public thanhtoanForm(string sdt, string tenkh)
        {
            InitializeComponent();
            this.sdt = sdt;
            this.tenKH = tenkh;
            tenkhlabel.Text = tenKH;
            sdtlabel.Text = sdt;
            connectionString = dbHelper.ConnectionString;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Kiểm tra nếu giỏ hàng không null
            if (GioHangThanhToan != null)
            {
                // Gán giỏ hàng cho DataGridView hoặc bất kỳ điều gì bạn muốn hiển thị
                dgvGioHangThanhToan.DataSource = GioHangThanhToan;
            }
        }

        private void btnDaThanhToan_Click(object sender, EventArgs e)
        {
           
        }
        private string GenerateNewMaPhieuBanHangMain()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Determine the maximum invoice number
                string query = "SELECT MAX(CAST(SUBSTRING(SOPHIEUBANHANG, 4, LEN(SOPHIEUBANHANG)) AS INT)) FROM PHIEUBANHANG";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    int maxNumber = (result != DBNull.Value) ? Convert.ToInt32(result) : 0;

                    // Increment the maximum number and create the new invoice ID
                    int newNumber = maxNumber + 1;
                    string newInvoiceID = $"PBH{newNumber:D6}";

                    return newInvoiceID;
                }
            }
        }
        private void UpdateQuantityInDatabase(SqlConnection connection, SqlTransaction transaction)
        {
            if (GioHangThanhToan.Columns.Contains("MASANPHAM") &&
                GioHangThanhToan.Columns.Contains("SOLUONG"))
            {
                foreach (DataRow row in GioHangThanhToan.Rows)
                {
                    string maSanPham = row["MASANPHAM"].ToString();
                    int soLuongBan = Convert.ToInt32(row["SOLUONG"]);

                    string updateQuery = $"UPDATE SANPHAM SET SOLUONGTON = SOLUONGTON - {soLuongBan} WHERE MASANPHAM = '{maSanPham}'";

                    // Use the existing connection and transaction
                    ExecuteNonQuery(updateQuery, connection, transaction);
                }
            }
            else
            {
                MessageBox.Show("Columns 'MASANPHAM' and 'SOLUONG' do not exist in the DataTable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private string GetMaKHFromSDT(string sdt)
        {
            // Execute a query to get maKH from the database based on the provided sdt
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT MAKHACHHANG FROM KHACHHANG WHERE SDT = '{sdt}'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }

        private void InsertPhieuBanHang(SqlConnection connection, SqlTransaction transaction, string maPhieuBanHang)
        {
            string maKH = GetMaKHFromSDT(sdt);

            // Check if the record already exists
            string checkExistQuery = $"SELECT COUNT(*) FROM PHIEUBANHANG WHERE SOPHIEUBANHANG = '{maPhieuBanHang}' AND MAKHACHHANG = '{maKH}'";
            using (SqlCommand checkExistCommand = new SqlCommand(checkExistQuery, connection, transaction))
            {
                int count = Convert.ToInt32(checkExistCommand.ExecuteScalar());
                if (count == 0)
                {
                    // Execute an INSERT query to add a new record to the PHIEUBANHANG table
                    string insertQuery = $"INSERT INTO PHIEUBANHANG (SOPHIEUBANHANG, MAKHACHHANG, NGAYLAP, TONGTIEN) VALUES ('{maPhieuBanHang}', '{maKH}', GETDATE(), 0)";
                    ExecuteNonQuery(insertQuery, connection, transaction);
                }
            }
        }

        private void InsertCTPhieuBanHang(SqlConnection connection, SqlTransaction transaction, string maPhieuBanHang, string maSanPham, int soLuongBan, decimal donGiaBan, decimal thanhTien)
        {
            // Execute an INSERT query to add a new record to the CT_PHIEUBANHANG table
            string insertQuery = $"INSERT INTO CT_PHIEUBANHANG (SOPHIEUBANHANG, MASANPHAM, SOLUONGBAN, DONGIABAN, THANHTIEN) " +
                $"VALUES ('{maPhieuBanHang}', '{maSanPham}', {soLuongBan}, {donGiaBan}, {thanhTien})";

            // Use the existing connection and transaction
            ExecuteNonQuery(insertQuery, connection, transaction);
        }

        private void ExecuteNonQuery(string query, SqlConnection connection, SqlTransaction transaction)
        {
            // Implement code to execute the provided SQL query on your database
            // This depends on the method you are using to interact with the database (e.g., ADO.NET SqlCommand)
            // Example:
            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
