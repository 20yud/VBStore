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
        private string connectionString = "Data Source=DESKTOP-C753LHT\\SQLEXPRESS;Initial Catalog=vbstore;Integrated Security=True";
        private string imagesDirectory = @"C:\Workspace\c#\cnpm\VBStore\images\poster";
        private List<string> imagePaths = new List<string>();
        private int currentImageIndex = 0;
        public mainForm()
        {
            InitializeComponent();
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
            mainPanel.Controls.Add(childForm);
            mainPanel.Tag = childForm;
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
                        themCustomerForm cusForm = new themCustomerForm(sdt);
                        OpenChildFrom(cusForm);
                        
                    }
                    else
                    {
                        MessageBox.Show("Số điện thoại không tồn tại trong bảng Khách hàng.");
                    }
                }
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            // Switch to the next image
            currentImageIndex++;
            if (currentImageIndex >= imagePaths.Count)
            {
                // Restart from the first image if all images have been shown
                currentImageIndex = 0;
            }

            // Display the next image
            pictureBox1.Image = Image.FromFile(imagePaths[currentImageIndex]);
        }

        private void LoadImages()
        {
            // Check if the directory exists
            if (Directory.Exists(imagesDirectory))
            {
                // Get all image files from the specified directory
                string[] imageFiles = Directory.GetFiles(imagesDirectory, "*.png");

                // Populate the imagePaths list with the paths of image files
                imagePaths.AddRange(imageFiles);

                // Display the first image
                if (imagePaths.Count > 0)
                {
                    pictureBox1.Image = Image.FromFile(imagePaths[0]);
                    pictureBox1.SizeMode=PictureBoxSizeMode.StretchImage;
                }

                // Start the timer to switch images every 30 seconds
                timer1.Start();
            }
            else
            {
                MessageBox.Show("Image directory not found.", "Error");
            }
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            // Set up the timer
            timer1.Interval = 10000; // 30 seconds
            timer1.Tick += Timer1_Tick;

            // Load initial images into pictureBox1
            LoadImages();
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

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            customerForm khachhang = new customerForm();
            OpenChildFrom(khachhang);
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


    }
}
