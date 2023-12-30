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
using System.Windows.Forms.DataVisualization.Charting;

namespace VBStore
{
    public partial class mainForm : Form
    {
        private string sdt;
        private string tenKH;
        private Form currentFormChild;
        private string connectionString;
        dbhelper dbHelper = new dbhelper();
        private string imagesDirectory = @"C:\Workspace\c#\cnpm\VBStore\images\poster";
        private List<string> imagePaths = new List<string>();
        private int currentImageIndex = 0;
        private ChildFormUtility childFormUtility;
        public mainForm()
        {
            InitializeComponent();
            connectionString = dbHelper.ConnectionString;
            label21.Text = "Dịch Vụ \n Đã Đặt";
            childFormUtility = new ChildFormUtility(this);
           
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
            pie_load();
            countDVBb();
            countDMH();
            countDBH();
            countHT();



        }
            void pie_load()
            {
                DataTable dataTable = new DataTable();

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = @"
                            SELECT
                                CASE WHEN MALOAISANPHAM IN ('LSP001', 'LSP002', 'LSP003', 'LSP004') THEN N'Đá Quý'
                                     WHEN MALOAISANPHAM = 'LSP005' THEN N'Trang Sức'
                                END AS ProductType,
                                SUM(SOLUONGTON) AS TOTAL_SOLUONGTON
                            FROM
                                SANPHAM
                            WHERE
                                MALOAISANPHAM IN ('LSP001', 'LSP002', 'LSP003', 'LSP004', 'LSP005')
                            GROUP BY
                                CASE WHEN MALOAISANPHAM IN ('LSP001', 'LSP002', 'LSP003', 'LSP004') THEN N'Đá Quý'
                                     WHEN MALOAISANPHAM = 'LSP005' THEN N'Trang Sức'
                                END";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                            {
                                adapter.Fill(dataTable);
                            }
                        }
                    }

                    Series pieSeries = new Series("DoanhThuTheoThang_Tron");
                    pieSeries.ChartType = SeriesChartType.Pie;

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string productType = row["ProductType"].ToString();
                        double totalQuantity = Convert.ToDouble(row["TOTAL_SOLUONGTON"]);

                        pieSeries.Points.AddXY(productType, totalQuantity);
                    }

                    ChartBDT.Series.Add(pieSeries);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

                
                string query = "SELECT COUNT(*) FROM DICHVU";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                   
                    int count = (int)command.ExecuteScalar();
                    countDV.Text = count.ToString(); 
                }
            }
        }
        void countDVBb()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                string query = "SELECT COUNT(*) FROM PHIEUDICHVU";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    int count = (int)command.ExecuteScalar();
                    countBDV.Text = count.ToString();
                }
            }
        }
        void countDMH()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                string query = "SELECT COUNT(*) FROM PHIEUMUAHANG";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    int count = (int)command.ExecuteScalar();
                    cDMH.Text = count.ToString();
                }
            }
        }
        void countDBH()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                string query = "SELECT COUNT(*) FROM PHIEUBANHANG";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    int count = (int)command.ExecuteScalar();
                    cDBHH.Text = count.ToString();
                }
            }
        }
        void countHT()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                string query = "SELECT COUNT(*) FROM BAOCAOTON";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    int count = (int)command.ExecuteScalar();
                    cBCT.Text = count.ToString();
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
                currentFormChild.WindowState = this.WindowState; // Set childForm's window state to match mainForm

                // Check if the currentFormChild is an instance of findcusForm
                if (currentFormChild is findcusForm)
                {
                    findcusBtn_Click(findcusBtn, EventArgs.Empty); // Call the method only when findcusForm is displayed
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

        private void guna2CustomGradientPanel7_Click(object sender, EventArgs e)
        {
            dichvubookedForm dichvubookedForm = new dichvubookedForm();
            OpenChildFrom(dichvubookedForm); 
            backBtn.Visible = true;
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomGradientPanel6_Click(object sender, EventArgs e)
        {
            dmhForm dmhform = new dmhForm();
            OpenChildFrom(dmhform);
            backBtn.Visible = true;
        }

        private void guna2CustomGradientPanel5_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void guna2CustomGradientPanel5_Click(object sender, EventArgs e)
        {
            dbhForm dbhform = new dbhForm();
            OpenChildFrom(dbhform);
            backBtn.Visible = true;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void muahangBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(numberTextBox.Text)) // Check if the numberTextBox is not empty
            {
                sdt = numberTextBox.Text; // Assign the value from numberTextBox to sdt
                gettenkh();
                muahangForm muahang = new muahangForm(sdt, tenKH);
                OpenChildFrom(muahang);
            }
            else
            {
                MessageBox.Show("Vui lòng nhập số điện thoại");
            }
        }
        void gettenkh()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT TENKH FROM KHACHHANG WHERE SDT = @PhoneNumber";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Update the parameter with the current phone number (sdt)
                    command.Parameters.AddWithValue("@PhoneNumber", sdt);  // Use the updated sdt value

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tenKH = reader.GetString(reader.GetOrdinal("TENKH"));
                           
                        }

                    }
                }
            }
        }
    }
}
