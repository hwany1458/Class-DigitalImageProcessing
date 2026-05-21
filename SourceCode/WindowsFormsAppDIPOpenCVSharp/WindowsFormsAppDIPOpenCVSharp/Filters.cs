using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace WindowsFormsAppDIPOpenCVSharp
{
    internal class Filters
    {
        public static Mat ApplySepiaFilter(Mat src)
        {
            // RGB 이미지를 float 타입으로 변환
            Mat srcFloat = new Mat();
            src.ConvertTo(srcFloat, MatType.CV_32F);

            // 세피아 변환 매트릭스
            float[,] sepiaKernelData = new float[,] {
                { 0.272f, 0.534f, 0.131f },
                { 0.349f, 0.686f, 0.168f },
                { 0.393f, 0.769f, 0.189f }
   };
            Mat sepiaKernel = Mat.FromArray(sepiaKernelData);

            // 변환 수행
            Mat appliedResult = new Mat();
            Cv2.Transform(srcFloat, appliedResult, sepiaKernel);

            // float 이미지를 8-비트로 변환
            appliedResult.ConvertTo(appliedResult, MatType.CV_8U);
            return appliedResult;
        }


        public static Mat ApplySepiaFilterEnhanced(Mat src)
        {
            // 1) float 변환
            Mat srcFloat = new Mat();
            src.ConvertTo(srcFloat, MatType.CV_32F);

            // 2) 세피아 변환 매트릭스
            float[,] sepiaKernelData = {
        { 0.272f, 0.534f, 0.131f },
        { 0.349f, 0.686f, 0.168f },
        { 0.393f, 0.769f, 0.189f }
    };

            Mat sepiaKernel = Mat.FromArray(sepiaKernelData);

            Mat sepia = new Mat();
            Cv2.Transform(srcFloat, sepia, sepiaKernel);

            // 3) 감마 조정 (약간 밝게)
            double gamma = 0.85;
            sepia = GammaCorrection(sepia, gamma);

            // 4) 대비 감소 (조금 부드럽게)
            double alpha = 0.90;  // contrast scale (<1이면 대비 감소)
            double beta = 0;      // brightness shift
            Cv2.ConvertScaleAbs(sepia, sepia, alpha, beta);

            // 5) 채도(Saturation) 약간 감소
            Mat hsv = new Mat();
            Cv2.CvtColor(sepia, hsv, ColorConversionCodes.BGR2HSV);
            Mat[] hsvSplit = Cv2.Split(hsv);

            hsvSplit[1] *= 0.85;   // Saturation 85% 유지

            Cv2.Merge(hsvSplit, hsv);
            Cv2.CvtColor(hsv, sepia, ColorConversionCodes.HSV2BGR);

            // 6) float → 8bit 변환
            sepia.ConvertTo(sepia, MatType.CV_8UC3);

            return sepia;
        }


        // ============================
        /// 감마 보정 함수 (OpenCvSharp 버전)
        /// ============================
        static Mat GammaCorrection(Mat src, double gamma)
        {
            // 1) 먼저 8bit로 변환 (LUT는 float 입력을 처리할 수 없음)
            Mat src8 = new Mat();
            src.ConvertTo(src8, MatType.CV_8UC3);

            // 2) LUT 생성
            Mat lut = new Mat(1, 256, MatType.CV_8UC1);
            for (int i = 0; i < 256; i++)
            {
                lut.Set(i, 0, (byte)(Math.Pow(i / 255.0, gamma) * 255.0));
            }

            // 3) LUT 적용
            Mat dst = new Mat();
            Cv2.LUT(src8, lut, dst);

            return dst;
        }
    }
}
