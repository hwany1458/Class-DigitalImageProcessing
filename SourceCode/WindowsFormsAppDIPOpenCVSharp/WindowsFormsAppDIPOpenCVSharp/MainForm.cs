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
                    splittedImage[2] = Mat.Zeros(width, height, MatType.CV_8UC1);
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

                //PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(magImage);
                //PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
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
                //outputImage = new Mat();

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

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(idft);

                // 4. 결과 표시
                //PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
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
                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(colorOutput);

                // 4. 결과 표시
                //PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
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

                int kernelSize = 31;  // GaussianBlur(Size(31,31))와 동일한 정도
                //Mat result = FourierGaussianBlur.ApplyGaussianFourierColor(inputImage, kernelSize);
                Mat blurred = FourierGaussianBlur.GaussianBlurFFT(inputImage, kernelSize);

                PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(blurred);
                //PB_OutputImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(outputImage);
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
