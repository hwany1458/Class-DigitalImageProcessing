#include <stdio.h>
#include <stdlib.h>
#include <Windows.h>

#define imageLength 512

// 수업시간에는 구조체 선언하지 않고 
// (여기에서와 같이) Windows.h를 include시켜서 작성함 

int main()
{
	BITMAPFILEHEADER hf;     // BMP 파일헤더 14Bytes
	BITMAPINFOHEADER hInfo;  // BMP 인포헤더 40Bytes
	RGBQUAD hRGB[256];       // 팔레트 (256 * 4Bytes)
	FILE* fp = NULL;

	// 입력, 출력하기 위한 이미지 (2차원)배열
	unsigned char inImg[imageLength][imageLength];
	unsigned char outImg[imageLength][imageLength];

	// file open
	fp = fopen("LennaGray.bmp", "rb");
	if (fp == NULL) {
		printf("File NOT found\n");
		return 1;
	}
	else
	{
		fread(&hf, sizeof(BITMAPFILEHEADER), 1, fp);
		fread(&hInfo, sizeof(BITMAPINFOHEADER), 1, fp);
		fread(&hRGB, sizeof(RGBQUAD), 256, fp);

		/*
		int imgSize = hInfo.biWidth * hInfo.biHeight;
		BYTE* inImg = (BYTE*)malloc(imgSize);    // 1차원 배열로 취급
		fread(inImg, sizeof(BYTE), imgSize, fp);
		*/

		int imgWidth = hInfo.biWidth;
		int imgHeight = hInfo.biHeight;
		fread(inImg, imgWidth * imgHeight, 1, fp);

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
		// free(inImg);
	}
	return 0;

}