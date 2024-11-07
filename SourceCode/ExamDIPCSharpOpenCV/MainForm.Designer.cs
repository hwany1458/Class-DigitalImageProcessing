
namespace ExamDIPCSharpOpenCV
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.파일ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.오픈ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.공간필터ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.필터ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.반전ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.그레이스케일ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.좌우대칭ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.장ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PB_InputImage = new System.Windows.Forms.PictureBox();
            this.PB_ResultImage = new System.Windows.Forms.PictureBox();
            this.퓨리에변환ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.가우시안블러닝ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.공간필터ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_InputImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_ResultImage)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.파일ToolStripMenuItem,
            this.공간필터ToolStripMenuItem,
            this.필터ToolStripMenuItem,
            this.장ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(819, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 파일ToolStripMenuItem
            // 
            this.파일ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.오픈ToolStripMenuItem});
            this.파일ToolStripMenuItem.Name = "파일ToolStripMenuItem";
            this.파일ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.파일ToolStripMenuItem.Text = "파일";
            // 
            // 오픈ToolStripMenuItem
            // 
            this.오픈ToolStripMenuItem.Name = "오픈ToolStripMenuItem";
            this.오픈ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.오픈ToolStripMenuItem.Text = "오픈";
            this.오픈ToolStripMenuItem.Click += new System.EventHandler(this.오픈ToolStripMenuItem_Click);
            // 
            // 공간필터ToolStripMenuItem
            // 
            this.공간필터ToolStripMenuItem.Name = "공간필터ToolStripMenuItem";
            this.공간필터ToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.공간필터ToolStripMenuItem.Text = "점처리";
            // 
            // 필터ToolStripMenuItem
            // 
            this.필터ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.반전ToolStripMenuItem,
            this.그레이스케일ToolStripMenuItem,
            this.좌우대칭ToolStripMenuItem,
            this.toolStripSeparator1,
            this.공간필터ToolStripMenuItem1,
            this.가우시안블러닝ToolStripMenuItem});
            this.필터ToolStripMenuItem.Name = "필터ToolStripMenuItem";
            this.필터ToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.필터ToolStripMenuItem.Text = "3장";
            // 
            // 반전ToolStripMenuItem
            // 
            this.반전ToolStripMenuItem.Name = "반전ToolStripMenuItem";
            this.반전ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.반전ToolStripMenuItem.Text = "반전";
            this.반전ToolStripMenuItem.Click += new System.EventHandler(this.반전ToolStripMenuItem_Click);
            // 
            // 그레이스케일ToolStripMenuItem
            // 
            this.그레이스케일ToolStripMenuItem.Name = "그레이스케일ToolStripMenuItem";
            this.그레이스케일ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.그레이스케일ToolStripMenuItem.Text = "그레이스케일";
            this.그레이스케일ToolStripMenuItem.Click += new System.EventHandler(this.그레이스케일ToolStripMenuItem_Click);
            // 
            // 좌우대칭ToolStripMenuItem
            // 
            this.좌우대칭ToolStripMenuItem.Name = "좌우대칭ToolStripMenuItem";
            this.좌우대칭ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.좌우대칭ToolStripMenuItem.Text = "좌우대칭";
            this.좌우대칭ToolStripMenuItem.Click += new System.EventHandler(this.좌우대칭ToolStripMenuItem_Click);
            // 
            // 장ToolStripMenuItem
            // 
            this.장ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.퓨리에변환ToolStripMenuItem});
            this.장ToolStripMenuItem.Name = "장ToolStripMenuItem";
            this.장ToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.장ToolStripMenuItem.Text = "4장";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.PB_InputImage, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.PB_ResultImage, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 27);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(810, 400);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // PB_InputImage
            // 
            this.PB_InputImage.Location = new System.Drawing.Point(3, 3);
            this.PB_InputImage.Name = "PB_InputImage";
            this.PB_InputImage.Size = new System.Drawing.Size(399, 394);
            this.PB_InputImage.TabIndex = 0;
            this.PB_InputImage.TabStop = false;
            // 
            // PB_ResultImage
            // 
            this.PB_ResultImage.Location = new System.Drawing.Point(408, 3);
            this.PB_ResultImage.Name = "PB_ResultImage";
            this.PB_ResultImage.Size = new System.Drawing.Size(399, 394);
            this.PB_ResultImage.TabIndex = 1;
            this.PB_ResultImage.TabStop = false;
            // 
            // 퓨리에변환ToolStripMenuItem
            // 
            this.퓨리에변환ToolStripMenuItem.Name = "퓨리에변환ToolStripMenuItem";
            this.퓨리에변환ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.퓨리에변환ToolStripMenuItem.Text = "퓨리에변환";
            this.퓨리에변환ToolStripMenuItem.Click += new System.EventHandler(this.퓨리에변환ToolStripMenuItem_Click);
            // 
            // 가우시안블러닝ToolStripMenuItem
            // 
            this.가우시안블러닝ToolStripMenuItem.Name = "가우시안블러닝ToolStripMenuItem";
            this.가우시안블러닝ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.가우시안블러닝ToolStripMenuItem.Text = "가우시안블러닝";
            this.가우시안블러닝ToolStripMenuItem.Click += new System.EventHandler(this.가우시안블러닝ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // 공간필터ToolStripMenuItem1
            // 
            this.공간필터ToolStripMenuItem1.Name = "공간필터ToolStripMenuItem1";
            this.공간필터ToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.공간필터ToolStripMenuItem1.Text = "공간필터";
            this.공간필터ToolStripMenuItem1.Click += new System.EventHandler(this.공간필터ToolStripMenuItem1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(819, 447);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MainWindowForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_InputImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_ResultImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox PB_InputImage;
        private System.Windows.Forms.PictureBox PB_ResultImage;
        private System.Windows.Forms.ToolStripMenuItem 파일ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 오픈ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 공간필터ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 필터ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 반전ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 그레이스케일ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 좌우대칭ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 장ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 퓨리에변환ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 가우시안블러닝ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 공간필터ToolStripMenuItem1;
    }
}
