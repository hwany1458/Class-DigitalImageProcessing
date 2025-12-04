using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
namespace WindowsFormsAppDIPOpenCVSharp
{
    internal class FourierGaussianBlur
    {
        // ============================================================
        // 0. FFT Shift (기존 방식)
        // ============================================================
        public static Mat ShiftDFT(Mat mat)
        {
            mat = mat.Clone();
            int cx = mat.Cols / 2;
            int cy = mat.Rows / 2;

            Mat q0 = new Mat(mat, new Rect(0, 0, cx, cy));
            Mat q1 = new Mat(mat, new Rect(cx, 0, cx, cy));
            Mat q2 = new Mat(mat, new Rect(0, cy, cx, cy));
            Mat q3 = new Mat(mat, new Rect(cx, cy, cx, cy));

            Mat tmp = new Mat();
            q0.CopyTo(tmp); q3.CopyTo(q0); tmp.CopyTo(q3);
            q1.CopyTo(tmp); q2.CopyTo(q1); tmp.CopyTo(q2);

            return mat;
        }

        // ============================================================
        // 1. 기존 Low-pass Gaussian (BlurChannel에서 사용)
        // ============================================================
        public static Mat CreateGaussianFilter(Size size, double sigma)
        {
            Mat filter = new Mat(size, MatType.CV_32FC2);

            int cx = size.Width / 2;
            int cy = size.Height / 2;

            for (int y = 0; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++)
                {
                    double dx = x - cx;
                    double dy = y - cy;
                    float v = (float)Math.Exp(-(dx * dx + dy * dy) / (2 * sigma * sigma));

                    filter.Set(y, x, new Vec2f(v, 0));
                }
            }

            return filter;
        }

        // ============================================================
        // 2. 기존 LPF 방식 BlurChannel (강한 흐림이 특징)
        // ============================================================
        public static Mat BlurChannel(Mat channel, int kernelSize)
        {
            double sigma = (kernelSize - 1) / 6.0;

            int optRows = Cv2.GetOptimalDFTSize(channel.Rows * 2);
            int optCols = Cv2.GetOptimalDFTSize(channel.Cols * 2);

            Mat padded = new Mat();
            Cv2.CopyMakeBorder(channel, padded,
                0, optRows - channel.Rows,
                0, optCols - channel.Cols,
                BorderTypes.Constant, Scalar.All(0));

            Mat floatImg = new Mat();
            padded.ConvertTo(floatImg, MatType.CV_32F);

            Mat[] planes =
            {
                floatImg,
                new Mat(padded.Rows, padded.Cols, MatType.CV_32F, Scalar.All(0))
            };

            Mat complex = new Mat();
            Cv2.Merge(planes, complex);

            Cv2.Dft(complex, complex);
            complex = ShiftDFT(complex);

            Mat filter = CreateGaussianFilter(complex.Size(), sigma);

            Mat filtered = new Mat();
            Cv2.MulSpectrums(complex, filter, filtered, 0);

            filtered = ShiftDFT(filtered);

            Mat idft = new Mat();
            Cv2.Dft(filtered, idft, DftFlags.Inverse | DftFlags.RealOutput | DftFlags.Scale);

            Mat cropped = new Mat(idft, new Rect(0, 0, channel.Cols, channel.Rows));
            cropped.ConvertTo(cropped, MatType.CV_8U);

            return cropped;
        }

        public static Mat ApplyGaussianFourierGray(Mat input, int kernelSize)
        {
            Mat gray = (input.Channels() == 1)
                ? input.Clone()
                : input.CvtColor(ColorConversionCodes.BGR2GRAY);

            return BlurChannel(gray, kernelSize);
        }

        public static Mat ApplyGaussianFourierColor(Mat input, int kernelSize)
        {
            Mat[] c = Cv2.Split(input);

            c[0] = BlurChannel(c[0], kernelSize);
            c[1] = BlurChannel(c[1], kernelSize);
            c[2] = BlurChannel(c[2], kernelSize);

            Mat merged = new Mat();
            Cv2.Merge(c, merged);
            return merged;
        }

        // ============================================================
        // 3. 정확한 GaussianBlurFFT (정석 FFT Gaussian blur)
        // 위치 조정
        // ============================================================
        private static Mat GaussianBlurFFT_Gray(Mat gray, int kernelSize)
        {
            double sigma = (kernelSize - 1) / 6.0;

            Mat f32 = new Mat();
            gray.ConvertTo(f32, MatType.CV_32F);

            Mat[] planes =
            {
        f32,
        new Mat(gray.Rows, gray.Cols, MatType.CV_32F, Scalar.All(0))
    };
            Mat complexSrc = new Mat();
            Cv2.Merge(planes, complexSrc);
            Cv2.Dft(complexSrc, complexSrc);

            // 1) 공간 도메인 Gaussian kernel (2D)
            Mat kernel1D = Cv2.GetGaussianKernel(gray.Rows, sigma, MatType.CV_32F);
            Mat kernel2D = kernel1D * kernel1D.T();  // outer product

            // 2) zero-padding to full image size
            Mat kernelPadded = new Mat();
            Cv2.CopyMakeBorder(kernel2D, kernelPadded,
                0, gray.Rows - kernel2D.Rows,
                0, gray.Cols - kernel2D.Cols,
                BorderTypes.Constant, Scalar.All(0));

            // 3) 커널을 중앙 기준으로 shift 
            kernelPadded = ShiftDFT(kernelPadded);

            // 4) FFT(kernel)
            Mat[] kPlanes =
            {
        kernelPadded,
        new Mat(gray.Rows, gray.Cols, MatType.CV_32F, Scalar.All(0))
    };
            Mat complexKernel = new Mat();
            Cv2.Merge(kPlanes, complexKernel);
            Cv2.Dft(complexKernel, complexKernel);

            // 5) Multiply
            Mat filtered = new Mat();
            Cv2.MulSpectrums(complexSrc, complexKernel, filtered, 0);

            // 6) Inverse FFT
            Mat idft = new Mat();
            Cv2.Dft(filtered, idft, DftFlags.Inverse | DftFlags.RealOutput | DftFlags.Scale);

            Mat result = new Mat();
            idft.ConvertTo(result, MatType.CV_8U);

            return result;
        }


        public static Mat GaussianBlurFFT(Mat input, int kernelSize)
        {
            if (input.Channels() == 1)
                return GaussianBlurFFT_Gray(input, kernelSize);

            Mat[] ch = Cv2.Split(input);

            for (int i = 0; i < 3; i++)
                ch[i] = GaussianBlurFFT_Gray(ch[i], kernelSize);

            Mat merged = new Mat();
            Cv2.Merge(ch, merged);
            return merged;
        }

    }
}
