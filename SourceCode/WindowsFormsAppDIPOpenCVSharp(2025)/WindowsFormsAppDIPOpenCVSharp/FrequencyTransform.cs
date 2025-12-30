using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace WindowsFormsAppDIPOpenCVSharp
{
    internal class FrequencyTransform
    {
        public static Mat ComputeDFT(Mat input)
        {
            // 1. 그레이스케일 변환
            Mat gray = new Mat();
            Cv2.CvtColor(input, gray, ColorConversionCodes.BGR2GRAY);

            // 2. float 형태로 변환
            Mat floatImage = new Mat();
            gray.ConvertTo(floatImage, MatType.CV_32F);

            // 3. DFT 수행하기 위해 2채널(Mat) 생성 (실수, 허수)
            Mat complexImage = new Mat();
            Mat[] planes = { floatImage, Mat.Zeros(gray.Size(), MatType.CV_32F) };
            Cv2.Merge(planes, complexImage);

            // 4. DFT 수행
            Cv2.Dft(complexImage, complexImage);

            return complexImage;
        }

        public static Mat GetMagnitudeSpectrum(Mat complexImage)
        {
            // 1. 실수/허수 분리
            Mat[] planes = new Mat[2];
            Cv2.Split(complexImage, out planes);
            Mat real = planes[0];
            Mat imag = planes[1];

            // 2. magnitude = sqrt(Re² + Im²)
            Mat magnitude = new Mat();
            Cv2.Magnitude(real, imag, magnitude);

            // 3. 로그 스케일 변환
            //Cv2.Add(magnitude + 1, magnitude);
            // '+'연산자는 'Mat' 및 'int' 형식의 피연산자에 적용할 수 없습니다
            // magnitude = magnitude + 1
            Cv2.Add(magnitude, new Scalar(1.0), magnitude);
            Cv2.Log(magnitude, magnitude);

            // 4. 사분면 이동 (중앙을 기준 주파수로)
            magnitude = ShiftDFTQuadrants(magnitude);

            // 5. 정규화 (0~255)
            Cv2.Normalize(magnitude, magnitude, 0, 255, NormTypes.MinMax);
            magnitude.ConvertTo(magnitude, MatType.CV_8U);

            return magnitude;
        }

        // DFT 결과를 시각화를 위해 사분면 이동
        public static Mat ShiftDFTQuadrants(Mat mag)
        {
            int cx = mag.Cols / 2;
            int cy = mag.Rows / 2;

            Mat q0 = new Mat(mag, new Rect(0, 0, cx, cy));       // Top-Left
            Mat q1 = new Mat(mag, new Rect(cx, 0, cx, cy));      // Top-Right
            Mat q2 = new Mat(mag, new Rect(0, cy, cx, cy));      // Bottom-Left
            Mat q3 = new Mat(mag, new Rect(cx, cy, cx, cy));     // Bottom-Right

            Mat tmp = new Mat();

            // q0 ↔ q3
            q0.CopyTo(tmp);
            q3.CopyTo(q0);
            tmp.CopyTo(q3);

            // q1 ↔ q2
            q1.CopyTo(tmp);
            q2.CopyTo(q1);
            tmp.CopyTo(q2);

            return mag;
        }

        public static Mat ComputeIDFT(Mat complexImage, Size originalSize)
        {
            // 1. 역 DFT 수행
            Mat inverse = new Mat();
            Cv2.Idft(complexImage, inverse, DftFlags.Scale | DftFlags.RealOutput);

            // 2. 결과 분리 (실수 부분만 사용)
            Mat[] planes = new Mat[2];
            Cv2.Split(inverse, out planes);
            Mat reconstructed = planes[0];

            // 3. 크기를 원본과 동일하게 조정
            Mat result = new Mat();
            Cv2.Resize(reconstructed, result, originalSize);

            // 4. 0-255 범위로 정규화
            Cv2.Normalize(result, result, 0, 255, NormTypes.MinMax);
            result.ConvertTo(result, MatType.CV_8U);

            return result;
        }
    }
}
