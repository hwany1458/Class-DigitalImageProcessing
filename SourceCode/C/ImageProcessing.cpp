#pragma warning(disable:4996)
#include <stdio.h>
#include <stdlib.h>
#include <Windows.h>

void main()
{
	BITMAPFILEHEADER hf; // BMP 파일헤더 14Bytes
	BITMAPINFOHEADER hInfo; // BMP 인포헤더 40Bytes
	RGBQUAD hRGB[256]; // 팔레트 (256 * 4Bytes)
	FILE* fp;

	// file open
	// (1) Open image
	fp = fopen("lenna.bmp", "rb");
	if (fp == NULL) return;

	fread(&hf, sizeof(BITMAPFILEHEADER), 1, fp);
	fread(&hInfo, sizeof(BITMAPINFOHEADER), 1, fp);
	fread(hRGB, sizeof(RGBQUAD), 256, fp);

	int ImgSize = hInfo.biWidth * hInfo.biHeight;
	BYTE* Image = (BYTE*)malloc(ImgSize);
	BYTE* Output = (BYTE*)malloc(ImgSize);

	fread(Image, sizeof(BYTE), ImgSize, fp);
	fclose(fp);
	
	/* 영상처리 ------- */

	for (int i = 0; i < ImgSize; i++) {
		Output[i] = Image[i]; // 항등 (Identity)
		//Output[i] = 255 - Image[i]; // 반전 (Negative)
	}

	/* ------- */

	// (2) Save image
	fp = fopen("output.bmp", "wb");
	fwrite(&hf, sizeof(BYTE), sizeof(BITMAPFILEHEADER), fp);
	fwrite(&hInfo, sizeof(BYTE), sizeof(BITMAPINFOHEADER), fp);
	fwrite(hRGB, sizeof(RGBQUAD), 256, fp);
	fwrite(Output, sizeof(BYTE), ImgSize, fp);

	// file close, memory release
	fclose(fp);
	free(Image);
	free(Output);
}