using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
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
        //string fileName;

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

                    //String text = string.Format("sigma:{0}", sigma);
                    //Cv2.PutText(dst, text, new OpenCvSharp.Point(10, 30), HersheyFonts.HersheyTriplex, 1.0, new OpenCvSharp.Scalar(255));

                    Cv2.ImShow("sigma:" + sigma, dst);
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

        private void 칼라분할ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 분리될 채널만큼의 1차원 배열 Mat
            // 3 채널 이미지의 경우, 아래와 같이 ..
            Mat[] splittedImage = new Mat[3];
            Mat mergedImage = new Mat();

            if (inputImage != null)
            {
                if (inputImage.Channels() == 1)
                {
                    MessageBox.Show("Count of image channel is 1");
                }
                else
                {
                    // 채널별 분리 (imread에서는 BGR 순으로)
                    Cv2.Split(inputImage, out splittedImage);
                    Cv2.ImShow("Channel B", splittedImage[0]);
                    Cv2.ImShow("Channel G", splittedImage[1]);
                    Cv2.ImShow("Channel R", splittedImage[2]);

                    int width = inputImage.Rows;
                    int height = inputImage.Cols;
                    //splittedImage[2] = Mat.Zeros(width, height, MatType.CV_8UC1);
                    Cv2.Merge(splittedImage, mergedImage);

                    PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mergedImage);
                    PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;

                    Cv2.WaitKey(0);
                    Cv2.DestroyAllWindows();
                }
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void bGR2GaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();
                //--- 여기에
                Cv2.CvtColor(inputImage, outputImage, ColorConversionCodes.BGR2GRAY);

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;

            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void bGRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 분리될 채널만큼의 1차원 배열 Mat
            Mat[] splittedImage = new Mat[3];

            if (inputImage != null)
            {
                outputImage = new Mat();
                //--- 여기에
                Cv2.CvtColor(inputImage, outputImage, ColorConversionCodes.BGR2YCrCb);

                Cv2.Split(outputImage, out splittedImage);
                Cv2.ImShow("Channel Y", splittedImage[0]);
                Cv2.ImShow("Channel Cr", splittedImage[1]);
                Cv2.ImShow("Channel Cb", splittedImage[2]);

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;

            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void bGR2RGBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();
                //--- 여기에
                Cv2.CvtColor(inputImage, outputImage, ColorConversionCodes.BGR2RGB);

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;

            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 퓨리에변환ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                int width = inputImage.Width;
                int height = inputImage.Height;

                Mat src = new Mat();
                if (inputImage.Channels() == 3)
                {
                    Cv2.CvtColor(inputImage, src, ColorConversionCodes.BGR2GRAY);
                }
                else
                {
                    src = inputImage;
                }

                // 이미지 크기 확장 (최적화된 DFT 크기 사용)
                Mat padded = new Mat();
                int m = Cv2.GetOptimalDFTSize(src.Rows);
                int n = Cv2.GetOptimalDFTSize(src.Cols);
                Cv2.CopyMakeBorder(src, padded, 0, m - src.Rows, 0, n - src.Cols, BorderTypes.Constant, Scalar.All(0));

                // 실수부와 허수부 채널 생성
                Mat realPart = new Mat();
                Mat imaginaryPart = Mat.Zeros(padded.Size(), MatType.CV_32F); // 허수부 (0으로 초기화)

                // 실수부 변환 (CV_32F 형식으로)
                padded.ConvertTo(realPart, MatType.CV_32F);

                // 실수부와 허수부의 채널 확인
                Console.WriteLine($"realPart Channels: {realPart.Channels()}"); // 1이어야 함
                Console.WriteLine($"imaginaryPart Channels: {imaginaryPart.Channels()}"); // 1이어야 함

                // 복합 이미지 생성 (2채널로 병합)
                Mat complexImage = new Mat();
                Cv2.Merge(new[] { realPart, imaginaryPart }, complexImage);

                // complexImage의 타입 확인
                Console.WriteLine($"complexImage Type: {complexImage.Type()}"); // 예상: CV_32FC2
                Console.WriteLine($"complexImage Channels: {complexImage.Channels()}"); // 예상: 2

                // Discrete Fourier Transform 수행
                Cv2.Dft(complexImage, complexImage, DftFlags.ComplexOutput);

                // DFT 결과를 분리
                Mat[] planes = Cv2.Split(complexImage);

                // Magnitude 계산
                Mat magnitude = new Mat();
                Cv2.Magnitude(planes[0], planes[1], magnitude);

                // 로그 스케일 변환
                Cv2.Add(magnitude, Scalar.All(1), magnitude); // log(1 + magnitude)
                Cv2.Log(magnitude, magnitude);

                // FFT Shift 적용 (저주파 성분을 중앙으로 이동)
                magnitude = FFTShift(magnitude);

                // 결과 이미지 정규화 (0~1 사이 값으로 변환)
                Mat magImage = new Mat();
                Cv2.Normalize(magnitude, magImage, 0, 1, NormTypes.MinMax);

                // 결과 출력
                Cv2.ImShow("Magnitude Spectrum", magImage);

                // 키 입력 대기 후 종료
                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();

                //PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(magImage);
                //PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        // FFT Shift 함수
        static Mat FFTShift(Mat mag)
        {
            // FFT Shift를 위해 이미지 크기를 절반으로 나눔
            int cx = mag.Cols / 2;
            int cy = mag.Rows / 2;

            // 4개 영역으로 분리
            Rect q0 = new Rect(0, 0, cx, cy);              // Top-Left
            Rect q1 = new Rect(cx, 0, cx, cy);            // Top-Right
            Rect q2 = new Rect(0, cy, cx, cy);            // Bottom-Left
            Rect q3 = new Rect(cx, cy, cx, cy);           // Bottom-Right

            Mat topLeft = new Mat(mag, q0);
            Mat topRight = new Mat(mag, q1);
            Mat bottomLeft = new Mat(mag, q2);
            Mat bottomRight = new Mat(mag, q3);

            // 사분면 교환 (Top-Left <-> Bottom-Right, Top-Right <-> Bottom-Left)
            Mat temp = new Mat();
            topLeft.CopyTo(temp);
            bottomRight.CopyTo(topLeft);
            temp.CopyTo(bottomRight);

            topRight.CopyTo(temp);
            bottomLeft.CopyTo(topRight);
            temp.CopyTo(bottomLeft);

            return mag;
        }

        private void 퓨리에변환함수ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                // 주파수변환
                Mat complexDFT = FrequencyTransform.ComputeDFT(inputImage);

                // 스펙트럼 시각화
                Mat spectrum = FrequencyTransform.GetMagnitudeSpectrum(complexDFT);

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(spectrum);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 퓨리에역변환ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                // 1. DFT 수행
                Mat complexDFT = FrequencyTransform.ComputeDFT(inputImage);

                // 2. 스펙트럼 시각화
                Mat spectrum = FrequencyTransform.GetMagnitudeSpectrum(complexDFT);

                // 3. IDFT 수행 (원본 복원)
                Mat reconstructed = FrequencyTransform.ComputeIDFT(complexDFT, inputImage.Size());

                // 4. 결과 표시
                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(reconstructed);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 변환후가우시안블러닝ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();
                //-- 여기에

                Mat gray = new Mat();
                Cv2.CvtColor(inputImage, gray, ColorConversionCodes.BGR2GRAY);

                // 1) DFT
                Mat floatImg = new Mat();
                gray.ConvertTo(floatImg, MatType.CV_32F);

                Mat[] planes = { floatImg, Mat.Zeros(gray.Size(), MatType.CV_32F) };
                Mat complex = new Mat();
                Cv2.Merge(planes, complex);
                Cv2.Dft(complex, complex);

                // 2) Shift (DFT 중심을 중앙으로 이동)
                Mat shifted = FourierGaussianBlur.ShiftDFT(complex);

                // 3) Gaussian LPF 생성
                Mat filter = FourierGaussianBlur.CreateGaussianFilter(gray.Size(), sigma: 40);

                // 4) 필터 적용 (element-wise multiplication)
                Mat filtered = new Mat();
                Cv2.MulSpectrums(shifted, filter, filtered, 0);

                // 5) 다시 Unshift (원래 위치로 되돌림)
                Mat unshifted = FourierGaussianBlur.ShiftDFT(filtered);

                // 6) IDFT
                Mat idft = new Mat();
                Cv2.Dft(unshifted, idft, DftFlags.Inverse | DftFlags.RealOutput | DftFlags.Scale);

                // 7) 정규화 후 출력
                Cv2.Normalize(idft, idft, 0, 255, NormTypes.MinMax);
                idft.ConvertTo(idft, MatType.CV_8U);

                // 4. 결과 표시
                //PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(idft);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 변환후가우시안블러링채널별ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();
                //-- 여기에

                Mat[] bgr = Cv2.Split(inputImage);    // R,G,B 분리
                Mat[] resultChannels = new Mat[3];

                // Gaussian Low-pass Filter (1개만 만들어서 모든 채널에 사용)
                //Mat filter = CreateGaussianFilter(inputImage.Size(), sigma: 40);
                Mat filter = FourierGaussianBlur.CreateGaussianFilter(inputImage.Size(), sigma: 40);

                for (int c = 0; c < 3; c++)
                {
                    Mat channel = bgr[c];

                    // 1) float 변환
                    Mat floatImg = new Mat();
                    channel.ConvertTo(floatImg, MatType.CV_32F);

                    // 2) DFT 변환
                    Mat[] planes = { floatImg, Mat.Zeros(channel.Size(), MatType.CV_32F) };
                    Mat complex = new Mat();
                    Cv2.Merge(planes, complex);
                    Cv2.Dft(complex, complex);

                    // 3) Shift
                    //Mat shifted = ShiftDFT(complex);
                    Mat shifted = FourierGaussianBlur.ShiftDFT(complex);

                    // 4) Spectral Filtering
                    Mat filtered = new Mat();
                    Cv2.MulSpectrums(shifted, filter, filtered, 0);

                    // 5) Unshift
                    //Mat unshifted = ShiftDFT(filtered);
                    Mat unshifted = FourierGaussianBlur.ShiftDFT(filtered);

                    // 6) IDFT
                    Mat idft = new Mat();
                    Cv2.Dft(unshifted, idft, DftFlags.Inverse | DftFlags.RealOutput | DftFlags.Scale);

                    // 7) 0~255로 정규화
                    Cv2.Normalize(idft, idft, 0, 255, NormTypes.MinMax);
                    idft.ConvertTo(idft, MatType.CV_8U);

                    // 8) 결과 채널 저장
                    resultChannels[c] = idft;
                }

                // 9) 필터링된 3개 채널 합치기
                Mat colorOutput = new Mat();
                Cv2.Merge(resultChannels, colorOutput);

                // 10) 출력
                // 4. 결과 표시
                //PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(colorOutput);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 변환후가우시안블러링칼라ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();
                //-- 여기에

                //int kernelSize = 31;  // GaussianBlur(Size(31,31))와 동일한 정도
                int kernelSize = 15;  
                //Mat outputImage = FourierGaussianBlur.ApplyGaussianFourierColor(inputImage, kernelSize);
                outputImage = FourierGaussianBlur.GaussianBlurFFT(inputImage, kernelSize);

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 노이즈생성ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                Mat noisedImage = new Mat(inputImage.Size(), MatType.CV_8UC3);

                Cv2.Randn(noisedImage, Scalar.All(0), Scalar.All(50));
                Cv2.ImShow("Filter", noisedImage);
                Cv2.AddWeighted(inputImage, 1, noisedImage, 1, 0, noisedImage);
                Cv2.ImShow("Noise", noisedImage);

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(noisedImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 노이즈추가제거ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                Mat noisedImage = new Mat(inputImage.Size(), MatType.CV_8UC3);
                Mat denoisedImage = new Mat(inputImage.Size(), MatType.CV_8UC3);

                Cv2.Randn(noisedImage, Scalar.All(0), Scalar.All(50));
                Cv2.AddWeighted(inputImage, 1, noisedImage, 1, 0, noisedImage);
                Cv2.ImShow("Noise", noisedImage);

                Cv2.FastNlMeansDenoisingColored(noisedImage, denoisedImage, 15, 15, 5, 10);
                //Cv2.FastNlMeansDenoising(noisedImage, denoisedImage, 3, 7, 21);
                Cv2.ImShow("Denoise", denoisedImage);

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(denoisedImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 노이즈제거효과ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                Mat noisedImage = new Mat(inputImage.Size(), MatType.CV_8UC3);
                Cv2.Randn(noisedImage, Scalar.All(0), Scalar.All(50));
                Cv2.AddWeighted(inputImage, 1, noisedImage, 1, 0, noisedImage);
                Cv2.ImShow("Noise", noisedImage);

                Mat removedNoiseMedianBlur = new Mat();
                Mat removedNoiseGaussianBlur = new Mat();
                Mat removedNoiseBilateralFilter = new Mat();
                Mat denoising = new Mat();

                Cv2.MedianBlur(noisedImage, removedNoiseMedianBlur, 3);
                Cv2.GaussianBlur(noisedImage, removedNoiseGaussianBlur, new OpenCvSharp.Size(5, 5), 3, 3);
                Cv2.BilateralFilter(noisedImage, removedNoiseBilateralFilter, 5, 250, 10);
                Cv2.FastNlMeansDenoisingColored(noisedImage, denoising, 15, 15, 5, 10);

                Cv2.ImShow("MedianBlur", removedNoiseMedianBlur);
                Cv2.ImShow("GaussianBlur", removedNoiseGaussianBlur);
                Cv2.ImShow("BilateralFilter", removedNoiseBilateralFilter);
                Cv2.ImShow("Denoising", denoising);

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(removedNoiseGaussianBlur);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 칼라히스토그램평활화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        public Mat HistogramEqualizationColor(Mat input)
        {
            // ① BGR → YCrCb 변환
            Mat ycrcb = new Mat();
            Cv2.CvtColor(input, ycrcb, ColorConversionCodes.BGR2YCrCb);

            // ② 채널 분리 (Y, Cr, Cb)
            Mat[] channels = Cv2.Split(ycrcb);

            // ③ Y 채널에만 히스토그램 평활화 수행
            Mat yEqualized = new Mat();
            Cv2.EqualizeHist(channels[0], yEqualized);

            // ④ 다시 채널 병합 (Y = equalized, Cr, Cb 유지)
            channels[0] = yEqualized;
            Mat merged = new Mat();
            Cv2.Merge(channels, merged);

            // ⑤ YCrCb → BGR 복원
            Mat result = new Mat();
            Cv2.CvtColor(merged, result, ColorConversionCodes.YCrCb2BGR);

            return result;
        }

        private void 칼라히스토그램평활화ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();
                //----------
                // 입력 영상이 컬러인지 확인
                if (inputImage.Channels() == 3)
                {
                    // ① B, G, R 채널 분리
                    Mat[] channels = Cv2.Split(inputImage);   // channels[0]=B, [1]=G, [2]=R

                    // ② 각 채널에 대해 Histogram Equalization 수행
                    Mat eqBlue = new Mat();
                    Mat eqGreen = new Mat();
                    Mat eqRed = new Mat();

                    Cv2.EqualizeHist(channels[0], eqBlue);
                    Cv2.EqualizeHist(channels[1], eqGreen);
                    Cv2.EqualizeHist(channels[2], eqRed);

                    Cv2.ImShow("equalized B", eqBlue); Cv2.ImShow("equalized G", eqGreen); Cv2.ImShow("equalized R", eqRed);

                    // ③ equalized 채널 3개를 다시 merge
                    //Mat output = new Mat();
                    Cv2.Merge(new Mat[] { eqBlue, eqGreen, eqRed }, outputImage);

                    // 4. 결과 표시
                    PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                    PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    MessageBox.Show("Input Image is NOT a color image ...");
                }
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 세피아필터ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();
                outputImage = Filters.ApplySepiaFilter(inputImage);
                //outputImage = Filters.ApplySepiaFilterEnhanced(inputImage);

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("ERROR] Image is NOT ready ...");
            }
        }

        private void 카툰화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();
                //---------- 여기
                outputImage = Cartoonizer.Cartoonify(inputImage);

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;

                //Cv2.ImShow("Cartoon Image", outputImage);
                //Cv2.WaitKey();
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 얼굴인식ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();
                //---------- 여기
                outputImage = VisionApplications.FaceDetection(inputImage);

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 좌우ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();

                Cv2.Flip(inputImage, outputImage, FlipMode.Y);
                // FlopMode 옵션 확인하기
                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 원점ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();

                Cv2.Flip(inputImage, outputImage, FlipMode.XY);
                // FlopMode 옵션 확인하기
                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 문턱ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();

                Mat grey = new Mat();    // 흑백이미지
                int thr = 100;
                if (inputImage.Channels() == 3) { grey = inputImage.CvtColor(ColorConversionCodes.BGR2GRAY); }
                else { grey = inputImage; }

                Cv2.Threshold(grey, outputImage, thr, 255, ThresholdTypes.Binary);
                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }

        }

        private void bGR2HSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 분리될 채널만큼의 1차원 배열 Mat
            Mat[] splittedImage = new Mat[3];

            if (inputImage != null)
            {
                outputImage = new Mat();
                //--- 여기에
                Cv2.CvtColor(inputImage, outputImage, ColorConversionCodes.BGR2HSV);

                Cv2.Split(outputImage, out splittedImage);
                Cv2.ImShow("Channel H", splittedImage[0]);
                Cv2.ImShow("Channel S", splittedImage[1]);
                Cv2.ImShow("Channel V", splittedImage[2]);

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 칼라평활화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();

                //--- 여기에
                Cv2.EqualizeHist(inputImage, outputImage);

                // 결과 표시
                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 소벨ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();

                //--- 여기에
                //Cv2.Sobel(inputImage, outputImage, MatType.CV_32F, 1, 1);
                Cv2.Sobel(inputImage, outputImage, MatType.CV_32F, 1, 0, ksize: 3, scale: 1, delta: 0, BorderTypes.Default);
                outputImage.ConvertTo(outputImage, MatType.CV_8U);

                // 결과 표시
                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 소벨그레이스케일ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();

                //--- 여기에
                Mat grey = new Mat();    // 흑백이미지
                if (inputImage.Channels() == 3) { grey = inputImage.CvtColor(ColorConversionCodes.BGR2GRAY); }
                Cv2.Sobel(grey, outputImage, MatType.CV_8U, 1, 1);

                // 결과 표시
                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 칼라히스토그램평활화변환후ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();

                //--- 여기에
                outputImage = HistogramEqualizationColor(inputImage);

                // 4. 결과 표시
                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 퓨리에변환챗GPTToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();

                //--- 여기에

                // 1. 이미지 경로 설정
                //string imagePath = @"C:\Images\input.jpg";

                // 2. 이미지 읽기
                //Mat src = Cv2.ImRead(imagePath, ImreadModes.Color);

                //if (src.Empty())
                //{
                //    Console.WriteLine("이미지를 불러올 수 없습니다.");
                //    return;
                //}

                Mat src = inputImage;

                // 3. 그레이스케일 변환
                Mat gray = new Mat();
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

                // 4. DFT 연산에 적합한 크기로 패딩
                int optimalRows = Cv2.GetOptimalDFTSize(gray.Rows);
                int optimalCols = Cv2.GetOptimalDFTSize(gray.Cols);

                Mat padded = new Mat();
                Cv2.CopyMakeBorder(
                    gray,
                    padded,
                    0,
                    optimalRows - gray.Rows,
                    0,
                    optimalCols - gray.Cols,
                    BorderTypes.Constant,
                    Scalar.All(0)
                );

                // 5. DFT 입력은 float 형식이어야 함
                Mat floatImage = new Mat();
                padded.ConvertTo(floatImage, MatType.CV_32FC1);

                // 6. 실수부와 허수부 생성
                Mat realPart = floatImage;
                Mat imaginaryPart = new Mat(padded.Size(), MatType.CV_32FC1, Scalar.All(0));

                Mat complexImage = new Mat();
                Cv2.Merge(new Mat[] { realPart, imaginaryPart }, complexImage);

                // 7. DFT 수행
                Cv2.Dft(complexImage, complexImage);

                // 8. 실수부와 허수부 분리
                Cv2.Split(complexImage, out Mat[] planes);

                Mat real = planes[0];
                Mat imaginary = planes[1];

                // 9. Magnitude 계산
                Mat magnitude = new Mat();
                Cv2.Magnitude(real, imaginary, magnitude);

                // 10. 로그 스케일 변환
                // magnitude = log(1 + magnitude)
                Cv2.Add(magnitude, Scalar.All(1), magnitude);
                Cv2.Log(magnitude, magnitude);

                // 11. 짝수 크기로 자르기
                magnitude = new Mat(
                    magnitude,
                    new Rect(
                        0,
                        0,
                        magnitude.Cols & -2,
                        magnitude.Rows & -2
                    )
                );

                // 12. 저주파 성분을 중앙으로 이동
                ShiftDFT(magnitude);

                // 13. 화면 표시를 위해 0~255 범위로 정규화
                Mat magnitudeDisplay = new Mat();
                Cv2.Normalize(
                    magnitude,
                    magnitudeDisplay,
                    0,
                    255,
                    NormTypes.MinMax
                );

                magnitudeDisplay.ConvertTo(magnitudeDisplay, MatType.CV_8UC1);

                // 14. 결과 출력
                Cv2.ImShow("Original Image", src);
                Cv2.ImShow("Gray Image", gray);
                Cv2.ImShow("Fourier Transform - Magnitude Spectrum", magnitudeDisplay);

                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();

                // 15. 메모리 해제
                //src.Dispose();
                //gray.Dispose();
                //padded.Dispose();
                //floatImage.Dispose();
                //imaginaryPart.Dispose();
                //complexImage.Dispose();
                //real.Dispose();
                //imaginary.Dispose();
                //magnitude.Dispose();
                //magnitudeDisplay.Dispose();


                //--- 여기까지
                // 결과 표시
                //PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                //PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        //---------------

        static void ShiftDFT(Mat image)
        {
            int cx = image.Cols / 2;
            int cy = image.Rows / 2;

            Mat q0 = new Mat(image, new Rect(0, 0, cx, cy));      // 좌상단
            Mat q1 = new Mat(image, new Rect(cx, 0, cx, cy));     // 우상단
            Mat q2 = new Mat(image, new Rect(0, cy, cx, cy));     // 좌하단
            Mat q3 = new Mat(image, new Rect(cx, cy, cx, cy));    // 우하단

            Mat temp = new Mat();

            // 좌상단 <-> 우하단
            q0.CopyTo(temp);
            q3.CopyTo(q0);
            temp.CopyTo(q3);

            // 우상단 <-> 좌하단
            q1.CopyTo(temp);
            q2.CopyTo(q1);
            temp.CopyTo(q2);

            temp.Dispose();
            q0.Dispose();
            q1.Dispose();
            q2.Dispose();
            q3.Dispose();
        }

        private void 퓨리에변환클로드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();

                //--- 여기에
                // 0. 입력 경로 결정 (인자가 없으면 기본 파일명 사용)
                //string path = args.Length > 0 ? args[0] : "input.jpg";

                // 1. 그레이스케일로 이미지 로드
                //Mat src = Cv2.ImRead(path, ImreadModes.Grayscale);
                //if (src.Empty())
                //{
                //    Console.WriteLine($"이미지를 불러올 수 없습니다: {path}");
                //    return;
                //}

                Mat loaded = inputImage;
                // 1. 이미지 로드
                //Mat loaded = Cv2.ImRead(path, ImreadModes.AnyColor);
                //if (loaded.Empty())
                //{
                //    Console.WriteLine($"이미지를 불러올 수 없습니다: {path}");
                //    return;
                //}
                // 1-1. ★ 반드시 단일 채널(그레이스케일)로 강제
                //      ImRead 옵션과 무관하게, 채널이 1이 아니면 직접 변환한다.
                //      (DFT는 3채널/4채널 영상을 받지 못함)
                Mat src = new Mat();
                if (loaded.Channels() == 1)
                    loaded.CopyTo(src);
                else
                    Cv2.CvtColor(loaded, src, ColorConversionCodes.BGR2GRAY);

                Console.WriteLine($"로드 영상 타입: {loaded.Type()} / 채널: {loaded.Channels()}");
                Console.WriteLine($"그레이 변환 후: {src.Type()} / 채널: {src.Channels()}");

                // 2. DFT 연산 속도를 위해 최적 크기로 0-패딩
                //    (2,3,5의 곱으로 분해되는 크기일 때 FFT가 가장 빠름)
                int optRows = Cv2.GetOptimalDFTSize(src.Rows);
                int optCols = Cv2.GetOptimalDFTSize(src.Cols);

                Mat padded = new Mat();
                Cv2.CopyMakeBorder(
                    src, padded,
                    top: 0, bottom: optRows - src.Rows,
                    left: 0, right: optCols - src.Cols,
                    BorderTypes.Constant, Scalar.All(0));

                // 3. 입력 영상을 32비트 float(CV_32FC1)로 변환
                //    ★ DFT 입력은 반드시 1채널 또는 2채널 float여야 한다.
                //      (CV_32FC1 / CV_32FC2 / CV_64FC1 / CV_64FC2)
                //      CV_8U 정수 영상이나 잘못된 채널수가 들어가면 다음 예외 발생:
                //      "type == CV_32FC1 || CV_32FC2 || CV_64FC1 || CV_64FC2"
                Mat floatImg = new Mat();
                padded.ConvertTo(floatImg, MatType.CV_32F);

                // (디버그) DFT 입력 타입 확인 — 정상이면 CV_32FC1 이어야 함
                Console.WriteLine($"DFT 입력 타입: {floatImg.Type()} / 채널: {floatImg.Channels()}");

                // 4. 순방향 DFT 수행
                //    DftFlags.ComplexOutput → 1채널 실수 입력을 2채널(복소수) 결과로 출력.
                //    (수동 Merge 없이도 안전하게 복소수 스펙트럼을 얻는 방식)
                Mat complex = new Mat();
                Cv2.Dft(floatImg, complex, DftFlags.ComplexOutput);

                // 5. 크기(Magnitude) 계산: sqrt(Re^2 + Im^2)
                Cv2.Split(complex, out Mat[] split);
                Mat magnitude = new Mat();
                Cv2.Magnitude(split[0], split[1], magnitude);
                foreach (var p in split) p.Dispose();

                // 6. 로그 스케일 변환: log(1 + magnitude)
                //    (저주파 성분이 너무 커서 그대로는 시각화가 어려움)
                Cv2.Add(magnitude, Scalar.All(1), magnitude);
                Cv2.Log(magnitude, magnitude);

                // 7. 홀수 행/열 제거 후 사분면 재배치(fftshift)
                //    → 저주파(DC) 성분을 영상 중앙으로 이동
                Mat spectrum = new Mat(
                    magnitude,
                    new Rect(0, 0, magnitude.Cols & -2, magnitude.Rows & -2));
                FftShift(spectrum);

                // 8. 0~1 범위로 정규화하여 디스플레이 가능하게 만듦
                Cv2.Normalize(spectrum, spectrum, 0, 1, NormTypes.MinMax);

                // 9. 결과 표시
                Cv2.ImShow("Input Image", src);
                Cv2.ImShow("Magnitude Spectrum", spectrum);
                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();

                // 결과 표시
                //PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                //PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        //---------
        private static void FftShift(Mat mag)
        {
            int cx = mag.Cols / 2;
            int cy = mag.Rows / 2;

            Mat q0 = new Mat(mag, new Rect(0, 0, cx, cy));    // 좌상
            Mat q1 = new Mat(mag, new Rect(cx, 0, cx, cy));   // 우상
            Mat q2 = new Mat(mag, new Rect(0, cy, cx, cy));   // 좌하
            Mat q3 = new Mat(mag, new Rect(cx, cy, cx, cy));  // 우하

            Mat tmp = new Mat();

            // 좌상 <-> 우하
            q0.CopyTo(tmp);
            q3.CopyTo(q0);
            tmp.CopyTo(q3);

            // 우상 <-> 좌하
            q1.CopyTo(tmp);
            q2.CopyTo(q1);
            tmp.CopyTo(q2);
        }

        private void 블러링챗GPTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();

                //--- 여기에
                // 1. 이미지 경로 설정
                //string imagePath = @"C:\Images\input.jpg";

                // 2. 이미지 읽기
                //Mat src = Cv2.ImRead(imagePath, ImreadModes.Color);
                Mat src = inputImage;

                if (src.Empty())
                {
                    Console.WriteLine("이미지를 불러올 수 없습니다.");
                    return;
                }

                // 3. 가우시안 블러링 적용
                Mat blurred = new Mat();

                // kernel size는 홀수여야 함: 3, 5, 7, 9, 15 ...
                // sigmaX, sigmaY가 클수록 더 많이 흐려짐
                Cv2.GaussianBlur(
                    src,
                    blurred,
                    new OpenCvSharp.Size(15, 15),
                    3.0,
                    3.0,
                    BorderTypes.Default
                );

                // 4. 결과 출력
                Cv2.ImShow("Original Image", src);
                Cv2.ImShow("Gaussian Blur - Spatial Domain", blurred);

                // 4. 결과 표시
                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(blurred);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;

                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();

                // 5. 메모리 해제
                //src.Dispose();
                //blurred.Dispose();
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 변환블러링챗GPTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();

                //--- 여기에

                // 1. 이미지 경로 설정
                //string imagePath = @"C:\Images\input.jpg";

                // 2. 이미지 읽기
                //Mat src = Cv2.ImRead(imagePath, ImreadModes.Color);
                Mat src = inputImage;

                if (src.Empty())
                {
                    Console.WriteLine("이미지를 불러올 수 없습니다.");
                    return;
                }

                // 3. B, G, R 채널 분리
                Cv2.Split(src, out Mat[] srcChannels);

                Mat[] blurredChannels = new Mat[srcChannels.Length];

                // 공간 도메인의 sigma와 비슷한 의미로 사용
                // 값이 클수록 더 강한 블러 효과가 나타남
                double sigmaSpatial = 3.0;

                // 4. 각 채널에 대해 주파수 도메인 가우시안 블러링 수행
                for (int i = 0; i < srcChannels.Length; i++)
                {
                    blurredChannels[i] = ApplyGaussianBlurFrequencyDomain(srcChannels[i], sigmaSpatial);
                }

                // 5. 처리된 채널 병합
                Mat frequencyBlurred = new Mat();
                Cv2.Merge(blurredChannels, frequencyBlurred);

                // 6. 결과 출력
                Cv2.ImShow("Original Image", src);
                Cv2.ImShow("Gaussian Blur - Frequency Domain", frequencyBlurred);

                // 4. 결과 표시
                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frequencyBlurred);
                //PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;

                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();

                // 7. 메모리 해제
                //src.Dispose();
                //frequencyBlurred.Dispose();
                //foreach (Mat ch in srcChannels) ch.Dispose();
                //foreach (Mat ch in blurredChannels) ch.Dispose();
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        //-----------------------------

        /// <summary>
        /// 단일 채널 이미지에 대해 주파수 도메인 Gaussian Low-Pass Filter 적용
        /// </summary>
        static Mat ApplyGaussianBlurFrequencyDomain(Mat inputChannel, double sigmaSpatial)
        {
            // 1. DFT 연산에 적합한 크기로 패딩
            int optimalRows = Cv2.GetOptimalDFTSize(inputChannel.Rows);
            int optimalCols = Cv2.GetOptimalDFTSize(inputChannel.Cols);

            Mat padded = new Mat();
            Cv2.CopyMakeBorder(
                inputChannel,
                padded,
                0,
                optimalRows - inputChannel.Rows,
                0,
                optimalCols - inputChannel.Cols,
                BorderTypes.Constant,
                Scalar.All(0)
            );

            // 2. DFT 입력은 float 형식이어야 함
            Mat floatImage = new Mat();
            padded.ConvertTo(floatImage, MatType.CV_32FC1);

            // 3. 복소수 영상 생성: 실수부 = 입력 영상, 허수부 = 0
            Mat imaginary = new Mat(padded.Size(), MatType.CV_32FC1, Scalar.All(0));

            Mat complexImage = new Mat();
            Cv2.Merge(new Mat[] { floatImage, imaginary }, complexImage);

            // 4. DFT 수행
            Cv2.Dft(complexImage, complexImage);

            // 5. Gaussian Low-Pass Filter 생성
            Mat gaussianFilter = CreateGaussianLowPassFilter(
                padded.Rows,
                padded.Cols,
                sigmaSpatial
            );

            // 6. 필터도 복소수 형태로 생성
            // 실수부와 허수부 모두 같은 필터를 넣으면
            // 복소수 스펙트럼의 실수부/허수부에 동일한 감쇠가 적용됨
            Mat filterComplex = new Mat();
            Cv2.Merge(new Mat[] { gaussianFilter, gaussianFilter }, filterComplex);

            // 7. 주파수 영역에서 스펙트럼 곱셈
            Mat filteredComplex = new Mat();
            Cv2.MulSpectrums(
                complexImage,
                filterComplex,
                filteredComplex,
                DftFlags.None,
                false
            );

            // 8. 역 DFT 수행
            Mat inverse = new Mat();
            Cv2.Dft(
                filteredComplex,
                inverse,
                DftFlags.Inverse | DftFlags.Scale | DftFlags.RealOutput
            );

            // 9. 원래 이미지 크기로 자르기
            Mat cropped = new Mat(
                inverse,
                new Rect(0, 0, inputChannel.Cols, inputChannel.Rows)
            );

            // 10. 화면 표시용 8비트 영상으로 변환
            Mat result = new Mat();
            cropped.ConvertTo(result, MatType.CV_8UC1);

            // 11. 메모리 해제
            padded.Dispose();
            floatImage.Dispose();
            imaginary.Dispose();
            complexImage.Dispose();
            gaussianFilter.Dispose();
            filterComplex.Dispose();
            filteredComplex.Dispose();
            inverse.Dispose();
            cropped.Dispose();

            return result;
        }

        /// <summary>
        /// 주파수 도메인용 Gaussian Low-Pass Filter 생성
        /// </summary>
        static Mat CreateGaussianLowPassFilter(int rows, int cols, double sigmaSpatial)
        {
            Mat filter = new Mat(rows, cols, MatType.CV_32FC1);

            double sigma2 = sigmaSpatial * sigmaSpatial;

            for (int y = 0; y < rows; y++)
            {
                // DFT 결과에서 DC 성분은 좌상단에 있으므로
                // 주파수 좌표를 원형 주기 구조로 계산
                int v = Math.Min(y, rows - y);

                for (int x = 0; x < cols; x++)
                {
                    int u = Math.Min(x, cols - x);

                    // 정규화된 주파수 좌표
                    double fx = (double)u / cols;
                    double fy = (double)v / rows;

                    // Gaussian Low-Pass Filter
                    // H(u,v) = exp(-2 * pi^2 * sigma^2 * (fx^2 + fy^2))
                    double value = Math.Exp(
                        -2.0 * Math.PI * Math.PI * sigma2 * (fx * fx + fy * fy)
                    );

                    filter.Set<float>(y, x, (float)value);
                }
            }

            return filter;
        }

        private void 가우시안블러링효과ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                outputImage = new Mat();
                int kernelSize = 15;  // GaussianBlur(Size(31,31))와 동일한 정도

                //--- 여기에
                // 공간 도메인에서의 가우시안 블러링을 적용
                // 3. 가우시안 블러링 적용
                Mat blurred = new Mat();

                // kernel size는 홀수여야 함: 3, 5, 7, 9, 15 ...
                // sigmaX, sigmaY가 클수록 더 많이 흐려짐
                double sigma = (kernelSize - 1) / 6.0;
                Cv2.GaussianBlur(inputImage, blurred,
                    new OpenCvSharp.Size(kernelSize, kernelSize), sigma, sigma, BorderTypes.Default
                );

                // 주파수 도메인에서 가우시안 블러링을 적용
                outputImage = FourierGaussianBlur.GaussianBlurFFT(inputImage, kernelSize);

                // 4. 결과 출력
                Cv2.ImShow("Original Image", inputImage);
                Cv2.ImShow("Gaussian Blur - Spatial Domain", blurred);
                Cv2.ImShow("Gaussian Blur - Frequency Domain", outputImage);

                // 4. 결과 표시
                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
                PB_OutputImage.SizeMode = PictureBoxSizeMode.StretchImage;

                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();

            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        //-----------------------------------
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
