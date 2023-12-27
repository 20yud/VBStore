using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using BarcodeLib;
using QRCoder;
using ZXing;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace VBStore
{
    public partial class muahangForm : Form
    {
        private DataTable dtSanPham;
        private DataTable dtGioHang;
        private string sdt;
        private string tenKH;
        private string connectionString;
        dbhelper dbHelper = new dbhelper();
        private string qrstring;
        FilterInfoCollection cameras;
        VideoCaptureDevice cam;
        BarcodeLib.Barcode code128;
        private Capture cap;
        private decimal transferAmount;


        public muahangForm()
        {
            InitializeComponent();
            InitializeDataTables();
            LoadSanPham();
        }
        public muahangForm(string sdt, string tenKH)
        {
            InitializeComponent();
            InitializeDataTables();
            LoadSanPham();
            this.sdt = sdt;
            this.tenKH = tenKH;
            tenkhlabel.Text = tenKH;
            sdtlabel.Text = sdt;
            connectionString = dbHelper.ConnectionString;
            cameras = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo info in cameras)
            {
                comboBox1.Items.Add(info.Name);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void InitializeDataTables()
        {
            // DataTable cho sản phẩm
            dtSanPham = new DataTable();
            dtSanPham.Columns.Add("MASANPHAM", typeof(string));
            dtSanPham.Columns.Add("TENSP", typeof(string));
            dtSanPham.Columns.Add("DONGIABAN", typeof(decimal));

            // DataTable cho giỏ hàng
            dtGioHang = new DataTable();
            dtGioHang.Columns.Add("MASANPHAM", typeof(string));
            dtGioHang.Columns.Add("TENSP", typeof(string));
            dtGioHang.Columns.Add("DONGIABAN", typeof(decimal));
            dtGioHang.Columns.Add("SOLUONG", typeof(int));
            dtGioHang.Columns.Add("THANHTIEN", typeof(decimal));

            // Gán DataTables cho DataGridViews
            dgvSanPham.DataSource = dtSanPham;
            dgvGioHang.DataSource = dtGioHang;
        }

        private void LoadSanPham()
        {
            // Thực hiện truy vấn để lấy dữ liệu sản phẩm từ database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT MASANPHAM, TENSP, DONGIABAN FROM SANPHAM";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                // Đổ dữ liệu vào DataTable dtSanPham
                while (reader.Read())
                {
                    dtSanPham.Rows.Add(reader["MASANPHAM"], reader["TENSP"], reader["DONGIABAN"]);
                }

                reader.Close();
            }
            
        }

        private void btnThemSanPham_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng đã chọn sản phẩm từ DataGridView dgvSanPham chưa
            if (dgvSanPham.SelectedRows.Count > 0)
            {
                // Lấy thông tin sản phẩm được chọn
                DataRowView selectedRow = dgvSanPham.SelectedRows[0].DataBoundItem as DataRowView;

                // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng chưa
                DataRow existingRow = dtGioHang.AsEnumerable().FirstOrDefault(row => row["MASANPHAM"].ToString() == selectedRow["MASANPHAM"].ToString());

                if (existingRow != null)
                {
                    // Nếu sản phẩm đã tồn tại trong giỏ hàng, tăng số lượng
                    existingRow["SOLUONG"] = Convert.ToInt32(existingRow["SOLUONG"]) + 1;
                    existingRow["THANHTIEN"] = Convert.ToDecimal(existingRow["SOLUONG"]) * Convert.ToDecimal(existingRow["DONGIABAN"]);
                }
                else
                {
                    // Nếu sản phẩm chưa có trong giỏ hàng, thêm mới
                    DataRow newRow = dtGioHang.NewRow();
                    newRow["MASANPHAM"] = selectedRow["MASANPHAM"];
                    newRow["TENSP"] = selectedRow["TENSP"];
                    newRow["DONGIABAN"] = selectedRow["DONGIABAN"];
                    newRow["SOLUONG"] = 1;
                    newRow["THANHTIEN"] = Convert.ToDecimal(newRow["DONGIABAN"]) * Convert.ToInt32(newRow["SOLUONG"]);

                    dtGioHang.Rows.Add(newRow);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm từ danh sách để thêm vào giỏ hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnXacthuc_Click(object sender, EventArgs e)
        {
            thanhtoanForm thanhToanForm = new thanhtoanForm(sdt, tenKH);

            // Gán giỏ hàng từ muahangForm sang thanhtoanForm
            thanhToanForm.GioHangThanhToan = dtGioHang;

            // Hiển thị thanhtoanForm
            thanhToanForm.Show();
            this.Hide();
        }

        private void muahangForm_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (cam != null && cam.IsRunning) cam.Stop();
            cam = new VideoCaptureDevice(cameras[comboBox1.SelectedIndex].MonikerString);
            cam.NewFrame += Cam_NewFrame;
            cam.Start();
            pictureBox1.Visible = true;
        }
        private void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap b = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = b;
        }
        private void qrcodeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cam != null && cam.IsRunning) cam.Stop();
        }
        void Giaima()
        {
            Bitmap imgQRCode = (Bitmap)pictureBox1.Image;
            if (imgQRCode != null)
            {
                try
                {
                    ZXing.BarcodeReader Reader = new ZXing.BarcodeReader();
                    Result result = Reader.Decode(imgQRCode);
                    if (result != null)
                    {
                        string decoded = result.ToString().Trim();
                        label3.Text = decoded;
                        qrstring = decoded;
                        imgQRCode.Dispose();
                        ExtractQRCodeInformation(qrstring);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "");
                }
            }
        }
        private void ExtractQRCodeInformation(string qrstring)
        {
            // Kết nối cơ sở dữ liệu và thực hiện truy vấn để lấy thông tin từ qrstring
           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ nếu có
                    MessageBox.Show("Đã xảy ra lỗi khi truy vấn cơ sở dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
