using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenCvSharp;

namespace ExamDIPCSharpOpenCV
{
    public partial class MainForm : ExamDIPCSharpOpenCV.Form1
    {
        // 변수 선언 (전역변수)
        Mat inputImage=null, resultImage=null;
        string fileName;

        public MainForm()
        {
            InitializeComponent();
        }

        private void 반전ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(inputImage != null)
            {
                resultImage = new Mat();

                Cv2.BitwiseNot(inputImage, resultImage);
                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resultImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 그레이스케일ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                resultImage = new Mat();

                Cv2.CvtColor(inputImage, resultImage, ColorConversionCodes.BGR2GRAY);
                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resultImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 좌우대칭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                resultImage = new Mat();

                Cv2.Flip(inputImage, resultImage, FlipMode.Y);
                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resultImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 퓨리에변환ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void 가우시안블러닝ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                resultImage = new Mat();

                Cv2.GaussianBlur(inputImage, resultImage, new OpenCvSharp.Size(9, 9), 1, 1, BorderTypes.Default);
                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resultImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 공간필터ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                resultImage = new Mat();
                Mat mask = new Mat(3, 3, MatType.CV_32F, new Scalar(1 / 9f));

                Cv2.Filter2D(inputImage, resultImage, inputImage.Type(), mask, new OpenCvSharp.Point(0, 0));

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resultImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        /*
        private void dFTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                resultImage = new Mat();

                Cv2.Dft(inputImage, resultImage, DftFlags.None, 0);
                Cv2.ImShow("Temp", resultImage);
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }
        */



        private void 오픈ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    fileName = dlg.FileName;
                    inputImage = Cv2.ImRead(dlg.FileName);
                    // Mat to Bitmap
                    PB_InputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(inputImage);
                    PB_InputImage.SizeMode = PictureBoxSizeMode.StretchImage;

                    PB_ResultImage.Image = null;
                    //Cv2.ImShow("Input Image", inputImage);
                    //Cv2.WaitKey(0);
                }
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}
