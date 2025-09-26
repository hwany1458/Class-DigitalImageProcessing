#include <stdio.h>
#include <stdlib.h>
#include <Windows.h>

#define imageLength 512

int main()
{
	BITMAPFILEHEADER hf;    // BMP 파일헤더 14Bytes
	BITMAPINFOHEADER hInfo; // BMP 인포헤더 40Bytes
	RGBQUAD hRGB[256];      // 팔레트 (256 * 4Bytes)
	FILE* fp = NULL;

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

		// read image
		int imgWidth = hInfo.biWidth;
		int imgHeight = hInfo.biHeight;
		fread(inImg, imgWidth * imgHeight, 1, fp);

		// 영상처리
		for (int i = 0; i < imgHeight; i++)
		{
			for (int j = 0; j < imgWidth; j++)
			{
				// 그대로 복사 (image copy)
				// 수업시간에 ImageReadWrite.cpp에서 작업한 내용
				outImg[i][j] = inImg[i][j];

				// 숙제 (그림판에서의 메뉴와 같이)
				// (1) 세로대칭이동 (좌우대칭)
				// (2) 가로대칭이동 (상하대칭)
				// (3) 오른쪽으로 90도 회전 (시계방향 회전)
				// (4) 왼쪽으로 90도 회전 (반시계방향 회전)
				// (5) 180도 회전 (좌우-상하 대칭과 동일한 결과가 나옴)
			}
		}

		// write image to file
		fp = fopen("output.bmp", "wb");
		if (fp == NULL)
		{
			printf("Output File Error\n");
			return 1;
		}
		else
		{
			fwrite(&hf, sizeof(BITMAPFILEHEADER), 1, fp);
			fwrite(&hInfo, sizeof(BITMAPINFOHEADER), 1, fp);
			fwrite(&hRGB, sizeof(RGBQUAD), 256, fp);
			fwrite(outImg, imgWidth * imgHeight, 1, fp);
		}

		// file close, memory release
		fclose(fp);
	}
	return 0;

}