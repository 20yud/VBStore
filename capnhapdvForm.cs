using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace VBStore
{
    public partial class capnhapdvForm : Form
    {
        private string sopHieuDichVu;
        private string connectionString;
        dbhelper dbHelper = new dbhelper();
        public capnhapdvForm(string maDV)
        {
            InitializeComponent();
            connectionString = dbHelper.ConnectionString;
            this.sopHieuDichVu = maDV;

            LoadCTDV();
        }

        private void LoadCTDV()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Câu truy vấn SQL để lấy thông tin cụ thể dựa trên SOPHIEUDICHVU
                    string query = "SELECT " +
                                   "PHIEUDICHVU.SOPHIEUDICHVU, PHIEUDICHVU.NGAYLAP, PHIEUDICHVU.TINHTRANG, " +
                                   "CT_PHIEUDICHVU.MALOAIDICHVU, CT_PHIEUDICHVU.DONGIADICHVU, " +
                                   "CT_PHIEUDICHVU.DONGIADUOCTINH, CT_PHIEUDICHVU.SOLUONG, " +
                                   "CT_PHIEUDICHVU.THANHTIEN, CT_PHIEUDICHVU.TRATRUOC, " +
                                   "CT_PHIEUDICHVU.CONLAI, CT_PHIEUDICHVU.NGAYGIAO, CT_PHIEUDICHVU.TINHTRANG " +
                                   "FROM PHIEUDICHVU " +
                                   "INNER JOIN CT_PHIEUDICHVU ON PHIEUDICHVU.SOPHIEUDICHVU = CT_PHIEUDICHVU.SOPHIEUDICHVU " +
                                   "WHERE PHIEUDICHVU.SOPHIEUDICHVU = @SOPHIEUDICHVU";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SOPHIEUDICHVU", sopHieuDichVu);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Check if any rows are returned
                            if (dataTable.Rows.Count > 0)
                            {
                                // Assuming your TextBox controls are named txtSOPHIEUDICHVU, txtNGAYLAP, txtTINHTRANG, etc.
                                txtSOPHIEUDICHVU.Text = dataTable.Rows[0]["SOPHIEUDICHVU"].ToString();
                                txtNGAYLAP.Text = dataTable.Rows[0]["NGAYLAP"].ToString();
                                txtTINHTRANG.Text = dataTable.Rows[0]["TINHTRANG"].ToString();
                                txtMALOAIDICHVU.Text = dataTable.Rows[0]["MALOAIDICHVU"].ToString();
                                txtDONGIADICHVU.Text = dataTable.Rows[0]["DONGIADICHVU"].ToString();
                                txtDONGIADUOCTINH.Text = dataTable.Rows[0]["DONGIADUOCTINH"].ToString();
                                txtSOLUONG.Text = dataTable.Rows[0]["SOLUONG"].ToString();
                                txtTHANHTIEN.Text = dataTable.Rows[0]["THANHTIEN"].ToString();
                                txtTRATRUOC.Text = dataTable.Rows[0]["TRATRUOC"].ToString();
                                txtCONLAI.Text = dataTable.Rows[0]["CONLAI"].ToString();
                                txtNGAYGIAO.Text = dataTable.Rows[0]["NGAYGIAO"].ToString();
                               
                                // Set other TextBox controls with corresponding column values
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy dữ liệu cho SOPHIEUDICHVU này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void createBtn_Click(object sender, EventArgs e)
        {
            
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
