namespace WindowsFormsAppDIPOpenCVSharp
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.파일ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.열기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.저장ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.종료ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.영상처리ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.반전ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PB_InputImage = new System.Windows.Forms.PictureBox();
            this.PB_OutputImage = new System.Windows.Forms.PictureBox();
            this.보기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.속성정보ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.대칭ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.좌우ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.상하ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.원점ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.히스토그램ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.히스토그램평활화ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.감마보정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_InputImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_OutputImage)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.파일ToolStripMenuItem,
            this.영상처리ToolStripMenuItem,
            this.보기ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 파일ToolStripMenuItem
            // 
            this.파일ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.열기ToolStripMenuItem,
            this.저장ToolStripMenuItem,
            this.종료ToolStripMenuItem});
            this.파일ToolStripMenuItem.Name = "파일ToolStripMenuItem";
            this.파일ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.파일ToolStripMenuItem.Text = "파일";
            // 
            // 열기ToolStripMenuItem
            // 
            this.열기ToolStripMenuItem.Name = "열기ToolStripMenuItem";
            this.열기ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.열기ToolStripMenuItem.Text = "열기";
            this.열기ToolStripMenuItem.Click += new System.EventHandler(this.열기ToolStripMenuItem_Click);
            // 
            // 저장ToolStripMenuItem
            // 
            this.저장ToolStripMenuItem.Name = "저장ToolStripMenuItem";
            this.저장ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.저장ToolStripMenuItem.Text = "저장";
            this.저장ToolStripMenuItem.Click += new System.EventHandler(this.저장ToolStripMenuItem_Click);
            // 
            // 종료ToolStripMenuItem
            // 
            this.종료ToolStripMenuItem.Name = "종료ToolStripMenuItem";
            this.종료ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.종료ToolStripMenuItem.Text = "종료";
            this.종료ToolStripMenuItem.Click += new System.EventHandler(this.종료ToolStripMenuItem_Click);
            // 
            // 영상처리ToolStripMenuItem
            // 
            this.영상처리ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.반전ToolStripMenuItem,
            this.대칭ToolStripMenuItem,
            this.히스토그램ToolStripMenuItem,
            this.히스토그램평활화ToolStripMenuItem,
            this.감마보정ToolStripMenuItem});
            this.영상처리ToolStripMenuItem.Name = "영상처리ToolStripMenuItem";
            this.영상처리ToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.영상처리ToolStripMenuItem.Text = "영상처리";
            // 
            // 반전ToolStripMenuItem
            // 
            this.반전ToolStripMenuItem.Name = "반전ToolStripMenuItem";
            this.반전ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.반전ToolStripMenuItem.Text = "반전";
            this.반전ToolStripMenuItem.Click += new System.EventHandler(this.반전ToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.PB_InputImage, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.PB_OutputImage, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 36);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(776, 385);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // PB_InputImage
            // 
            this.PB_InputImage.Location = new System.Drawing.Point(3, 3);
            this.PB_InputImage.Name = "PB_InputImage";
            this.PB_InputImage.Size = new System.Drawing.Size(380, 379);
            this.PB_InputImage.TabIndex = 0;
            this.PB_InputImage.TabStop = false;
            // 
            // PB_OutputImage
            // 
            this.PB_OutputImage.Location = new System.Drawing.Point(391, 3);
            this.PB_OutputImage.Name = "PB_OutputImage";
            this.PB_OutputImage.Size = new System.Drawing.Size(382, 379);
            this.PB_OutputImage.TabIndex = 1;
            this.PB_OutputImage.TabStop = false;
            // 
            // 보기ToolStripMenuItem
            // 
            this.보기ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.속성정보ToolStripMenuItem});
            this.보기ToolStripMenuItem.Name = "보기ToolStripMenuItem";
            this.보기ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.보기ToolStripMenuItem.Text = "보기";
            // 
            // 속성정보ToolStripMenuItem
            // 
            this.속성정보ToolStripMenuItem.Name = "속성정보ToolStripMenuItem";
            this.속성정보ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.속성정보ToolStripMenuItem.Text = "속성정보";
            // 
            // 대칭ToolStripMenuItem
            // 
            this.대칭ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.상하ToolStripMenuItem,
            this.좌우ToolStripMenuItem,
            this.원점ToolStripMenuItem});
            this.대칭ToolStripMenuItem.Name = "대칭ToolStripMenuItem";
            this.대칭ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.대칭ToolStripMenuItem.Text = "대칭";
            // 
            // 좌우ToolStripMenuItem
            // 
            this.좌우ToolStripMenuItem.Name = "좌우ToolStripMenuItem";
            this.좌우ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.좌우ToolStripMenuItem.Text = "좌우";
            // 
            // 상하ToolStripMenuItem
            // 
            this.상하ToolStripMenuItem.Name = "상하ToolStripMenuItem";
            this.상하ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.상하ToolStripMenuItem.Text = "상하";
            this.상하ToolStripMenuItem.Click += new System.EventHandler(this.상하ToolStripMenuItem_Click);
            // 
            // 원점ToolStripMenuItem
            // 
            this.원점ToolStripMenuItem.Name = "원점ToolStripMenuItem";
            this.원점ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.원점ToolStripMenuItem.Text = "상하좌우";
            // 
            // 히스토그램ToolStripMenuItem
            // 
            this.히스토그램ToolStripMenuItem.Name = "히스토그램ToolStripMenuItem";
            this.히스토그램ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.히스토그램ToolStripMenuItem.Text = "히스토그램";
            this.히스토그램ToolStripMenuItem.Click += new System.EventHandler(this.히스토그램ToolStripMenuItem_Click);
            // 
            // 히스토그램평활화ToolStripMenuItem
            // 
            this.히스토그램평활화ToolStripMenuItem.Name = "히스토그램평활화ToolStripMenuItem";
            this.히스토그램평활화ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.히스토그램평활화ToolStripMenuItem.Text = "히스토그램평활화";
            this.히스토그램평활화ToolStripMenuItem.Click += new System.EventHandler(this.히스토그램평활화ToolStripMenuItem_Click);
            // 
            // 감마보정ToolStripMenuItem
            // 
            this.감마보정ToolStripMenuItem.Name = "감마보정ToolStripMenuItem";
            this.감마보정ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.감마보정ToolStripMenuItem.Text = "감마보정";
            this.감마보정ToolStripMenuItem.Click += new System.EventHandler(this.감마보정ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "DIPwithOpenCVSharp";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_InputImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_OutputImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 파일ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 열기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 저장ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 종료ToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox PB_InputImage;
        private System.Windows.Forms.PictureBox PB_OutputImage;
        private System.Windows.Forms.ToolStripMenuItem 영상처리ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 반전ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 보기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 속성정보ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 대칭ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 좌우ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 상하ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 원점ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 히스토그램ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 히스토그램평활화ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 감마보정ToolStripMenuItem;
    }
}