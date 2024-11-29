using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenCvSharp;
using System.IO;
using System.Windows.Media.Imaging;

namespace ExamDIPCSharpOpenCV
{
    public partial class FormFileInfo : Form
    {
        public FormFileInfo()
        {
            InitializeComponent();
        }

        public FormFileInfo(string fileName)
        {
            InitializeComponent();

            FileInfo fileInfo = new FileInfo(fileName);

            labelImageName.Text = fileInfo.Name;
            labelImageExtension.Text = fileInfo.Extension;
            labelImageLocation.Text = fileInfo.DirectoryName;
            labelImageDimension.Text = "";
            labelImageSize.Text = (fileInfo.Length / 1024.0).ToString("0.0") + "KB";
            labelImageCreatedOn.Text = fileInfo.CreationTime.ToString("yyyy MM dd, ddd");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
