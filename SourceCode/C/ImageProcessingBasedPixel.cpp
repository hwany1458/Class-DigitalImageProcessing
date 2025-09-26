#include <stdio.h>
#include <stdlib.h>
#include <Windows.h>

#define imageLength 512

int main()
{
	BITMAPFILEHEADER hf;    // BMP ������� 14Bytes
	BITMAPINFOHEADER hInfo; // BMP ������� 40Bytes
	RGBQUAD hRGB[256];      // �ȷ�Ʈ (256 * 4Bytes)
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

		// ����ó��
		for (int i = 0; i < imgHeight; i++)
		{
			for (int j = 0; j < imgWidth; j++)
			{
				// �״�� ���� (image copy)
				// �����ð��� ImageReadWrite.cpp���� �۾��� ����
				outImg[i][j] = inImg[i][j];

				// ���� (�׸��ǿ����� �޴��� ����)
				// (1) ���δ�Ī�̵� (�¿��Ī)
				// (2) ���δ�Ī�̵� (���ϴ�Ī)
				// (3) ���������� 90�� ȸ�� (�ð���� ȸ��)
				// (4) �������� 90�� ȸ�� (�ݽð���� ȸ��)
				// (5) 180�� ȸ�� (�¿�-���� ��Ī�� ������ ����� ����)
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