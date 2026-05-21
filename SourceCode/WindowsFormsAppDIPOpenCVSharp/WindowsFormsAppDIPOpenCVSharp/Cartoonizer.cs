using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace WindowsFormsAppDIPOpenCVSharp
{
    internal class Cartoonizer
    {
        public static Mat Cartoonify(Mat src)
        {
            Mat imgColor = src.Clone();

            // 1) Edge Mask 생성
            Mat imgGray = new Mat();
            Cv2.CvtColor(src, imgGray, ColorConversionCodes.BGR2GRAY);

            Mat imgBlur = new Mat();
            Cv2.MedianBlur(imgGray, imgBlur, 7);

            Mat imgEdge = new Mat();
            Cv2.AdaptiveThreshold(imgBlur, imgEdge, 255,
                AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 9, 2);

            // 2) Bilateral Filter 반복 적용 (src != dst 반드시 분리!)
            Mat imgColorFiltered = imgColor.Clone();

            for (int i = 0; i < 5; i++)
            {
                Mat temp = new Mat();
                Cv2.BilateralFilter(imgColorFiltered, // src
                    temp,                // dst (src와 분리!)
                    d: 9,
                    sigmaColor: 75, sigmaSpace: 75);
                imgColorFiltered = temp.Clone();
            }

            // 3) Edge Mask + ColorFiltered 합성
            Mat imgEdgeColor = new Mat();
            Cv2.CvtColor(imgEdge, imgEdgeColor, ColorConversionCodes.GRAY2BGR);

            Mat cartoon = new Mat();
            Cv2.BitwiseAnd(imgColorFiltered, imgEdgeColor, cartoon);

            return cartoon;
        }

    }
}
