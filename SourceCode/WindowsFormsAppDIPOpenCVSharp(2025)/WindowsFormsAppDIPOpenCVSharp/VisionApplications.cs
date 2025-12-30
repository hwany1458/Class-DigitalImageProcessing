using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAppDIPOpenCVSharp
{
    internal class VisionApplications
    {
        public static Mat FaceDetection(Mat src)
        {
            // Haar Cascade 로드
            string cascadePath = "haarcascade_frontalface_default.xml";
            CascadeClassifier faceCascade = new CascadeClassifier(cascadePath);

            // 원본 복사
            Mat result = src.Clone();

            // 1) Grayscale 변환
            Mat gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            // 2) 얼굴 탐지
            Rect[] faces = faceCascade.DetectMultiScale(
                gray,
                scaleFactor: 1.1,
                minNeighbors: 5,
                //flags: HaarDetectionType.ScaleImage,
                flags: 0,
                minSize: new OpenCvSharp.Size(60, 60)
            );

            // 3) 얼굴에 사각형 그리기
            foreach (var face in faces)
            {
                Cv2.Rectangle(result, face, Scalar.Red, 3);
            }

            return result;
        }
    }
}
