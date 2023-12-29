using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace VBStore
{
    public partial class thongkeForm : Form
    {
        dbhelper dbHelper = new dbhelper();
        private string connectionString;

        public thongkeForm()
        {
            InitializeComponent();
            connectionString = dbHelper.ConnectionString;
        }

        private void thongkeForm_Load(object sender, EventArgs e)
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
    }
}
