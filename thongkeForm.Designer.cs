namespace VBStore
{
    partial class thongkeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.ChartBDT = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.ChartBDT)).BeginInit();
            this.SuspendLayout();
            // 
            // ChartBDT
            // 
            chartArea1.Name = "ChartArea1";
            this.ChartBDT.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.ChartBDT.Legends.Add(legend1);
            this.ChartBDT.Location = new System.Drawing.Point(12, 28);
            this.ChartBDT.Name = "ChartBDT";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Legend = "Legend1";
            series1.Name = "ChartBDT";
            this.ChartBDT.Series.Add(series1);
            this.ChartBDT.Size = new System.Drawing.Size(551, 651);
            this.ChartBDT.TabIndex = 1;
            this.ChartBDT.Text = "chart2";
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title1.Name = "Title1";
            title1.ShadowColor = System.Drawing.Color.Red;
            title1.Text = "Thống Kê Sản Phẩm";
            this.ChartBDT.Titles.Add(title1);
            // 
            // thongkeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 818);
            this.Controls.Add(this.ChartBDT);
            this.Name = "thongkeForm";
            this.Text = "thongkeForm";
            this.Load += new System.EventHandler(this.thongkeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ChartBDT)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart ChartBDT;
    }
}