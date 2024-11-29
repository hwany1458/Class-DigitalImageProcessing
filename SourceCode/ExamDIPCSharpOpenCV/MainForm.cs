using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using OpenCvSharp;
using System.IO;
using System.Windows.Media.Imaging;
using ExifPhotoReader;

namespace ExamDIPCSharpOpenCV
{
    public partial class MainForm : ExamDIPCSharpOpenCV.Form1
    {
        // 변수 선언 (전역변수)
        Mat inputImage=null, resultImage=null;
        string fileName;
        int blurringFactor = 3;
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
            if (inputImage != null)
            {
                int width = inputImage.Width;
                int height = inputImage.Height;

                Mat src = new Mat();
                Cv2.CvtColor(inputImage, src, ColorConversionCodes.BGR2GRAY);
                /*
                //Mat temp = new Mat(new OpenCvSharp.Size(width, height), MatType.CV_8UC3, Scalar.All(255));

                Mat temp = new Mat(new OpenCvSharp.Size(width, height), MatType.CV_32FC1);
                Mat imgFloat32 = new Mat();
                //Cv2.Multiply(grayscaleImage, temp, imgFloat32, 1, OpenCvSharp.MatType.CV_32FC1);
                Cv2.Multiply(grayscaleImage, temp, imgFloat32, 1, MatType.CV_32F);
                //Cv2.ImShow("float32", imgFloat32);
                
                Mat dst = new Mat();
                Cv2.Dft(imgFloat32, dst, DftFlags.ComplexOutput);
                */

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

                // Depth of the image must be CV_8U
                //PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(magImage);
                //PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
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

        }

        private void 이미지정보ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if(!inputImage.Empty())
            if (fileName != null)
            {
                //MessageBox.Show(fileName);

                // 서브 윈도우 폼 준비
                string subStr = fileName.Substring(fileName.IndexOf('.')+1).Trim();
                if (subStr == "tiff" || subStr == "tif" || subStr == "jpeg" || subStr == "jpg")
                {
                    FormDataGridView subWin = new FormDataGridView(fileName);
                    subWin.Show();
                }
            }
            else
            {
                MessageBox.Show("ERROR] Image is NOT ready ...");
            }
        }

        private void eXIFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileName != null)
            {
                //MessageBox.Show(fileName);

                // 1,2번 중 하나를 선택하여 EXIF를 읽어옴
                // (1) Get EXIF data from an image file via a path
                ExifImageProperties exifImage = ExifPhoto.GetExifDataPhoto(fileName);

                // (2) Get EXIF data from an image through a Bitmap object
                //Image file = new Bitmap(fileName);
                //ExifImageProperties exifImage = ExifPhoto.GetExifDataPhoto(file);

                //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string gps = "[" + exifImage.GPSInfo.Latitude + ", " + exifImage.GPSInfo.Longitude + "]";
                string cameraModel = exifImage.Model;
                MessageBox.Show(gps + " " + cameraModel, "GPS Info");
            }
            else
            {
                MessageBox.Show("ERROR] Image is NOT ready ...");
            }
        }

        private void 히스토그램ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                // 함수로 빼낸다 ..
                GetHistogram(inputImage);

                /* -----
                //Mat grey = new Mat();  // 흑백이미지
                Mat grey = inputImage.CvtColor(ColorConversionCodes.BGR2GRAY);

                // Histogram view
                const int width = 260, height = 200;
                Mat render = new Mat(new OpenCvSharp.Size(width, height), MatType.CV_8UC3, Scalar.All(255));

                // calculate histogram
                Mat hist = new Mat();   //히스토그램
                int[] hdims = { 256 };
                Rangef[] ranges = { new Rangef(0, 256), };  // min max
                Cv2.CalcHist(
                    new Mat[] { grey },  // Mat image
                    new int[] { 0 },   // channels : grayscale=0, R,G,B=0,1,2
                    null,  // mask : all area = null
                    hist,  // output,
                    1,   // dims
                    hdims,   // hist size
                    ranges);  // range

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
                        new OpenCvSharp.Point((i+1) * binW, render.Rows),
                        color,
                        -1);
                }
                //new Window("Image", grey);
                new Window("Histogram", render);

                Cv2.WaitKey();
                Cv2.DestroyAllWindows();
                ----- */

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
                //Mat grey = new Mat();  // 흑백이미지
                Mat grey = inputImage.CvtColor(ColorConversionCodes.BGR2GRAY);

                // equalize the histogram
                Mat histoEqualizedImage = new Mat();
                Cv2.EqualizeHist(grey, histoEqualizedImage);

                // 히스토그램 평활화 결과를 (우측) picture box 에 뿌려줌
                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(histoEqualizedImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;

                // Histogram view
                //GetHistogram(histoEqualizedImage);
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
                float[] kernelData = new float[9] { 0, -1, 0, -1, 5, -1, 0, -1, 0 };
                //Mat kernel = new Mat(3, 3, MatType.CV_32F, kernelData);
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 파일정보ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FormFileInfo subWin = new FormFileInfo(fileName);
            //subWin.Show();

            // DataGrid 형태로 변경
            FormDataGridView subWin = new FormDataGridView(fileName, inputImage);
            subWin.Show();
        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 리셋ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(fileName != null)
            {
                inputImage = Cv2.ImRead(fileName);
                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(inputImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 이미지메타정보ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileName != null)
            {
                // DataGrid 형태로 변경
                FormDataGridView subWin = new FormDataGridView(fileName, inputImage);
                subWin.Show();
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void eXIFToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (fileName != null)
            {
                //MessageBox.Show(fileName);

                // 1,2번 중 하나를 선택하여 EXIF를 읽어옴
                // (1) Get EXIF data from an image file via a path
                ExifImageProperties exifImage = ExifPhoto.GetExifDataPhoto(fileName);

                // (2) Get EXIF data from an image through a Bitmap object
                //Image file = new Bitmap(fileName);
                //ExifImageProperties exifImage = ExifPhoto.GetExifDataPhoto(file);

                //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string gps = "[" + exifImage.GPSInfo.Latitude + ", " + exifImage.GPSInfo.Longitude + "]";
                string cameraModel = exifImage.Model;
                MessageBox.Show(gps + " " + cameraModel, "GPS Info");
            }
            else
            {
                MessageBox.Show("ERROR] Image is NOT ready ...");
            }
        }

        private void softSepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileName != null)
            {
                float[] maskData = new float[9] { 0.272f, 0.534f, 0.131f, 0.349f, 0.686f, 0.168f, 0.393f, 0.769f, 0.189f };
                float[] data = { 0.272f, 0.534f, 0.131f, 0.349f, 0.686f, 0.168f, 0.393f, 0.769f, 0.189f };
                double[] dataD = { 0.272, 0.534, 0.131, 0.349, 0.686, 0.168, 0.393, 0.769, 0.189 };

                //Mat maskData = new Mat(3, 3, MatType.CV_32F, data);
                //Mat kernel = new Mat(3, 3, MatType.CV_32F, Scalar.All(0.0f));
                //Mat mask = new Mat(3, 3, MatType.CV_32F, new Scalar(1 / 9f));
                //Mat render = new Mat(new OpenCvSharp.Size(3, 3), MatType.CV_32F, new Scalar(1 / 9f));

                Mat kernel = new Mat();

            }
            else
            {
                MessageBox.Show("ERROR] Image is NOT ready ...");
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
                    //inputImage = Cv2.ImRead(dlg.FileName); 
                    // default : Color
                    //inputImage = Cv2.ImRead(dlg.FileName, ImreadModes.Grayscale);
                    inputImage = Cv2.ImRead(dlg.FileName, ImreadModes.Unchanged);
                    // Imread() 메소드 참조
                    //https://shimat.github.io/opencvsharp_docs/html/9ce0f7ce-cc6a-8778-75e5-3a33949a537a.htm
                    // ImreadModes 참조
                    //https://shimat.github.io/opencvsharp_docs/html/f2c6a40e-a45a-d2da-e1fc-bbe6e4a2fd31.htm

                    //MessageBox.Show(fileName + " [" + inputImage.Rows + "," + inputImage.Cols + "] ");
                    labelFileName.Text = fileName;

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

        private void 블러링ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                resultImage = new Mat();
                //Mat mask = new Mat(3, 3, MatType.CV_32F, new Scalar(1 / 9f));
                //Cv2.Filter2D(inputImage, resultImage, inputImage.Type(), mask, new OpenCvSharp.Point(0, 0));
                Cv2.Blur(inputImage, resultImage, new OpenCvSharp.Size(blurringFactor, blurringFactor), new OpenCvSharp.Point(-1, -1), BorderTypes.Default);

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resultImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 박스필터ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                resultImage = new Mat();
                Cv2.BoxFilter(inputImage, resultImage, MatType.CV_8UC3, new OpenCvSharp.Size(blurringFactor, blurringFactor), new OpenCvSharp.Point(-1, -1), true, BorderTypes.Default);

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resultImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 메디안블러링ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                resultImage = new Mat();
                Cv2.MedianBlur(inputImage, resultImage, blurringFactor);

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resultImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 가우시안블러ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                resultImage = new Mat();
                Cv2.GaussianBlur(inputImage, resultImage, new OpenCvSharp.Size(blurringFactor, blurringFactor), 1, 1, BorderTypes.Default);

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resultImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 양방향필터ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                resultImage = new Mat();
                Cv2.BilateralFilter(inputImage, resultImage, 9, 3, 3, BorderTypes.Default);
                Cv2.GaussianBlur(inputImage, resultImage, new OpenCvSharp.Size(blurringFactor, blurringFactor), 1, 1, BorderTypes.Default);

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resultImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
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
                resultImage = new Mat();
                Mat mask = new Mat(blurringFactor, blurringFactor, MatType.CV_32F, new Scalar(1 / 9f));
                Cv2.Filter2D(inputImage, resultImage, inputImage.Type(), mask, new OpenCvSharp.Point(0, 0));

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resultImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 그레이스케일ToolStripMenuItem1_Click(object sender, EventArgs e)
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

        private void 이진화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                resultImage = new Mat();
                Mat grayscale = new Mat();

                Cv2.CvtColor(inputImage, grayscale, ColorConversionCodes.BGR2GRAY);
                // 기준이 될 임계치 = (256/2)
                Cv2.Threshold(grayscale, resultImage, (256/2), 255, ThresholdTypes.Binary);

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resultImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 적응형임계치ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                resultImage = new Mat();
                Mat grayscale = new Mat();
                int blockSize = 9;
                int c = 5;

                Cv2.CvtColor(inputImage, grayscale, ColorConversionCodes.BGR2GRAY);
                Cv2.AdaptiveThreshold(grayscale, resultImage, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, blockSize, c);
                //Cv2.AdaptiveThreshold(grayscale, resultImage, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, blockSize, c);

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resultImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 클리어ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //inputImage = new Mat();
            inputImage = null;
            PB_InputImage.Image = null;
            PB_ResultImage.Image = null;
        }

        private void 테스트ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void 노이즈이미지ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //check if the image is chosen
            if (inputImage != null)
            {
                Mat noisedImage = new Mat(inputImage.Size(), MatType.CV_8UC3);

                Cv2.Randn(noisedImage, Scalar.All(0), Scalar.All(50));
                //Cv2.ImShow("Filter", noisedImage);
                Cv2.AddWeighted(inputImage, 1, noisedImage, 1, 0, noisedImage);
                //Cv2.ImShow("Noise", noisedImage);

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(noisedImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
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
                Bitmap filteredImage = (Bitmap)PB_ResultImage.Image;
                if (sfd.FilterIndex == 1)
                { 
                    filteredImage.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg); 
                } else if (sfd.FilterIndex == 2)
                {
                    filteredImage.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                } else
                {
                    filteredImage.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }
        }

        private void 노이지제거ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                Mat removedNoiseMedianBlur = new Mat();
                Mat removedNoiseGaussianBlur = new Mat();
                Mat removedNoiseBilateralFilter = new Mat();
                Mat denoising = new Mat();

                Cv2.MedianBlur(inputImage, removedNoiseMedianBlur, 3);
                Cv2.GaussianBlur(inputImage, removedNoiseGaussianBlur, new OpenCvSharp.Size(5, 5), 3, 3);
                Cv2.BilateralFilter(inputImage, removedNoiseBilateralFilter, 5, 250, 10);
                Cv2.FastNlMeansDenoisingColored(inputImage, denoising, 15, 15, 5, 10);

                Cv2.ImShow("MedianBlur", removedNoiseMedianBlur);
                Cv2.ImShow("GaussianBlur", removedNoiseGaussianBlur);
                Cv2.ImShow("BilateralFilter", removedNoiseBilateralFilter);
                Cv2.ImShow("Denoising", denoising);

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(removedNoiseGaussianBlur);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 카툰화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                var width = inputImage.Width;
                var height = inputImage.Height;

                //-------------- B type
                // https://charlezz.com/?p=45040
                /*
                Mat resizedImage = new Mat();
                // 이미지의 크기를 줄이면 효과적으로 뭉개고, 연산량을 빨리 하는 효과가 있음
                Cv2.Resize(inputImage, resizedImage, new OpenCvSharp.Size(width/4, height/4));

                // 블러 적용
                Mat blur = new Mat();
                Cv2.BilateralFilter(resizedImage, blur, -1, 20.0, 7.0);

                // 엣지 검출한 뒤, 이미지를 반전시킨다.
                Mat edge = new Mat();
                Mat edgeBitwiseNot = new Mat();
                Mat edgeBitwiseNotColor = new Mat();
                Cv2.Canny(resizedImage, edge, 100.0, 150.0);
                Cv2.BitwiseNot(edge, edgeBitwiseNot);
                Cv2.CvtColor(edgeBitwiseNot, edgeBitwiseNotColor, ColorConversionCodes.GRAY2BGR);

                //블러시킨 이미지와 반전된 edge를 and연산자로 합치면 edge부분은 검정색으로 나오고,
                //나머지는 많이 뭉개지고 블러처리된 이미지로 나옴, 카툰효과
                Mat dst = new Mat();
                Mat resizedDst = new Mat();
                Cv2.BitwiseAnd(blur, edgeBitwiseNotColor, dst);
                Cv2.Resize(dst, resizedDst, new OpenCvSharp.Size(width, height), 1.0, 1.0, InterpolationFlags.Nearest);
                */
                Mat resizedDst = new Mat();
                Cv2.Stylization(inputImage, resizedDst, 50, 0.2f);

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resizedDst);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;

                /*
                //-------------- A type
                // https://www.analyticsvidhya.com/blog/2022/06/cartoonify-image-using-opencv-and-python/
                // (1) convert to grayscale image
                Mat inputGrayImage = new Mat();
                Cv2.CvtColor(inputImage, inputGrayImage, ColorConversionCodes.RGB2GRAY);
                //Cv2.ImShow("Gray", inputGrayImage);
                
                // (1-2) 프로세싱 타임을 고려하여 이미지 크기 조정
                Mat resizedGrayscaleImage = new Mat();
                Cv2.Resize(inputGrayImage, resizedGrayscaleImage, new OpenCvSharp.Size(width/4, height/4));
                //Cv2.ImShow("Resized Gray", resizedGrayscaleImage);

                // (2) applying median blur to smoothen an image
                Mat smoothGrayScale = new Mat();
                Cv2.MedianBlur(resizedGrayscaleImage, smoothGrayScale, 5);
                //Cv2.ImShow("Median Blurring", smoothGrayScale);

                // (3) retrieving the edges for cartoon effect
                Mat getEdge = new Mat();
                Cv2.AdaptiveThreshold(smoothGrayScale, getEdge, 255, 
                    AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 9, 9);
                //Cv2.ImShow("Adaptive Thresholding -Edge", getEdge);

                // (4) applying bilateral filter to remove noise and keep edge sharp as required
                Mat colorImage = new Mat();
                Cv2.BilateralFilter(inputImage, colorImage, 9, 300, 300);
                //Cv2.ImShow("Bilateral Filtering", colorImage);

                // 이미지 크기를 (원 이미지 크기로) 복구
                Mat resizedColorImage = new Mat();
                Mat resizedGetEdge = new Mat();
                Cv2.Resize(colorImage, resizedColorImage, new OpenCvSharp.Size(width, height));
                Cv2.Resize(getEdge, resizedGetEdge, new OpenCvSharp.Size(width, height));
                //Cv2.ImShow("Resized Color image", resizedColorImage);

                // masking edged image with ...
                Mat cartoonImage = new Mat();
                Cv2.BitwiseAnd(resizedColorImage, resizedColorImage, cartoonImage, resizedGetEdge);
                //Cv2.ImShow("Bitwise AND -Cartoon", cartoonImage);

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(cartoonImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
                */

                // ---------- C type
                // https://my-coding-footprints.tistory.com/156

                // ----------- D type
                // https://dev.to/ethand91/cartoon-filter-using-opencv-and-python-3nj5
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 스케치ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                var width = inputImage.Width;
                var height = inputImage.Height;

                // https://charlezz.com/?p=45040
                
                // (1) convert to grayscale image
                Mat inputGrayImage = new Mat();
                Cv2.CvtColor(inputImage, inputGrayImage, ColorConversionCodes.RGB2GRAY);

                // (2) applying gaussian blur to the grayscale image
                Mat blurredImage = new Mat();
                Cv2.GaussianBlur(inputGrayImage, blurredImage, new OpenCvSharp.Size(0.0, 0.0), 3.0);

                // (3) 엣지만 남고 평탄한 부분은 흰색으로 바꿈
                Mat finalImage = new Mat();
                Cv2.Divide(inputGrayImage, blurredImage, finalImage, 255.0);

                // ---
                // https://sooho-kim.tistory.com/202?category=935408
                //Cv2.PencilSketch()

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(finalImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
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
                    // 채널 분리 시에, 1차원 배열 Mat 앞에 out을 붙여줘야 함
                    Cv2.Split(inputImage, out splittedImage);
                    Cv2.ImShow("Channel B", splittedImage[0]);
                    Cv2.ImShow("Channel G", splittedImage[1]);
                    Cv2.ImShow("Channel R", splittedImage[2]);

                    // 병합 전에, 임의의 변경을 추가해서 살펴본다 (테스트로) ----
                    // (예) R 채널 이미지를 0 이미지로 변경
                    int width = inputImage.Rows;
                    int height = inputImage.Cols;
                    splittedImage[2] = Mat.Zeros(width, height, MatType.CV_8UC1);
                    // ---- 여기까지

                    // 채널 병합
                    Cv2.Merge(splittedImage, mergedImage);

                    PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mergedImage);
                    PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;

                    Cv2.WaitKey(0);
                    Cv2.DestroyAllWindows();
                }
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }


        //-------------------------- user defined method

        private void GetHistogram(Mat inImage)
        {
            // 흑백이미지
            Mat grey = inputImage.CvtColor(ColorConversionCodes.BGR2GRAY);

            // Histogram view
            const int width = 260, height = 200;
            Mat render = new Mat(new OpenCvSharp.Size(width, height), MatType.CV_8UC3, Scalar.All(255));

            // calculate histogram
            Mat hist = new Mat();   //히스토그램
            int[] hdims = { 256 };
            Rangef[] ranges = { new Rangef(0, 256), };  // min max

            Cv2.CalcHist(
                new Mat[] { grey },  // Mat image
                new int[] { 0 },   // channels : grayscale=0, R,G,B=0,1,2
                null,  // mask : all area = null
                hist,  // output,
                1,   // dims
                hdims,   // hist size
                ranges);  // range

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
            //new Window("Image", grey);
            new Window("Histogram", render);

            Cv2.WaitKey();
            Cv2.DestroyAllWindows();
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

        private void rGB2GrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mat convertedImage = new Mat();

            if (inputImage != null)
            {
                Cv2.CvtColor(inputImage, convertedImage, ColorConversionCodes.BGR2GRAY);

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(convertedImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void rGB2HSIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mat convertedImage = new Mat();
            // 분리될 채널만큼의 1차원 배열 Mat
            Mat[] splittedImage = new Mat[3];

            if (inputImage != null)
            {
                Cv2.CvtColor(inputImage, convertedImage, ColorConversionCodes.BGR2HSV);

                Cv2.Split(convertedImage, out splittedImage);
                Cv2.ImShow("Channel H", splittedImage[0]);
                Cv2.ImShow("Channel S", splittedImage[1]);
                Cv2.ImShow("Channel V", splittedImage[2]);

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(convertedImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void rGB2YCbCrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mat convertedImage = new Mat();
            // 분리될 채널만큼의 1차원 배열 Mat
            Mat[] splittedImage = new Mat[3];

            if (inputImage != null)
            {
                Cv2.CvtColor(inputImage, convertedImage, ColorConversionCodes.BGR2YCrCb);

                Cv2.Split(convertedImage, out splittedImage);
                Cv2.ImShow("Channel Y", splittedImage[0]);
                Cv2.ImShow("Channel Cr", splittedImage[1]);
                Cv2.ImShow("Channel Cb", splittedImage[2]);

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(convertedImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }

        }

        private void 노이즈추가N제거ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                Mat noisedImage = new Mat(inputImage.Size(), MatType.CV_8UC3);
                Mat denoisedImage = new Mat(inputImage.Size(), MatType.CV_8UC3);

                Cv2.Randn(noisedImage, Scalar.All(0), Scalar.All(50));
                //Cv2.ImShow("Filter", noisedImage);
                Cv2.AddWeighted(inputImage, 1, noisedImage, 1, 0, noisedImage);
                Cv2.ImShow("Noise", noisedImage);

                Cv2.FastNlMeansDenoisingColored(noisedImage, denoisedImage, 15, 15, 5, 10);
                //Cv2.FastNlMeansDenoising(noisedImage, denoisedImage, 3, 7, 21);
                Cv2.ImShow("Denoise", denoisedImage);

                //PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(noisedImage);
                //PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        private void 가우시안샤프닝1ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 푸리에역변환ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void rGB2CMYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputImage != null)
            {
                Mat rgbImage = new Mat();
                Cv2.CvtColor(inputImage, rgbImage, ColorConversionCodes.BGR2RGB);

                Scalar white = new Scalar(255, 255, 255);
                Mat CMYImage = white - rgbImage;

                Mat[] splittedCMYImage = new Mat[3];
                Cv2.Split(CMYImage, out splittedCMYImage);

                Mat black = new Mat();
                Cv2.Min(splittedCMYImage[0], splittedCMYImage[1], black);
                Cv2.Min(black, splittedCMYImage[2], black);

                Cv2.ImShow("C", splittedCMYImage[0]);
                Cv2.ImShow("M", splittedCMYImage[1]);
                Cv2.ImShow("Y", splittedCMYImage[2]);
                Cv2.ImShow("K", black);

                Mat mergedCMYImage = new Mat();
                Cv2.Merge(splittedCMYImage, mergedCMYImage);

                PB_ResultImage.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(CMYImage);
                PB_ResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Input Image is NOT ready ...");
            }
        }

        // Inverse FFT Shift 함수
        static Mat InverseFFTShift(Mat mag)
        {
            int cx = mag.Cols / 2;
            int cy = mag.Rows / 2;

            Rect q0 = new Rect(0, 0, cx, cy);              // Top-Left
            Rect q1 = new Rect(cx, 0, cx, cy);            // Top-Right
            Rect q2 = new Rect(0, cy, cx, cy);            // Bottom-Left
            Rect q3 = new Rect(cx, cy, cx, cy);           // Bottom-Right

            Mat topLeft = new Mat(mag, q0);
            Mat topRight = new Mat(mag, q1);
            Mat bottomLeft = new Mat(mag, q2);
            Mat bottomRight = new Mat(mag, q3);

            Mat temp = new Mat();
            bottomRight.CopyTo(temp);
            topLeft.CopyTo(bottomRight);
            temp.CopyTo(topLeft);

            bottomLeft.CopyTo(temp);
            topRight.CopyTo(bottomLeft);
            temp.CopyTo(topRight);

            return mag;
        }
    }
}
