using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                //GetImageHistogram(histoEqualizedImage);
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

                double gamma_value = 2.2;
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

        private void 공간필터링ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();
                /*
                //Mat mask = new Mat(blurringFactor, blurringFactor, MatType.CV_32F, new Scalar(1 / 9f));
                Mat mask = new Mat(3, 3, MatType.CV_32F, new Scalar(1 / 9f));
                Cv2.Filter2D(inputImage, outputImage, inputImage.Type(), mask, new OpenCvSharp.Point(0, 0));
                */

                float[] data = new float[9] { 1f/9, 1f/9, 1f/9, 1f/9, 1f/9, 1f/9, 1f/9, 1f/9, 1f/9 };
                Mat mask = new Mat(3, 3, MatType.CV_32F);
                for (int i = 0; i < 9; i++)
                {
                    mask.Set<float>(i / 3, i % 3, data[i]);
                }

                Cv2.Filter2D(inputImage, outputImage, inputImage.Type(), mask, new OpenCvSharp.Point(0, 0));

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

                
                
        private void 블러링ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();
                Cv2.Blur(inputImage, outputImage, new OpenCvSharp.Size(9, 9), new OpenCvSharp.Point(-1, -1), BorderTypes.Default);

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;

            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 공간필터링ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();
                float[] data = new float[9] { 0, -1, 0, -1, 5, -1, 0, -1, 0 };

                //Mat mask = new Mat(3,3, MatType.CV_32F, data);
                //Mat mask = Mat.FromArray<float>(data, 3, 3, MatType.CV_32F);
                Mat mask = new Mat(3, 3, MatType.CV_32F);
                for (int i = 0; i < 9; i++)
                {
                    mask.Set<float>(i / 3, i % 3, data[i]);
                }

                Cv2.Filter2D(inputImage, outputImage, inputImage.Type(), mask, new OpenCvSharp.Point(0, 0));

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;

            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 블러링효과ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();
                Cv2.GaussianBlur(inputImage, outputImage, new OpenCvSharp.Size(9, 9), 1, 1, BorderTypes.Default);

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;

                // -- 
                Mat blur = new Mat();
                Mat box_filter = new Mat();
                Mat median_blur = new Mat();
                Mat gaussian_blur = new Mat();
                Mat bilateral_filter = new Mat();

                Cv2.Blur(inputImage, blur, new OpenCvSharp.Size(9, 9), new OpenCvSharp.Point(-1, -1), BorderTypes.Default);
                Cv2.BoxFilter(inputImage, box_filter, MatType.CV_8UC3, new OpenCvSharp.Size(9, 9), new OpenCvSharp.Point(-1, -1), true, BorderTypes.Default);
                Cv2.MedianBlur(inputImage, median_blur, 9);
                Cv2.GaussianBlur(inputImage, gaussian_blur, new OpenCvSharp.Size(9, 9), 1, 1, BorderTypes.Default);
                Cv2.BilateralFilter(inputImage, bilateral_filter, 9, 3, 3, BorderTypes.Default);

                Cv2.ImShow("blur", blur);
                Cv2.ImShow("box_filter", box_filter);
                Cv2.ImShow("median_blur", median_blur);
                Cv2.ImShow("gaussian_blur", gaussian_blur);
                Cv2.ImShow("bilateral_filter", bilateral_filter);
                Cv2.WaitKey(0);

            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 필터링효과ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                Mat outputImageL = new Mat();
                Mat outputImageH = new Mat();
                float[] dataL = new float[9] { 1f/9, 1f/9, 1f/9, 1f/9, 1f/9, 1f/9, 1f/9, 1f/9, 1f/9 };
                float[] dataH = new float[9] { 0, -1, 0, -1, 5, -1, 0, -1, 0};
                Mat maskL = new Mat(3, 3, MatType.CV_32F);
                Mat maskH = new Mat(3, 3, MatType.CV_32F);
                for (int i = 0; i < 9; i++) { maskL.Set<float>(i / 3, i % 3, dataL[i]); }
                for (int i = 0; i < 9; i++) { maskH.Set<float>(i / 3, i % 3, dataH[i]); }
                
                Cv2.Filter2D(inputImage, outputImageL, inputImage.Type(), maskL, new OpenCvSharp.Point(0, 0));
                Cv2.Filter2D(inputImage, outputImageH, inputImage.Type(), maskH, new OpenCvSharp.Point(0, 0));

                Cv2.ImShow("블러링", outputImageL);
                Cv2.ImShow("샤프닝", outputImageH);
                Cv2.WaitKey(0);
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 샤프닝ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();
                Mat gBlur = new Mat();
                Cv2.GaussianBlur(inputImage, gBlur, new OpenCvSharp.Size(9, 9), 1, 1, BorderTypes.Default);

                outputImage = 2 * inputImage - gBlur;

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 샤프닝효과ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();

                Mat gBlur = new Mat();
                Mat blur = new Mat();
                Mat output1 = new Mat();
                Mat output2 = new Mat();
                Cv2.GaussianBlur(inputImage, gBlur, new OpenCvSharp.Size(9, 9), 1, 1, BorderTypes.Default);
                Cv2.Blur(inputImage, blur, new OpenCvSharp.Size(9, 9), new OpenCvSharp.Point(-1, -1), BorderTypes.Default);

                output1 = 2 * inputImage - gBlur;
                output2 = 2 * inputImage - blur;

                Cv2.ImShow("가우시안블러링", output1);
                Cv2.ImShow("평균블러링", output2);
                Cv2.WaitKey(0);

                //PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                //PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 샤프닝효과2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();
                for (int sigma = 1; sigma <= 5; sigma+=2)
                {
                    Mat blurred = new Mat();
                    Cv2.GaussianBlur(inputImage, blurred, new OpenCvSharp.Size(), sigma);

                    float alpha = 1.0f;
                    Mat dst = (1 + alpha) * inputImage - alpha * blurred;

                    String text = string.Format("sigma:{0}", sigma);
                    Cv2.PutText(dst, text, new OpenCvSharp.Point(10, 30), HersheyFonts.HersheyTriplex, 1.0, new OpenCvSharp.Scalar(255));

                    Cv2.ImShow("dst"+sigma, dst);
                }
                Cv2.WaitKey(0);


                //PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                //PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 캐니엣지추출ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();
                Mat blur = new Mat();

                Cv2.GaussianBlur(inputImage, blur, new OpenCvSharp.Size(3, 3), 1, 0, BorderTypes.Default);
                Cv2.Canny(blur, outputImage, 100, 200, 3, true);

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;

            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 엣지추출효과ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();

                Mat blur = new Mat();
                Mat sobel = new Mat();
                Mat scharr = new Mat();
                Mat laplacian = new Mat();
                Mat canny = new Mat();

                Cv2.GaussianBlur(inputImage, blur, new OpenCvSharp.Size(3, 3), 1, 0, BorderTypes.Default);

                Cv2.Sobel(blur, sobel, MatType.CV_32F, 1, 0, ksize: 3, scale: 1, delta: 0, BorderTypes.Default);
                sobel.ConvertTo(sobel, MatType.CV_8UC1);

                Cv2.Scharr(blur, scharr, MatType.CV_32F, 1, 0, scale: 1, delta: 0, BorderTypes.Default);
                scharr.ConvertTo(scharr, MatType.CV_8UC1);

                Cv2.Laplacian(blur, laplacian, MatType.CV_32F, ksize: 3, scale: 1, delta: 0, BorderTypes.Default);
                laplacian.ConvertTo(laplacian, MatType.CV_8UC1);

                Cv2.Canny(blur, canny, 100, 200, 3, true);

                Cv2.ImShow("sobel", sobel);
                Cv2.ImShow("scharr", scharr);
                Cv2.ImShow("laplacian", laplacian);
                Cv2.ImShow("canny", canny);
                Cv2.WaitKey(0);
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
