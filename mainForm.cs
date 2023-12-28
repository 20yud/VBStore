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
    public partial class mainForm : Form
    {
        private string sdt;
        private Form currentFormChild;
        private string connectionString;
        dbhelper dbHelper = new dbhelper();
        private string imagesDirectory = @"C:\Workspace\c#\cnpm\VBStore\images\poster";
        private List<string> imagePaths = new List<string>();
        private int currentImageIndex = 0;
        public mainForm()
        {
            InitializeComponent();
            connectionString = dbHelper.ConnectionString;
        }

        private void OpenChildFrom(Form childForm)
        {
            if (currentFormChild != null) 
            {
                currentFormChild.Close();
            }
            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.Dock = DockStyle.Fill; // Đảm bảo childForm fill hết panel6
            mainPanel.Controls.Add(childForm);
            childForm.BringToFront();
            childForm.WindowState = FormWindowState.Maximized; // Mở childForm ở kích thước lớn nhất
            childForm.Show();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       

        private void findcusBtn_Click(object sender, EventArgs e)
        {
            string phoneNumber = numberTextBox.Text;
            sdt = phoneNumber;

            // Kết nối đến cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Tạo câu truy vấn SQL
                string query = "SELECT COUNT(*) FROM KHACHHANG WHERE SDT = @PhoneNumber";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                    // Thực thi câu truy vấn và trả về số lượng khách hàng có số điện thoại tương ứng
                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        findcusForm cusForm = new findcusForm(sdt);
                        OpenChildFrom(cusForm);
                        backBtn.Visible = true;
                        titlelabel.Text = "Thông tin khách hàng";

                    }
                    else
                    {
                        MessageBox.Show("Số điện thoại không tồn tại trong bảng Khách hàng.");
                    }
                }
            }
        }

        

        private void mainForm_Load(object sender, EventArgs e)
        {
            if (currentFormChild != null)
            {
                backBtn.Visible = true;
            }
            timer1.Interval = 10000;
            coutnKH();
            countDQTS();
            countDVF();

            
        }
        void coutnKH()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Tạo câu truy vấn SQL để đếm số lượng khách hàng
                string query = "SELECT COUNT(*) FROM KHACHHANG";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thực thi câu truy vấn và trả về số lượng khách hàng
                    int count = (int)command.ExecuteScalar();
                    countKH.Text = count.ToString(); // Gán kết quả vào Label countKH
                }
            }
        }
        void countDQTS()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Create SQL query to count the number of products for each product type
                string query = "SELECT MALOAISANPHAM, COUNT(*) FROM SANPHAM GROUP BY MALOAISANPHAM";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    int totalCountDQ = 0;
                    int totalCountTS = 0;

                    while (reader.Read())
                    {
                        string maLoaiSanPham = reader.GetString(0);
                        int count = reader.GetInt32(1);

                        // Update total counts based on product type
                        if (maLoaiSanPham == "LSP001" || maLoaiSanPham == "LSP002" || maLoaiSanPham == "LSP003" || maLoaiSanPham == "LSP004")
                        {
                            totalCountDQ += count;
                        }
                        else if (maLoaiSanPham == "LSP005")
                        {
                            totalCountTS += count;
                        }
                    }

                    // Assign the total counts to the respective labels
                    countDQ.Text = totalCountDQ.ToString();
                    countTS.Text = totalCountTS.ToString();
                }
            }
        }
        void countDVF()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Tạo câu truy vấn SQL để đếm số lượng dịch vụ
                string query = "SELECT COUNT(*) FROM DICHVU";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thực thi câu truy vấn và trả về số lượng dịch vụ
                    int count = (int)command.ExecuteScalar();
                    countDV.Text = count.ToString(); // Gán kết quả vào Label countDV
                }
            }
        }
        private void panelgem_Click(object sender, EventArgs e)
        {
            daquyForm gemForm = new daquyForm();
            OpenChildFrom(gemForm);
            backBtn.Visible = true;
            titlelabel.Text = "Đá Quý";
        }

        private void paneljewelry_Click(object sender, EventArgs e)
        {
            trangsucForm trangsuc = new trangsucForm();
            OpenChildFrom(trangsuc);
            backBtn.Visible = true;
            titlelabel.Text = "Trang Sức";
        }

        private void mainForm_ClientSizeChanged(object sender, EventArgs e)
        {
            if (currentFormChild != null)
            {
                currentFormChild.WindowState = this.WindowState; // Đặt trạng thái cửa sổ của childForm giống với mainForm
                if (this.WindowState == FormWindowState.Maximized || this.WindowState == FormWindowState.Normal)
                {
                    if(numberTextBox.Text != null)
                    {
                        findcusBtn_Click(findcusBtn, EventArgs.Empty);
                    }
                }
            }
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
            }
            backBtn.Visible = false;
            titlelabel.Text = "VBStore";
            coutnKH();
            countDQTS();
            countDVF();
        }

        private void panelcustomer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelcustomer_Click_1(object sender, EventArgs e)
        {
            customerForm customerForm = new customerForm(sdt);
            OpenChildFrom(customerForm);
            backBtn.Visible = true;
            titlelabel.Text = "Danh sách khách hàng";
        }

        private void panelservice_Click(object sender, EventArgs e)
        {
            dichvuForm dichVu = new dichvuForm();
            OpenChildFrom(dichVu);
            backBtn.Visible = true;
            titlelabel.Text = "Dịch Vụ";
        }
    }
}
