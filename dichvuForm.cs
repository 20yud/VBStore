using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VBStore
{
    public partial class dichvuForm : Form
    {
        private string connectionString = "Data Source=DESKTOP-DF0UTEB\\MIND;Initial Catalog=QLVB;Integrated Security=True";

        public dichvuForm()
        {
            InitializeComponent();
        }

        private void dichvuForm_Load(object sender, EventArgs e)
        {
            LoadServices(); // Load service data when the form is loaded
        }

        private void LoadServices()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Modify the query based on your database structure and requirements
                    string query = "SELECT MADICHVU AS 'Mã dịch vụ', " +
                                   "TENDICHVU AS 'Tên dịch vụ', " +
                                   "GIA AS 'Giá' " +
                                   "FROM DICHVU";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            guna2DataGridView1.DataSource = dataTable;

                            // Set column headers
                            if (guna2DataGridView1.Columns.Count >= 3)
                            {
                                guna2DataGridView1.Columns[0].HeaderText = "Mã dịch vụ";
                                guna2DataGridView1.Columns[1].HeaderText = "Tên dịch vụ";
                                guna2DataGridView1.Columns[2].HeaderText = "Giá";
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

        private void createBtn_Click(object sender, EventArgs e)
        {
            ThemDichVuForm themDichVuForm = new ThemDichVuForm();
            themDichVuForm.ShowDialog();
            LoadServices(); // Reload data after adding a new service
        }

        private void detailBtn_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                string maDichVu = guna2DataGridView1.SelectedRows[0].Cells["Mã dịch vụ"].Value.ToString();
                ChiTietDichVuForm chiTietDichVuForm = new ChiTietDichVuForm(maDichVu);
                chiTietDichVuForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dịch vụ để xem chi tiết.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                string maDichVu = guna2DataGridView1.SelectedRows[0].Cells["Mã dịch vụ"].Value.ToString();
                SuaDichVuForm suaDichVuForm = new SuaDichVuForm(maDichVu);
                suaDichVuForm.ShowDialog();
                LoadServices(); // Reload data after editing a service
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dịch vụ để sửa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];
                string maDichVu = selectedRow.Cells["Mã dịch vụ"].Value.ToString();

                // Pass the service code to the XoaDichVuForm when opening this form
                XoaDichVuForm xoaDichVuForm = new XoaDichVuForm(maDichVu);
                xoaDichVuForm.ShowDialog();
                LoadServices(); // Reload data after deleting a service
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dịch vụ để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Other event handlers can be added as needed

        // ...
    }
}
