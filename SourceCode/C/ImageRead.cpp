#include <stdio.h>
#include <stdlib.h>

// BMP����� �ޱ� ���� ����ü(struct)�� ������ ���
// �����ð����� ����ü �������� �ʰ� Windows.h�� include���Ѽ� �ۼ��� 
// (ImageReadWriteV2.cpp ����)

#define WORD    unsigned short       //2byte
#define DWORD   unsigned int         //4byte
#define LONG    unsigned int         //4byte
#define BYTE    unsigned char        //1byte

//BITMAPFILEHEADER Structure
typedef struct tagBITMAPFILEHEADER {  // ��Ʈ�� ���� ��� ����ü
	WORD  bfType;      // Specifies the file type ��Ʈ������ ���� Ȯ�� (e.g., 'BM' for BMP)
	DWORD bfSize;      // Specifies the size of the file in bytes ������ ũ��(����Ʈ)
	WORD  bfReserved1; // Reserved; must be 0 �̷��� ���� ����� ����
	WORD  bfReserved2; // Reserved; must be 0 �̷��� ���� ����� ����
	DWORD bfOffBits;   // Specifies the offset to the bitmap data (pixel array) ��Ʈ�� ����Ÿ ���� ��ġ
} BITMAPFILEHEADER;

// BITMAPINFOHEADER Structure
typedef struct tagBITMAPINFOHEADER {  // ��Ʈ�� ���� ��� ����ü
	DWORD biSize;          // Specifies the number of bytes required by the struct ���� ��Ʈ�� ���� ����� ũ��
	LONG  biWidth;         // Specifies the width of the bitmap in pixels �̹��� ���� ũ�� (�ȼ�)
	LONG  biHeight;        // Specifies the height of the bitmap in pixels �̹��� ���� ũ�� (�ȼ�)
	WORD  biPlanes;        // Specifies the number of color planes, must be 1 ����ϴ� �������� ��(�׻� 1)
	WORD  biBitCount;      // Specifies the number of bits per pixel (color depth) �ȼ� 1���� ǥ���ϴ� ��Ʈ ��
	DWORD biCompression;   // Specifies the type of compression (e.g., BI_RGB for no compression) ���� ���, ���� ��Ʈ���� �������� �ʾƼ� 0
	DWORD biSizeImage;     // Specifies the size of the image data in bytes �̹��� �ȼ� ������ ũ��
	LONG  biXPelsPerMeter; // Specifies the horizontal resolution in pixels per meter �̹��� ���� �ػ�
	LONG  biYPelsPerMeter; // Specifies the vertical resolution in pixels per meter �̹��� ���� �ػ�
	DWORD biClrUsed;       // Specifies the number of colors used by the bitmap ���� ���̺��� ���� ���Ǵ� ���� ��
	DWORD biClrImportant;  // Specifies the number of important colors ��Ʈ���� ǥ���ϱ� ���� �ʿ��� ���� �ε��� ��
} BITMAPINFOHEADER;

typedef struct tagRGBQUAD {  // 24bit ��Ʈ���� �ȼ� ����ü
	BYTE rgbBlue;     // �Ķ��� ����	
	BYTE rgbGreen;    // ��� ����
	BYTE rgbRed;      // ������ ����
	BYTE rgbeserved;  // Ȯ���� ���� ����� ��
} RGBQUAD;


int main()
{
	BITMAPFILEHEADER hf;    // BMP ������� 14Bytes
	BITMAPINFOHEADER hInfo; // BMP ������� 40Bytes
	RGBQUAD hRGB[256];      // �ȷ�Ʈ (256 * 4Bytes)
	FILE* fp = NULL;

	// file open
	fp = fopen("LennaGray.bmp", "rb");
	if (fp == NULL) {
		printf("File NOT found\n");
		return 1;
	}
	// fopen() ��ſ� fopen_s()�� ����Ϸ���, �Ʒ��� ����
	// �����ð����� fopen()�� �״�� ����ϸ鼭 ������Ʈ ȯ�� ������ �����Ͽ���
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
		BYTE* img = (BYTE*)malloc(imgSize);  // 1���� �迭�� ���
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