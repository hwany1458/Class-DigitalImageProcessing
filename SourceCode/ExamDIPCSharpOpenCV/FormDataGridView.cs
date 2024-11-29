using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenCvSharp;
using System.IO;
using System.Windows.Media.Imaging;
using ExifPhotoReader;

namespace ExamDIPCSharpOpenCV
{
    public partial class FormDataGridView : Form
    {
        public FormDataGridView()
        {
            InitializeComponent();
        }

        // 이미지 파일이름을 매개변수로 넘겨 받음
        public FormDataGridView(string fileName)
        {
            InitializeComponent();

            // -----
            labelImageFileName.Text = fileName;

            FileInfo fileInfo = new FileInfo(fileName);
            DataTable dt = GetImageMetaData(fileInfo);
            // dataGridView1 = 윈도우 폼에 위치한 객체 이름
            ShowDataFromDataTable2DataGridView(dt, dataGridView1);
        }

        // 이미지 파일이름, 읽어들인 이미지 Mat을 매개변수로 넘겨 받음
        public FormDataGridView(string fileName, Mat inputImage)
        {
            InitializeComponent();

            // -----
            labelImageFileName.Text = fileName;

            FileInfo fileInfo = new FileInfo(fileName);
            DataTable dti = GetImageInfo(fileName, fileInfo, inputImage);

            ShowDataFromDataTable2DataGridView(dti, dataGridView1);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ------------ User defined method

        // 파일 이름을 매개변수로 넘겨 받음
        private DataTable GetImageInfo(FileInfo f)
        {
            // 선택한 파일(넘어온 파일이름)의 파일스트림을 생성
            //FileStream fs = new FileStream(f.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);

            // 새로운 데이터테이블을 생성
            DataTable ii = new DataTable();
            ii.Columns.Add("attribute");   // 속성 이름을 저장
            ii.Columns.Add("value");  // 속성 값을 저장

            //ii.Rows.Add("Name", f.Name);
            ii.Rows.Add("Name", f.Name.Replace(f.Extension, ""));
            ii.Rows.Add("Extension", f.Extension);
            ii.Rows.Add("Location", f.DirectoryName);
            ii.Rows.Add("Size", (f.Length / 1024.0).ToString("0.0") + "KB");
            ii.Rows.Add("Created On", f.CreationTime.ToString("yyyy MM dd, ddd"));

            return ii;
        }

        // (이미지) 파일이름, 파일(속성)정보, (읽어들인) 이미지 Mat을 매개변수로 넘겨 받음
        private DataTable GetImageInfo(string fileName, FileInfo fileInfo, Mat inputImage)
        {
            // 새로운 데이터테이블을 생성
            DataTable ii = new DataTable();
            ii.Columns.Add("attribute");   // 속성 이름을 저장
            ii.Columns.Add("value");  // 속성 값을 저장

            string strExtension = fileInfo.Extension;

            //ii.Rows.Add("Name", fileInfo.Name);
            ii.Rows.Add("Name", fileInfo.Name.Replace(fileInfo.Extension, ""));
            //ii.Rows.Add("Extension", fileInfo.Extension);
            ii.Rows.Add("Extension", strExtension);
            ii.Rows.Add("Location", fileInfo.DirectoryName);
            ii.Rows.Add("Size", (fileInfo.Length / 1024.0).ToString("0.0") + " KB");
            ii.Rows.Add("Created On", fileInfo.CreationTime.ToString("yyyy MM dd, ddd"));

            ii.Rows.Add("Dimension", inputImage.Rows + " X " + inputImage.Cols);
            ii.Rows.Add("Channel Count", inputImage.Channels());

            if (strExtension.ToUpper() == ".JPG" || strExtension.ToUpper() == ".JPEG")
            {
                ExifImageProperties exif = ExifPhoto.GetExifDataPhoto(fileName);

                ii.Rows.Add("GPS", exif.GPSInfo.Latitude + "," + exif.GPSInfo.Longitude);
                ii.Rows.Add("Camera Model", exif.Model);
                ii.Rows.Add("Manufacturer", exif.Make);
                ii.Rows.Add("Resolution", exif.XResolution + " " + exif.YResolution);
                ii.Rows.Add("Size", exif.ExifImageWidth + "," + exif.ExifImageHeight);
                ii.Rows.Add("ColorSpace", exif.ColorSpace);
            }

            return ii;
        }

        /// <summary>
        /// 선택한 이미지 파일의 메타데이터를 불러옴
        /// </summary>
        /// <param name="f">이미지 정보를 가져올 파일의 FileInfo를 매개변수로 전달 받음</param>
        /// <returns>이미지 정보를 DataTable 형식으로 반환</returns>
        private DataTable GetImageMetaData(FileInfo f)
        {
            // 선택한 파일(넘어온 파일이름)의 파일스트림을 생성
            FileStream fs = new FileStream(f.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);

            // 새로운 데이터테이블을 생성
            DataTable dt = new DataTable();
            dt.Columns.Add("attribute");   // 속성 이름을 저장
            dt.Columns.Add("value");  // 속성 값을 저장

            // 선택한 파일의 파일스트림을 받아서 비트맵소스를 생성
            BitmapSource img = BitmapFrame.Create(fs);
            BitmapMetadata md = (BitmapMetadata)img.Metadata;

            // BitmapMetadata에서 제공하는 속성 값을 가져옴
            // https://learn.microsoft.com/ko-kr/dotnet/api/system.windows.media.imaging.bitmapmetadata?view=windowsdesktop-8.0&redirectedfrom=MSDN

            dt.Rows.Add("ApplicationName", md.ApplicationName);
            dt.Rows.Add("Author", md.Author);
            dt.Rows.Add("CameraManufacturer", md.CameraManufacturer);
            dt.Rows.Add("CameraModel", md.CameraModel);
            dt.Rows.Add("CanFreeze", md.CanFreeze);
            dt.Rows.Add("Comment", md.Comment);
            dt.Rows.Add("Copyright", md.Copyright);
            dt.Rows.Add("DateTaken", md.DateTaken);
            dt.Rows.Add("DependencyObjectType", md.DependencyObjectType);
            dt.Rows.Add("Dispatcher", md.Dispatcher);
            dt.Rows.Add("Format", md.Format);
            dt.Rows.Add("IsFixedSize", md.IsFixedSize);
            dt.Rows.Add("IsFrozen", md.IsFrozen);
            dt.Rows.Add("IsReadOnly", md.IsReadOnly);
            dt.Rows.Add("IsSealed", md.IsSealed);
            dt.Rows.Add("Keywords", md.Keywords);
            dt.Rows.Add("Location", md.Location);
            dt.Rows.Add("Rating", md.Rating);
            dt.Rows.Add("Subject", md.Subject);
            dt.Rows.Add("Title", md.Title);

            return dt;
        }

        /// <summary>
        ///  선택한 폴더의 파일 목록ㅇ르 가져와서 DataGridView 객체에 뿌려줌
        /// </summary>
        /// <param name="dt">선택한 폴더의 파일 목록이 들어있는 데이터테이블을 매개변수로 넘겨받음</param>
        /// <param name="dgv">결과를 출력한 데이터그리드뷰를 매개변수로 넘겨받음</param>
        private void ShowDataFromDataTable2DataGridView(DataTable dt, DataGridView dgv)
        {
            dgv.Rows.Clear();  // 이전 정보가 있을 경우, 모든 행을 삭제
            dgv.Columns.Clear(); // 이전 정보가 있을 경우, 모든 열을 삭제

            // 선택한 파일 목록이 들어있는 DataTable의 모든 열을 스캔
            foreach (DataColumn dc in dt.Columns) 
            {
                // 출력할 DataGridView에 열을 추가
                dgv.Columns.Add(dc.ColumnName, dc.ColumnName); 
            }

            // 행 인덱스 번호(초기 값)
            int row_index = 0;
            
            // 선택한 파일 목록이 들어있는 DataTable의 모든 행을 스캔
            foreach (DataRow dr in dt.Rows) 
            {
                // 빈 행을 하나 추가
                dgv.Rows.Add();

                // 선택한 파일 목록이 들어있는 DataTable의 모든 열을 스캔
                foreach (DataColumn dc in dt.Columns) 
                {
                    // 선택 행 별로, 스캔하는 열에 해당하는 셀 값을 입력
                    dgv.Rows[row_index].Cells[dc.ColumnName].Value = dr[dc.ColumnName]; 
                }
                // 다음 행 인덱스를 선택하기 위해 1을 더해줌
                row_index++; 
            }

            // 결과를 출력할 DataGridView의 모든 열을 스캔
            foreach (DataGridViewColumn drvc in dgv.Columns) 
            {
                // 선택 열의 너비를 자동으로 설정합니다.
                drvc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; 
            }
        }
    }
}
