#include <stdio.h>
#include <stdlib.h>

// BMP헤더를 받기 위한 구조체(struct)를 선언할 경우
// 수업시간에는 구조체 선언하지 않고 Windows.h를 include시켜서 작성함 
// (ImageReadWriteV2.cpp 참조)

#define WORD    unsigned short       //2byte
#define DWORD   unsigned int         //4byte
#define LONG    unsigned int         //4byte
#define BYTE    unsigned char        //1byte

//BITMAPFILEHEADER Structure
typedef struct tagBITMAPFILEHEADER {  // 비트맵 파일 헤더 구조체
	WORD  bfType;      // Specifies the file type 비트맵파일 여부 확인 (e.g., 'BM' for BMP)
	DWORD bfSize;      // Specifies the size of the file in bytes 파일의 크기(바이트)
	WORD  bfReserved1; // Reserved; must be 0 미래를 위한 예약된 공간
	WORD  bfReserved2; // Reserved; must be 0 미래를 위한 예약된 공간
	DWORD bfOffBits;   // Specifies the offset to the bitmap data (pixel array) 비트맵 데이타 시작 위치
} BITMAPFILEHEADER;

// BITMAPINFOHEADER Structure
typedef struct tagBITMAPINFOHEADER {  // 비트맵 정보 헤더 구조체
	DWORD biSize;          // Specifies the number of bytes required by the struct 현재 비트맵 정보 헤더의 크기
	LONG  biWidth;         // Specifies the width of the bitmap in pixels 이미지 가로 크기 (픽셀)
	LONG  biHeight;        // Specifies the height of the bitmap in pixels 이미지 세로 크기 (픽셀)
	WORD  biPlanes;        // Specifies the number of color planes, must be 1 사용하는 색상판의 수(항상 1)
	WORD  biBitCount;      // Specifies the number of bits per pixel (color depth) 픽셀 1개를 표현하는 비트 수
	DWORD biCompression;   // Specifies the type of compression (e.g., BI_RGB for no compression) 압축 방식, 보통 비트맵은 압축하지 않아서 0
	DWORD biSizeImage;     // Specifies the size of the image data in bytes 이미지 픽셀 데이터 크기
	LONG  biXPelsPerMeter; // Specifies the horizontal resolution in pixels per meter 이미지 가로 해상도
	LONG  biYPelsPerMeter; // Specifies the vertical resolution in pixels per meter 이미지 세로 해상도
	DWORD biClrUsed;       // Specifies the number of colors used by the bitmap 색상 테이블에서 실제 사용되는 색상 수
	DWORD biClrImportant;  // Specifies the number of important colors 비트맵을 표현하기 위해 필요한 색상 인덱스 수
} BITMAPINFOHEADER;

typedef struct tagRGBQUAD {  // 24bit 비트맵의 픽셀 구조체
	BYTE rgbBlue;     // 파란색 성분	
	BYTE rgbGreen;    // 녹색 성분
	BYTE rgbRed;      // 빨간색 성분
	BYTE rgbeserved;  // 확장을 위한 예약된 값
} RGBQUAD;


int main()
{
	BITMAPFILEHEADER hf;    // BMP 파일헤더 14Bytes
	BITMAPINFOHEADER hInfo; // BMP 인포헤더 40Bytes
	RGBQUAD hRGB[256];      // 팔레트 (256 * 4Bytes)
	FILE* fp = NULL;

	// file open
	fp = fopen("LennaGray.bmp", "rb");
	if (fp == NULL) {
		printf("File NOT found\n");
		return 1;
	}
	// fopen() 대신에 fopen_s()를 사용하려면, 아래와 같이
	// 수업시간에는 fopen()을 그대로 사용하면서 프로젝트 환경 설정을 변경하였음
	//if (0 == fopen_s(&fp, "LennaGray.bmp", "rb"))
	//{
	//	printf("File NOT found\n");
	//	fclose(fp);
	//	return 1;
	//}
	else
	{
		fread(&hf, sizeof(BITMAPFILEHEADER), 1, fp);
		fread(&hInfo, sizeof(BITMAPINFOHEADER), 1, fp);
		fread(&hRGB, sizeof(RGBQUAD), 256, fp);

		int imgSize = hInfo.biWidth * hInfo.biHeight;
		BYTE* img = (BYTE*)malloc(imgSize);  // 1차원 배열로 취급
		fread(img, sizeof(BYTE), imgSize, fp);

		// print image information
		printf("hInfo.biSize=%d\n", hInfo.biSize);
		printf("hInfo.biWidth=%d\n", hInfo.biWidth);
		printf("hInfo.biHeight=%d\n", hInfo.biHeight);
		printf("hf.bfSize=%d\n", hf.bfSize);
		printf("hf.bfType=%hx", hf.bfType);
		printf("hInfo.biClrUsed=%d\n", hInfo.biClrUsed);
		printf("hInfo.biBitCount=%d\n", hInfo.biBitCount);

		// file close, memory release
		fclose(fp);
		free(img);
	}
	return 0;

}