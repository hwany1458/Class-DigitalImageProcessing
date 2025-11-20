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
            this.대칭ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.상하ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.좌우ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.원점ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.히스토그램ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.히스토그램평활화ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.감마보정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.블러링ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.공간필터링ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.샤프링ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.보기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.속성정보ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PB_InputImage = new System.Windows.Forms.PictureBox();
            this.PB_OutputImage = new System.Windows.Forms.PictureBox();
            this.블러링ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.샤프ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.공간필터링ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.블러링효과ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.필터링효과ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.샤프닝ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.샤프닝효과ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.샤프닝효과2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.캐니엣지추출ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.엣지추출효과ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.열기ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.열기ToolStripMenuItem.Text = "열기";
            this.열기ToolStripMenuItem.Click += new System.EventHandler(this.열기ToolStripMenuItem_Click);
            // 
            // 저장ToolStripMenuItem
            // 
            this.저장ToolStripMenuItem.Name = "저장ToolStripMenuItem";
            this.저장ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.저장ToolStripMenuItem.Text = "저장";
            this.저장ToolStripMenuItem.Click += new System.EventHandler(this.저장ToolStripMenuItem_Click);
            // 
            // 종료ToolStripMenuItem
            // 
            this.종료ToolStripMenuItem.Name = "종료ToolStripMenuItem";
            this.종료ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.종료ToolStripMenuItem.Text = "종료";
            this.종료ToolStripMenuItem.Click += new System.EventHandler(this.종료ToolStripMenuItem_Click);
            // 
            // 영상처리ToolStripMenuItem
            // 
            this.영상처리ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.반전ToolStripMenuItem,
            this.대칭ToolStripMenuItem,
            this.toolStripSeparator2,
            this.히스토그램ToolStripMenuItem,
            this.히스토그램평활화ToolStripMenuItem,
            this.감마보정ToolStripMenuItem,
            this.toolStripSeparator1,
            this.블러링ToolStripMenuItem,
            this.샤프ToolStripMenuItem,
            this.샤프링ToolStripMenuItem});
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
            // 상하ToolStripMenuItem
            // 
            this.상하ToolStripMenuItem.Name = "상하ToolStripMenuItem";
            this.상하ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.상하ToolStripMenuItem.Text = "상하";
            this.상하ToolStripMenuItem.Click += new System.EventHandler(this.상하ToolStripMenuItem_Click);
            // 
            // 좌우ToolStripMenuItem
            // 
            this.좌우ToolStripMenuItem.Name = "좌우ToolStripMenuItem";
            this.좌우ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.좌우ToolStripMenuItem.Text = "좌우";
            // 
            // 원점ToolStripMenuItem
            // 
            this.원점ToolStripMenuItem.Name = "원점ToolStripMenuItem";
            this.원점ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.원점ToolStripMenuItem.Text = "상하좌우";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // 블러링ToolStripMenuItem
            // 
            this.블러링ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.공간필터링ToolStripMenuItem,
            this.블러링ToolStripMenuItem1,
            this.블러링효과ToolStripMenuItem});
            this.블러링ToolStripMenuItem.Name = "블러링ToolStripMenuItem";
            this.블러링ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.블러링ToolStripMenuItem.Text = "블러링";
            // 
            // 공간필터링ToolStripMenuItem
            // 
            this.공간필터링ToolStripMenuItem.Name = "공간필터링ToolStripMenuItem";
            this.공간필터링ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.공간필터링ToolStripMenuItem.Text = "공간필터링";
            this.공간필터링ToolStripMenuItem.Click += new System.EventHandler(this.공간필터링ToolStripMenuItem_Click);
            // 
            // 샤프링ToolStripMenuItem
            // 
            this.샤프링ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.캐니엣지추출ToolStripMenuItem,
            this.엣지추출효과ToolStripMenuItem});
            this.샤프링ToolStripMenuItem.Name = "샤프링ToolStripMenuItem";
            this.샤프링ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.샤프링ToolStripMenuItem.Text = "엣지추출";
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
            this.속성정보ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.속성정보ToolStripMenuItem.Text = "속성정보";
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
            // 블러링ToolStripMenuItem1
            // 
            this.블러링ToolStripMenuItem1.Name = "블러링ToolStripMenuItem1";
            this.블러링ToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.블러링ToolStripMenuItem1.Text = "블러링";
            this.블러링ToolStripMenuItem1.Click += new System.EventHandler(this.블러링ToolStripMenuItem1_Click);
            // 
            // 샤프ToolStripMenuItem
            // 
            this.샤프ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.공간필터링ToolStripMenuItem1,
            this.필터링효과ToolStripMenuItem,
            this.샤프닝ToolStripMenuItem,
            this.샤프닝효과ToolStripMenuItem,
            this.샤프닝효과2ToolStripMenuItem});
            this.샤프ToolStripMenuItem.Name = "샤프ToolStripMenuItem";
            this.샤프ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.샤프ToolStripMenuItem.Text = "샤프닝";
            // 
            // 공간필터링ToolStripMenuItem1
            // 
            this.공간필터링ToolStripMenuItem1.Name = "공간필터링ToolStripMenuItem1";
            this.공간필터링ToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.공간필터링ToolStripMenuItem1.Text = "공간필터링";
            this.공간필터링ToolStripMenuItem1.Click += new System.EventHandler(this.공간필터링ToolStripMenuItem1_Click);
            // 
            // 블러링효과ToolStripMenuItem
            // 
            this.블러링효과ToolStripMenuItem.Name = "블러링효과ToolStripMenuItem";
            this.블러링효과ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.블러링효과ToolStripMenuItem.Text = "블러링효과";
            this.블러링효과ToolStripMenuItem.Click += new System.EventHandler(this.블러링효과ToolStripMenuItem_Click);
            // 
            // 필터링효과ToolStripMenuItem
            // 
            this.필터링효과ToolStripMenuItem.Name = "필터링효과ToolStripMenuItem";
            this.필터링효과ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.필터링효과ToolStripMenuItem.Text = "필터링효과";
            this.필터링효과ToolStripMenuItem.Click += new System.EventHandler(this.필터링효과ToolStripMenuItem_Click);
            // 
            // 샤프닝ToolStripMenuItem
            // 
            this.샤프닝ToolStripMenuItem.Name = "샤프닝ToolStripMenuItem";
            this.샤프닝ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.샤프닝ToolStripMenuItem.Text = "샤프닝";
            this.샤프닝ToolStripMenuItem.Click += new System.EventHandler(this.샤프닝ToolStripMenuItem_Click);
            // 
            // 샤프닝효과ToolStripMenuItem
            // 
            this.샤프닝효과ToolStripMenuItem.Name = "샤프닝효과ToolStripMenuItem";
            this.샤프닝효과ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.샤프닝효과ToolStripMenuItem.Text = "샤프닝효과1";
            this.샤프닝효과ToolStripMenuItem.Click += new System.EventHandler(this.샤프닝효과ToolStripMenuItem_Click);
            // 
            // 샤프닝효과2ToolStripMenuItem
            // 
            this.샤프닝효과2ToolStripMenuItem.Name = "샤프닝효과2ToolStripMenuItem";
            this.샤프닝효과2ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.샤프닝효과2ToolStripMenuItem.Text = "샤프닝효과2";
            this.샤프닝효과2ToolStripMenuItem.Click += new System.EventHandler(this.샤프닝효과2ToolStripMenuItem_Click);
            // 
            // 캐니엣지추출ToolStripMenuItem
            // 
            this.캐니엣지추출ToolStripMenuItem.Name = "캐니엣지추출ToolStripMenuItem";
            this.캐니엣지추출ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.캐니엣지추출ToolStripMenuItem.Text = "캐니엣지추출";
            this.캐니엣지추출ToolStripMenuItem.Click += new System.EventHandler(this.캐니엣지추출ToolStripMenuItem_Click);
            // 
            // 엣지추출효과ToolStripMenuItem
            // 
            this.엣지추출효과ToolStripMenuItem.Name = "엣지추출효과ToolStripMenuItem";
            this.엣지추출효과ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.엣지추출효과ToolStripMenuItem.Text = "엣지추출효과";
            this.엣지추출효과ToolStripMenuItem.Click += new System.EventHandler(this.엣지추출효과ToolStripMenuItem_Click);
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 블러링ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 공간필터링ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 샤프링ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 블러링ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 샤프ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 공간필터링ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 블러링효과ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 필터링효과ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 샤프닝ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 샤프닝효과ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 샤프닝효과2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 캐니엣지추출ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 엣지추출효과ToolStripMenuItem;
    }
}