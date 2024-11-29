
namespace ExamDIPCSharpOpenCV
{
    partial class FormFileInfo
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelImageName = new System.Windows.Forms.Label();
            this.labelImageExtension = new System.Windows.Forms.Label();
            this.labelImageLocation = new System.Windows.Forms.Label();
            this.labelImageDimension = new System.Windows.Forms.Label();
            this.labelImageSize = new System.Windows.Forms.Label();
            this.labelImageCreatedOn = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Image Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Image Extension:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Image Location:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "Image Dimension:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "Image Size:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 170);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "Image Created On:";
            // 
            // labelImageName
            // 
            this.labelImageName.AutoSize = true;
            this.labelImageName.Location = new System.Drawing.Point(135, 20);
            this.labelImageName.Name = "labelImageName";
            this.labelImageName.Size = new System.Drawing.Size(38, 12);
            this.labelImageName.TabIndex = 6;
            this.labelImageName.Text = "label7";
            // 
            // labelImageExtension
            // 
            this.labelImageExtension.AutoSize = true;
            this.labelImageExtension.Location = new System.Drawing.Point(135, 50);
            this.labelImageExtension.Name = "labelImageExtension";
            this.labelImageExtension.Size = new System.Drawing.Size(38, 12);
            this.labelImageExtension.TabIndex = 7;
            this.labelImageExtension.Text = "label8";
            // 
            // labelImageLocation
            // 
            this.labelImageLocation.AutoSize = true;
            this.labelImageLocation.Location = new System.Drawing.Point(135, 80);
            this.labelImageLocation.Name = "labelImageLocation";
            this.labelImageLocation.Size = new System.Drawing.Size(38, 12);
            this.labelImageLocation.TabIndex = 8;
            this.labelImageLocation.Text = "label9";
            // 
            // labelImageDimension
            // 
            this.labelImageDimension.AutoSize = true;
            this.labelImageDimension.Location = new System.Drawing.Point(135, 110);
            this.labelImageDimension.Name = "labelImageDimension";
            this.labelImageDimension.Size = new System.Drawing.Size(44, 12);
            this.labelImageDimension.TabIndex = 9;
            this.labelImageDimension.Text = "label10";
            // 
            // labelImageSize
            // 
            this.labelImageSize.AutoSize = true;
            this.labelImageSize.Location = new System.Drawing.Point(135, 140);
            this.labelImageSize.Name = "labelImageSize";
            this.labelImageSize.Size = new System.Drawing.Size(44, 12);
            this.labelImageSize.TabIndex = 10;
            this.labelImageSize.Text = "label11";
            // 
            // labelImageCreatedOn
            // 
            this.labelImageCreatedOn.AutoSize = true;
            this.labelImageCreatedOn.Location = new System.Drawing.Point(135, 170);
            this.labelImageCreatedOn.Name = "labelImageCreatedOn";
            this.labelImageCreatedOn.Size = new System.Drawing.Size(44, 12);
            this.labelImageCreatedOn.TabIndex = 11;
            this.labelImageCreatedOn.Text = "label12";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(137, 200);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "확인";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormFileInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 235);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelImageCreatedOn);
            this.Controls.Add(this.labelImageSize);
            this.Controls.Add(this.labelImageDimension);
            this.Controls.Add(this.labelImageLocation);
            this.Controls.Add(this.labelImageExtension);
            this.Controls.Add(this.labelImageName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormFileInfo";
            this.Text = "FormFileInfo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelImageName;
        private System.Windows.Forms.Label labelImageExtension;
        private System.Windows.Forms.Label labelImageLocation;
        private System.Windows.Forms.Label labelImageDimension;
        private System.Windows.Forms.Label labelImageSize;
        private System.Windows.Forms.Label labelImageCreatedOn;
        private System.Windows.Forms.Button button1;
    }
}