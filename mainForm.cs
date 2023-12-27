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
            childForm.Dock = DockStyle.Fill;
            panel6.Controls.Add(childForm);
            panel6.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void inoutBtn_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
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
                        customerForm cusForm = new customerForm(sdt);
                        OpenChildFrom(cusForm);

                        
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
            // Set up the timer
            timer1.Interval = 10000; // 30 seconds
            
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            daquyForm daquy = new daquyForm();
            OpenChildFrom(daquy);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            trangsucForm trangsuc = new trangsucForm();
            OpenChildFrom(trangsuc);
        }

        private void panelcustomer_Click(object sender, EventArgs e)
        {

        }

        private void panelgem_Click(object sender, EventArgs e)
        {
            daquyForm daquy = new daquyForm();
            OpenChildFrom(daquy);
        }

        private void paneljewelry_Click(object sender, EventArgs e)
        {
            trangsucForm trangsuc = new trangsucForm();
            OpenChildFrom(trangsuc);
        }

        private void panelservice_Click(object sender, EventArgs e)
        {

        }
    }
}
