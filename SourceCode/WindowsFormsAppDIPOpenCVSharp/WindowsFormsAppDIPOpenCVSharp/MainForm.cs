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

namespace WindowsFormsAppDIPOpenCVSharp
{
    public partial class MainForm : Form
    {
        // 변수 선언 (전역변수)
        Mat inputImage = null, outputImage = null;
        string fileName;

        public MainForm()
        {
            InitializeComponent();
        }

        private void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //Mat inputImage = Cv2.ImRead(dlg.FileName);
                    // 전역변수로 변경 (다른 메소드에서 함께 사용하려면)
                    inputImage = Cv2.ImRead(dlg.FileName);

                    PB_InputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(inputImage);
                    PB_InputImage.SizeMode = PictureBoxSizeMode.StretchImage;

                    // 우측 PictureBox는 클리어
                    PB_OutputImage.Image = null;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

        }

        private void 저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "JPEG Image|*.jpg|PNG Image|*.png";
            sfd.Title = "Save the filtered image";
            sfd.ShowDialog();

            if (sfd.FileName != "")
            {
                Bitmap filteredImage = (Bitmap)PB_OutputImage.Image;
                if (sfd.FilterIndex == 1)
                {
                    filteredImage.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else if (sfd.FilterIndex == 2)
                {
                    filteredImage.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
                else
                {
                    filteredImage.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }

        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 상하ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();

                Cv2.Flip(inputImage, outputImage, FlipMode.X);
                // FlopMode 옵션 확인하기
                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 히스토그램ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                // 다른 메소드에서 활용하기 위해 함수로 변경
                GetImageHistogram(inputImage);
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 반전ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();

                Cv2.BitwiseNot(inputImage, outputImage);
                // cv.Bitwise [And | Or | Xor | Not] 확인하기
                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 히스토그램평활화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                Mat grey = new Mat();    // 흑백이미지
                if (inputImage.Channels() == 3) { 
                    grey = inputImage.CvtColor(ColorConversionCodes.BGR2GRAY); 
                }
                else { 
                    grey = inputImage; 
                }

                // equalize the histogram
                Mat histoEqualizedImage = new Mat();
                Cv2.EqualizeHist(grey, histoEqualizedImage);

                // 히스토그램 평활화 결과를 (우측) picture box 에 뿌려줌
                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(histoEqualizedImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;

                // Histogram view
                //GetHistogram(histoEqualizedImage);
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 감마보정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();

                double gamma_value = 0.5;
                // 감마값이 0.5, 1.0, 2.0, 2.2, 3.0, 4.0일때 결과를 확인하기
                byte[] lut = new byte[256];

                for (int i = 0; i < lut.Length; i++)
                {
                    lut[i] = (byte)(Math.Pow(i / 255.0, 1.0 / gamma_value) * 255.0);
                }

                //Cv.LUT(src, gamma, lut);
                Cv2.LUT(inputImage, lut, outputImage);


                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        // ------
        private void GetImageHistogram(Mat inputImage)
        {
            Mat grey = new Mat();  // 흑백이미지
            if (inputImage.Channels() == 3)
            {
                grey = inputImage.CvtColor(ColorConversionCodes.BGR2GRAY);
            }
            else
            {
                grey = inputImage;
            }


            // Histogram view
            const int width = 260, height = 200;
            Mat render = new Mat(new OpenCvSharp.Size(width, height), MatType.CV_8UC3, Scalar.All(255));

            // calculate histogram
            Mat hist = new Mat();   //히스토그램
            int[] hdims = { 256 };
            Rangef[] ranges = { new Rangef(0, 256), };  // min max
            Cv2.CalcHist(new Mat[] { grey }, new int[] { 0 }, null, hist, 1, hdims, ranges);

            // Get the max value of histogram
            double minVal, maxVal;
            Cv2.MinMaxLoc(hist, out minVal, out maxVal);

            Scalar color = Scalar.All(100);

            // scales and draws histogram
            hist = hist * (maxVal != 0 ? height / maxVal : 0.0);
            for (int i = 0; i < hdims[0]; i++)
            {
                int binW = (int)((double)width / hdims[0]);
                render.Rectangle(
                    new OpenCvSharp.Point(i * binW, render.Rows - (int)hist.Get<float>(i)),
                    new OpenCvSharp.Point((i + 1) * binW, render.Rows),
                    color,
                    -1);
            }
            new Window("Image", grey);
            new Window("Histogram", render);

            Cv2.WaitKey();
            Cv2.DestroyAllWindows();
        }
    }
}
