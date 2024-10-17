using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace WinFormsAppDIPusingCSharp1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        // 변수 선언 (전역변수) --------------
        static byte[,] inImage = null, outImage = null;
        static int inHeight=0, inWidth=0, outHeight=0, outWidth=0;
        static string fileName;
        static Bitmap bitmap;
        const int maxValue = 255;
        const int minValue = 0;
        static int intensityLevelSlicingValue = 255;
        double gammaCorrectionConstantValue = 255.0;
        int logTransformConstantValue = 1;

        bool flag;
        double[,] inTempImage, outTempImage;

        // 메뉴 [점처리 - 반전] 클릭
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            // 이미지 반전 함수 호출
            ReverseImage();
        }

        // 메뉴 [파일 - 열기] 클릭
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            /*
            // ofd : 파일을 담는 변수
            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.ShowDialog();
            // 오류 체크하는 습관을 갖도록
            if (ofd.ShowDialog() == DialogResult.Cancel) { return; }
            fileName = ofd.FileName;
            
            long fileSize = new FileInfo(fileName).Length;  // 파일크기
            // 입력 이미지의 가로, 세로 크기 (메모리 크기를 잡아줘야 하기 때문에 중요)
            inHeight = inWidth = (int)Math.Sqrt((double)fileSize);

            // 메모리 할당 (입력 메모리 확보)
            inImage = new byte[inHeight, inWidth];
            // 파일 --> 입력 메모리 로딩
            BinaryReader br = new BinaryReader(File.Open(fileName, FileMode.Open));
            for (int i=0; i<inHeight; i++) { 
                for (int j=0; j<inWidth; j++) {
                    inImage[i, j] = br.ReadByte();
                }
            }
            br.Close();
            */
            OpenImage();
            //EqualImage();
        }

        // --- User defined functions ---

        void OpenImage()
        {
            // ofd : 파일을 담는 변수
            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.ShowDialog();
            // 오류 체크하는 습관을 갖도록
            if (ofd.ShowDialog() == DialogResult.OK) { 
                fileName = ofd.FileName;

                long fileSize = new FileInfo(fileName).Length;  // 파일크기
                // 입력 이미지의 가로, 세로 크기 (메모리 크기를 잡아줘야 하기 때문에 중요)
                inHeight = inWidth = (int)Math.Sqrt((double)fileSize);

                // 메모리 할당 (입력 메모리 확보)
                inImage = new byte[inHeight, inWidth];
                // 파일 --> 입력 메모리 로딩
                BinaryReader br = new BinaryReader(File.Open(fileName, FileMode.Open));
                for (int i = 0; i < inHeight; i++) {
                    for (int j = 0; j < inWidth; j++) {
                        inImage[i, j] = br.ReadByte();
                    }
                }
                br.Close();

                // 오픈한 이미지 파일을 화면에 표시
                EqualImage();
            }
        }

        void DisplayImage()
        {
            // 비트맵 및 픽처박스 크기 조절
            bitmap = new Bitmap(outHeight, outWidth);
            PB_OutImage.Size = new Size(outHeight, outWidth);
            this.Size = new Size(outHeight + 40, outHeight + 90);

            Color pen;  // 비트맵에 찍을 펜
            for (int i=0; i<outHeight; i++) {
                for (int j=0; j<outWidth; j++) {
                    byte data = outImage[i, j];  // 한점(=색상)
                    pen = Color.FromArgb(data, data, data);  // 펜에 잉크 묻힘
                    bitmap.SetPixel(j, i, pen);  // (콕콕 한점씩) 점찍기
                }
            }
            PB_OutImage.Image = bitmap;
        }

        // 메뉴 [점처리 - 밝게 어둡게] 클릭
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            BrightDark();
        }

        void EqualImage()
        {
            // 출력 이미지 크기
            outHeight = inHeight;
            outWidth = inWidth;
            // 출력 이미지 메모리 할당
            outImage = new byte[outHeight, outWidth];

            // 영상처리 알고리즘 *****
            for (int i = 0; i < inHeight; i++) {
                for (int j = 0; j < inWidth; j++) {
                    outImage[i, j] = inImage[i, j];
                }
            }

            DisplayImage();
        }

        // 메뉴 [점처리 - 범위강조] 클릭
        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            IntensityLevelSlicing();
        }

        // 이미지 반전
        void ReverseImage()
        {
            // 이미지가 열려있지 않으면, 영상처리 불가
            if(inImage == null) { return; }
            
            // 출력 (결과) 이미지의 크기를 결정
            outHeight = inHeight;
            outWidth = inWidth;

            // 출력 이미지 메모리 할당
            outImage = new byte[outHeight, outWidth];

            // 영상처리 알고리즘 -- 이미지 반전
            for (int i=0; i<inHeight; i++) {
                for (int j=0; j<inWidth; j++) {
                    outImage[i, j] = (byte)(255 - inImage[i, j]);
                }
            }
            DisplayImage();
        }

        // 문턱치 (Thresholding) 처리
        void ThresholdingImage()
        {

        }

       
        // 사용자 입력폼 (값 1개 받음)
        float getValue1()
        {
            // 서브 윈도우 폼 준비
            Input1Form sub = new Input1Form();
            float inputValue;

            if (sub.ShowDialog() == DialogResult.Cancel)
            {
                // 화면에 띄우기(ShowDialog는 뜬 창만 클릭이 가능하고 다른 창은 클릭 불가
                inputValue = 0.0f;
            }
            else
            {
                // input1Form 에 있는 numericalUpDown의 값을 가져오기 위해
                inputValue = (float)(sub.numericUpDown1.Value);
            }

            return inputValue;
        }

        // 메뉴 [점처리 - 기하학 - 좌우반전]
        private void toolStripMenuItem21_Click(object sender, EventArgs e)
        {
            LeftRightImage();
        }

        // 사용자 입력폼 (값 2개 받음)
        Tuple<int, int> getValue2()
        {
            Input2Form sub = new Input2Form();  // 서브 폼
            int value1, value2;
            if (sub.ShowDialog() == DialogResult.Cancel)
            {
                value1 = 0;
                value2 = 0;
            }
            else
            {
                value1 = (int)(sub.numericUpDown1.Value);
                value2 = (int)(sub.numericUpDown2.Value);
            }

            return new Tuple<int, int>(value1, value2);
        }

        // 밝게, 어둡게 처리
        void BrightDark()
        {
            // 이미지가 열려있지 않으면, 영상처리 불가
            if (inImage == null) { return; }

            // 출력 (결과) 이미지의 크기를 결정
            outHeight = inHeight;
            outWidth = inWidth;

            // 출력 이미지 메모리 할당
            outImage = new byte[outHeight, outWidth];

            // 영상처리 알고리즘 -- 이미지를 밝게 또는 어둡게
            int value = (int)getValue1();
            for (int i = 0; i < inHeight; i++) {
                for (int j = 0; j < inWidth; j++) {
                    int pixelValue = inImage[i, j] + value;
                    if (pixelValue > maxValue) { pixelValue = maxValue; }
                    else if (pixelValue < minValue) { pixelValue = minValue;  }

                    outImage[i, j] = (byte)pixelValue;
                }
            }

            DisplayImage();
        }

        // 범위강조 (밝기 레벨 슬라이싱)
        void IntensityLevelSlicing()
        {
            // 이미지가 열려있지 않으면, 영상처리 불가
            if (inImage == null) { return; }

            // 출력 (결과) 이미지의 크기를 결정
            outHeight = inHeight;
            outWidth = inWidth;

            // 출력 이미지 메모리 할당
            outImage = new byte[outHeight, outWidth];

            // 영상처리 알고리즘 -- 범위 강조 (밝기 레벨 슬라이싱)
            var rv = getValue2();
            for (int i = 0; i < inHeight; i++)
            {
                for (int j = 0; j < inWidth; j++)
                {
                    int pixelValue = inImage[i, j];
                    if (rv.Item1 < pixelValue && pixelValue < rv.Item2) { 
                        outImage[i, j] = (byte)intensityLevelSlicingValue; 
                    }
                    else { outImage[i, j] = (byte)pixelValue; }
                }
            }

            DisplayImage();
        }

        // 로그 변환 이미지
        void LogTransformImage()
        {

        }

        // 메뉴 [점처리  - 감마보정] 클릭
        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            GammaCorrectionImage();
        }

        // 감마 변환 이미지
        void GammaCorrectionImage()
        {
            if (inImage == null) { return; }

            outHeight = inHeight;
            outWidth = inWidth;
            outImage = new byte[outHeight, outWidth];
            double temp = 0.0;

            // 영상처리 알고리즘 -- 감마보정
            float gamma = (float)getValue1();
            //MessageBox.Show("입력값 =" + gamma);

            for (int i = 0; i < inHeight; i++)
            {
                for (int j = 0; j < inWidth; j++)
                {
                    temp = gammaCorrectionConstantValue * Math.Pow(((double)inImage[i,j]/(double)maxValue), gamma);
                    if (temp > maxValue) { outImage[i, j] = (byte)maxValue; }
                    else if (temp < minValue) { outImage[i, j] = (byte)minValue; }
                    else { outImage[i, j] = (byte)temp; } 
                }
            }

            DisplayImage();
        }

        // 이미지 축소
        void ZoomInImage()
        {
            if (inImage == null) { return; }

            int scale = 2; // 2배 축소
            outHeight = inHeight / scale;
            outWidth = inWidth / scale;
            outImage = new byte[outHeight, outWidth];

            for (int i = 0; i < inHeight; i++)
            {
                for (int j = 0; j < inWidth; j++)
                {
                    outImage[i / scale, j / scale] = inImage[i, j];
                }
            }

            DisplayImage();
        }

        // 메뉴 [점처리 - 기하학 - 축소] 클릭
        private void toolStripMenuItem20_Click(object sender, EventArgs e)
        {
            ZoomInImage();
        }


        // 이미지 확대
        void ZoomOutImage()
        {

        }

        // 메뉴 [점처리 - 기하학 - 회전(시계방향)] 클릭 --- 원점(0,0)을 기준으로, 시계방향으로 회전
        private void toolStripMenuItem24_Click(object sender, EventArgs e)
        {
            OriginClockwiseRotationImage();
        }

        // 메뉴 [점처리 - 기하학 - 중앙기준회전(시계방향)] 클릭 --- 이미지 센터를 기준으로, 시계방향으로 회전
        private void toolStripMenuItem25_Click(object sender, EventArgs e)
        {
            CenterClockwiseRotationImage();
        }

        private void toolStripMenuItem26_Click(object sender, EventArgs e)
        {

        }

        // 좌우반전
        void LeftRightImage()
        {
            if (inImage == null) { return; }

            outHeight = inHeight;
            outWidth = inWidth;
            outImage = new byte[outHeight, outWidth];

            // 영상처리 알고리즘 -- 좌우반전
            for (int i = 0; i < inHeight; i++)
            {
                for (int j = 0; j < inWidth; j++)
                {
                    outImage[i, j] = inImage[i, (outWidth - 1 - j)];
                }
            }

            DisplayImage();
        }

        // 상하반전
        void UpDownImage()
        {

        }

        // 상하좌우반전
        void UpDownLeftRightImage()
        {

        }

        // 원점 기준으로, 시계방향 회전
        void OriginClockwiseRotationImage()
        {
            if (inImage == null) { return; }

            outHeight = inHeight;
            outWidth = inWidth;
            outImage = new byte[outHeight, outWidth];

            int degree = (int)getValue1();
            double radian = ((double)degree * Math.PI) / 180.0;  // 각도 값을 받아서 라디안(radian)으로 변환

            // 영상처리 알고리즘 -- 원점(0,0) 기준으로, 시계방향으로 이미지 회전
            for (int i = 0; i < inHeight; i++)
            {
                for (int j = 0; j < inWidth; j++)
                {
                    int xd = (int)(Math.Cos(radian) * i - Math.Sin(radian) * j);
                    int yd = (int)(Math.Sin(radian) * i + Math.Cos(radian) * j);
                    // xd, yd 가 outImage 범위 안에 있는 값만 골라내고 벗어나면 버림
                    if ((0 <= xd && xd < outHeight) && (0 <= yd && yd < outWidth))
                    {
                        outImage[i, j] = inImage[xd, yd];
                    }
                }
            }

            DisplayImage();
        }

        // 이미지 중앙점을 기준으로, 시계방향 회전
        void CenterClockwiseRotationImage()
        {
            
        }

    }
}
